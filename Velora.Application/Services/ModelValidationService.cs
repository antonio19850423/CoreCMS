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
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class ModelValidationService : IModelValidationService
    {
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IResourceCacheService _resourceCache;
        private readonly IGeneralContextService _generalContextService;
        private static readonly Regex EmailRegex =
    new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        private static readonly System.Text.RegularExpressions.Regex MobileRegex =
            new(@"^(?:\+98|0)?9\d{9}$");
        // E.164 – شماره موبایل جهانی

        private static readonly Regex ZipCodeRegex =
            new(@"^\d{5}(-\d{4})?$", RegexOptions.Compiled);
        private static readonly Regex NationalCodeRegex =
    new(@"^\d{10}$", RegexOptions.Compiled);
        public ModelValidationService(IResourceCacheService resourceCache, IGeneralContextService generalContextService, Lazy<ILocalizationMessageService> messageService)
        {
            _resourceCache = resourceCache;
            _generalContextService = generalContextService;
            _messageService = messageService;
        }

        public async Task<ResultDto<List<string>>> ValidateAsync<T>(T model) where T : class
        {
            var errors = new List<string>();

            if (model == null)
            {
                errors.Add(await _messageService.Value.GetMessageAsync(LocalizationKeys.ModelIsNull));
                return new ResultDto<List<string>> { Success = false, Data = errors };
            }

            // 1️⃣ گرفتن Resourceهای مربوط به مدل
            var entityName = typeof(T).Name;
            if (entityName.EndsWith("Crud", StringComparison.OrdinalIgnoreCase))
                entityName = entityName.Substring(0, entityName.Length - 4); // حذف 'Crud'
            entityName = entityName.ToUpper() + ".";
            var allResourcesResult = await _resourceCache.GetResourcesAsync(_generalContextService.CurrentLanguage, entityName);

            if (!allResourcesResult.Success || allResourcesResult.Data == null)
            {
                return new ResultDto<List<string>>
                {
                    Success = false,
                    Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.LoadResources),
                    Data = new List<string>()
                };
            }

            // فقط Resourceهای FIELD و ShowInForm = true و ResourceCode شروع با EntityName.
            var resources = allResourcesResult.Data
                .Where(r =>
                    r.ResourceTypeCode?.Equals("FIELD", StringComparison.OrdinalIgnoreCase) == true
                    && r.ShowInForm == true
                    && !string.IsNullOrEmpty(r.ResourceCode))
                .ToList();

            if (!resources.Any())
            {
                return new ResultDto<List<string>>
                {
                    Success = false,
                    Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.NoResources),
                    Data = new List<string>()
                };
            }

            // 2️⃣ بررسی هر فیلد مدل
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var resource = resources.FirstOrDefault(r =>
                     r.ResourceCode.Equals(entityName + prop.Name, StringComparison.OrdinalIgnoreCase));

                if (resource == null)
                    continue; // Resource تعریف نشده، رد شود
                if(resource.FieldType==FieldTypes.MultiSelectBox)
                {

                }
                var value = prop.GetValue(model);

                // ⚡️ اعتبارسنجی Required
                if (resource.IsRequired == true)
                {
                    if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                    {
                        var propsTitle = await _resourceCache.GetResourcesAsync(_generalContextService.CurrentLanguage, entityName + prop.Name);
                        var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.Required);
                        var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                        errors.Add(string.Format(error, fieldName));
                        continue;
                    }
                }

                // ⚡️ اعتبارسنجی MaxLength
                if (value is string strVal && resource.MaxLength.HasValue && resource.MaxLength>0)
                {
                    if (strVal.Length > resource.MaxLength.Value)
                    {
                        var propsTitle = await _resourceCache.GetResourcesAsync(_generalContextService.CurrentLanguage, entityName + prop.Name);
                        var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.MaxLength);
                        var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                        errors.Add(string.Format(error, fieldName, resource.MaxLength.Value));
                    }
                }

                // ⚡️ اعتبارسنجی Number
                if (resource.FieldType?.Equals("Number", StringComparison.OrdinalIgnoreCase) == true && value != null)
                {
                    if (!int.TryParse(value.ToString(), out _))
                    {
                        var propsTitle = await _resourceCache.GetResourcesAsync(_generalContextService.CurrentLanguage, entityName + prop.Name);
                        var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.Number);
                        var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                        errors.Add(string.Format(error, fieldName));
                    }
                }

                // ⚡️ اعتبارسنجی Checkbox
                if (resource.FieldType?.Equals("Checkbox", StringComparison.OrdinalIgnoreCase) == true && value != null)
                {
                    if (value is not bool)
                    {
                        var propsTitle = await _resourceCache.GetResourcesAsync(_generalContextService.CurrentLanguage, entityName + prop.Name);
                        var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.Checkbox);
                        var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                        errors.Add(string.Format(error, fieldName));
                    }
                }


                if (resource.InputMask?.Equals("Email", StringComparison.OrdinalIgnoreCase) == true
    && value is string email && !EmailRegex.IsMatch(email))
                {
                    var propsTitle = await _resourceCache.GetResourcesAsync(
                        _generalContextService.CurrentLanguage, entityName + prop.Name);

                    var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                    var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.InvalidField);

                    errors.Add(string.Format(error, fieldName));
                }
                if (resource.InputMask?.Equals("Mobile", StringComparison.OrdinalIgnoreCase) == true
    && value is string mobile && !MobileRegex.IsMatch(mobile))
                {
                    var propsTitle = await _resourceCache.GetResourcesAsync(
                        _generalContextService.CurrentLanguage, entityName + prop.Name);

                    var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                    var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.InvalidField);

                    errors.Add(string.Format(error, fieldName));
                }
                if (resource.InputMask?.Equals("NationalCode", StringComparison.OrdinalIgnoreCase) == true
                    && value is string nationalCode && !NationalCodeRegex.IsMatch(nationalCode))
                {
                    var propsTitle = await _resourceCache.GetResourcesAsync(
                        _generalContextService.CurrentLanguage, entityName + prop.Name);

                    var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                    var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.InvalidField);

                    errors.Add(string.Format(error, fieldName));
                }
                if (resource.InputMask?.Equals("ZipCode", StringComparison.OrdinalIgnoreCase) == true
                    && value is string zip && !ZipCodeRegex.IsMatch(zip))
                {
                    var propsTitle = await _resourceCache.GetResourcesAsync(
                        _generalContextService.CurrentLanguage, entityName + prop.Name);

                    var fieldName = propsTitle.Data?.FirstOrDefault()?.Name ?? prop.Name;
                    var error = await _messageService.Value.GetMessageAsync(LocalizationKeys.InvalidField);

                    errors.Add(string.Format(error, fieldName));
                }

                // ⚡️ TODO: سایر FieldType ها مثل Date, SelectBox, Autocomplete
            }

            return new ResultDto<List<string>>
            {
                Success = !errors.Any(),
                Data = errors
            };
        }

    }
}
