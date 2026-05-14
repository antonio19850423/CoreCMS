using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{


    public class ExcelTemplateService : IExcelTemplateService
    {
        private readonly IResourceCacheService _resourceCache;
        private readonly IGeneralContextService _generalContextService;
        private readonly IResourceTypeService _resourceTypeService;
        private readonly IResourceService _resourceService;
        private readonly IRoleService _roleService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;

        public ExcelTemplateService(
            IResourceCacheService resourceCache,
            IGeneralContextService generalContextService,
            IResourceTypeService resourceTypeService,
            IResourceService resourceService,
            IRoleService roleService,
            Lazy<ILocalizationMessageService> messageService)
        {
            _resourceCache = resourceCache;
            _generalContextService = generalContextService;
            _resourceTypeService = resourceTypeService;
            _resourceService = resourceService;
            _roleService = roleService;
            _messageService = messageService;
        }

        public async Task<byte[]> GenerateTemplateWithLookupsAsync(string entityName, int emptyRows = 100)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new ArgumentException("Entity name is required.", nameof(entityName));

            var language = _generalContextService.CurrentLanguage;

            // پیدا کردن Type مدل
            var modelType = ModelMapping.GetModelType(entityName);
            if (modelType == null)
                throw new InvalidOperationException($"Model '{entityName}' not found.");

            var resourcePrefix = modelType.Name.EndsWith("Crud", StringComparison.OrdinalIgnoreCase)
                ? modelType.Name[..^4].ToUpper() + "."
                : modelType.Name.ToUpper() + ".";

            var resourcesResult = await _resourceCache.GetResourcesAsync(language, resourcePrefix);
            var resources = resourcesResult.Data?
                .Where(r =>
                    r.ResourceTypeCode == "FIELD" &&
                    r.ShowInForm == true &&
                    !string.IsNullOrEmpty(r.ResourceCode) &&
                    r.ResourceCode.ToUpper().StartsWith(resourcePrefix))
                .OrderBy(r => r.FormOrder)
                .ToList() ?? new();

            if (!resources.Any())
                throw new InvalidOperationException($"No fields found for entity '{entityName}'.");

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(resourcePrefix.TrimEnd('.'));
            if (language.Equals("fa", StringComparison.OrdinalIgnoreCase))
            {
                sheet.RightToLeft = true;
            }
            // ستون ShouldInsert به عنوان اولین ستون
            var shouldInsertTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ShouldInsert);
            sheet.Cell(1, 1).Value = "ShouldInsert"; // نام پراپرتی مدل (برای خواندن Excel)
            sheet.Cell(2, 1).Value = shouldInsertTitle; // عنوان نمایشی برای کاربر
            var shouldInsertRange = sheet.Range(2, 1, emptyRows + 1, 1);
            var dv = shouldInsertRange.CreateDataValidation();
            dv.List("\"TRUE,FALSE\"");
            dv.IgnoreBlanks = true;
            dv.InCellDropdown = true;
            dv.ShowErrorMessage = true;
            dv.ErrorStyle = XLErrorStyle.Stop;
            dv.ErrorMessage = "فقط TRUE یا FALSE مجاز است";
            int col = 2;

            foreach (var res in resources)
            {
                var propertyName = res.ResourceCode!.Replace(resourcePrefix, "", StringComparison.OrdinalIgnoreCase);
                var entityLookUp = res.EntityName;
                // ستون نمایشی (ComboBox) با نام ترجمه شده
                sheet.Cell(2, col).Value = res.Name ?? propertyName;

                ApplyColumnFormat(sheet.Column(col), res);
                ApplyColumnValidation(sheet, col, res, emptyRows);
                // ردیف اول = نام پراپرتی واقعی مدل
                sheet.Cell(1, col).Value =
                    !string.IsNullOrWhiteSpace(res.Route)
                        ? res.LinkedFieldCode     // برای Excel خواندن
                        : propertyName;

       
                // اگر Route تعریف شده باشد، Lookup بساز
                if (!string.IsNullOrWhiteSpace(res.EntityName))
                {
                    // ستون مخفی (Id واقعی که به مدل Cast می‌شود)
                    var hiddenCol = col + 1;
                    sheet.Cell(1, hiddenCol).Value = propertyName; // 👈 همیشه پراپرتی مدل
                    sheet.Column(hiddenCol).Hide();
                    var lookupData = await GetLookupDataAsync(entityLookUp);
                    if (lookupData != null && lookupData.Any())
                    {
                        var lookupSheetName = $"{propertyName}_Lookup";
                        var lookupSheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == lookupSheetName)
                            ?? workbook.Worksheets.Add(lookupSheetName);

                        lookupSheet.Cell(1, 1).Value = "Id";
                        lookupSheet.Cell(1, 2).Value = "Display";

                        int lookupRow = 2;
                        foreach (var item in lookupData)
                        {
                            lookupSheet.Cell(lookupRow, 1).Value = item.Value.ToString();
                            lookupSheet.Cell(lookupRow, 2).Value = item.Label;
                            lookupRow++;
                        }

                        // Data Validation ستون اصلی
                        var validationRange =
                            sheet.Range(2, col, emptyRows + 1, col).CreateDataValidation();

                        validationRange.List($"'{lookupSheetName}'!$B$2:$B${lookupRow - 1}");
                        validationRange.IgnoreBlanks = true;
                        validationRange.InCellDropdown = true;

                        // ستون Id را با INDEX/MATCH مقداردهی خودکار کن
                        for (int row = 2; row <= emptyRows + 1; row++)
                        {
                            var formula = $"=IF({sheet.Cell(row, col).Address}= \"\", \"\", INDEX('{lookupSheetName}'!A:A, MATCH({sheet.Cell(row, col).Address}, '{lookupSheetName}'!B:B, 0)))";
                            sheet.Cell(row, hiddenCol).FormulaA1 = formula;
                        }

                        lookupSheet.Visibility = XLWorksheetVisibility.Visible;
                    }
                }

                if (!string.IsNullOrWhiteSpace(res.EntityName))
                {
                    // کد ایجاد ستون مخفی و Lookup

                    col += 2; // Display + HiddenId
                }
                else
                {
                    col += 1; // فقط Display، بدون ستون خالی
                }
            }

            // ردیف اول (نام پراپرتی‌ها) مخفی شود
            sheet.Row(1).Hide();

            sheet.Range(2, 1, emptyRows + 1, col - 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }


        // این متد باید داده‌ها را از Route (مثلا API) دریافت کند و به صورت Id+Display برگرداند
        private async Task<List<ComboBoxItemDto<Guid>>> GetLookupDataAsync(string resourceCode)
        {
            // نمونه ساده، در عمل می‌توانی IGenericService یا HttpClient بگیری
            if (resourceCode.Equals(LookupEntities.ResourceType, StringComparison.OrdinalIgnoreCase))
            {
                var resourceTypes = await _resourceTypeService.GetAllQuery();

                var resourceItems = await resourceTypes.ToComboBoxItemsAsync(
                                                x => x.Id,    // Value
                                                x => x.Name   // Label
                                            );
                return resourceItems;
            }
            if (resourceCode.Equals(LookupEntities.Resource, StringComparison.OrdinalIgnoreCase))
            {
                var resources = await _resourceService.GetAllQuery();

                var resourceItems = await resources.ToComboBoxItemsAsync(
                                                x => x.Id,    // Value
                                                x => x.Code   // Label
                                            );
                return resourceItems;
            }
            if (resourceCode.Equals(LookupEntities.Role, StringComparison.OrdinalIgnoreCase))
            {
                var roles = await _roleService.GetAllQuery();

                var roleItems = await roles.ToComboBoxItemsAsync(
                                                x => x.Id,    // Value
                                                x => x.Code   // Label
                                            );
                return roleItems;
            }

            // سایر Routeها...
            return new();
        }
        private void ApplyColumnValidation(
            IXLWorksheet sheet,
            int col,
            ResourcesViewDto resource,
            int emptyRows)
        {
            // اگر Lookup دارد، اینجا Validation نزن
            if (!string.IsNullOrWhiteSpace(resource.EntityName))
                return;

            var range = sheet.Range(2, col, emptyRows + 1, col);

            // ❗ اگر قبلاً Validation دارد، همان را بگیر
            var dv = range.GetDataValidation() ?? range.CreateDataValidation();

            switch (resource.FieldType)
            {
                case "Number":
                    dv.WholeNumber.Between(-999999999, 999999999);
                    dv.ErrorMessage = "فقط عدد مجاز است";
                    break;

                case "Date":
                    dv.Date.Between(
                        new DateTime(1900, 1, 1),
                        new DateTime(2100, 12, 31)
                    );
                    dv.ErrorMessage = "فرمت تاریخ معتبر نیست";
                    break;

                case "Checkbox":
                    dv.List("\"TRUE,FALSE\"");
                    dv.ErrorMessage = "فقط TRUE یا FALSE مجاز است";
                    break;

                default:
                    dv.TextLength.Between(0, 500);
                    dv.ErrorMessage = "طول متن بیش از حد مجاز است";
                    break;
            }

            dv.ShowErrorMessage = true;
            dv.ErrorStyle = XLErrorStyle.Stop;
        }




        private void ApplyColumnFormat(IXLColumn column, ResourcesViewDto resource)
        {
            switch (resource.FieldType)
            {
                case "Number":
                    column.Style.NumberFormat.Format = "0";
                    break;

                case "Checkbox":
                    column.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    break;

                case "Date":
                    column.Style.DateFormat.Format = "yyyy-MM-dd";
                    break;

                default:
                    column.Style.NumberFormat.Format = "@"; // Text
                    break;
            }
        }
    }

}
