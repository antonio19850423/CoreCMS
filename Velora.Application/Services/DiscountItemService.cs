using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class DiscountItemService : GenericService<SqlDiscountItem, SqlDiscountItem, DiscountItemDto>, IDiscountItemService
    {
        private readonly ISqlRepository<SqlDiscountItem> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IDiscountItemService _roleDiscountItemService;
        protected readonly ICurrentUserService _currentUserService;
        public DiscountItemService(ISqlRepository<SqlDiscountItem> sqlRepository, IPosgreSqlRepository<SqlDiscountItem> pgRepository, IMapper mapper, IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env, Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService, ICurrentUserService currentUserService) : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _messageService = messageService;
            _modelValidationService = modelValidationService;
            _env = env;
            _config = config;
            _excelTemplateService = excelTemplateService;
            _currentUserService = currentUserService;
        }

        public async Task<IQueryable<DiscountItemCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwDiscountItemForm, VwDiscountItemForm, DiscountItemCrud>();
        }

        public async Task<ResultDto<DiscountItemDto>> CreateAsync(DiscountItemCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<DiscountItemDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var validationMessage = await ValidateDiscountItemAsync(input.ParentId.Value, input.ProductVariantId, input.ProductId, input.ProductBrandId, input.ProductCategoryId);
                if (validationMessage != null)
                {
                    return new ResultDto<DiscountItemDto>
                    {
                        Success = false,
                        Message = validationMessage
                    };
                }

                var discountItem = new DiscountItemDto
                {
                    DiscountId = input.ParentId.Value,
                    ProductId = input.ProductId == Guid.Empty ? null : input.ProductId,
                    ProductBrandId = input.ProductBrandId == Guid.Empty ? null : input.ProductBrandId,
                    ProductCategoryId = input.ProductCategoryId == Guid.Empty ? null : input.ProductCategoryId,
                    ProductVariantId = input.ProductVariantId == Guid.Empty ? null : input.ProductVariantId,
                    SortOrder = input.SortOrder,
                };
                var result = await CreateAsync(discountItem);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<DiscountItemDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<DiscountItemCrud>> CreateAdvancedAsync(DiscountItemCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.ProductVariantId.HasValue && input.ProductVariantId != Guid.Empty)
                {
                    input.ProductId = Guid.Empty;
                    input.ProductBrandId = null;
                    input.ProductCategoryId = null;
                }
                else if (input.ProductId != Guid.Empty)
                {
                    input.ProductVariantId = null;
                    input.ProductBrandId = null;
                    input.ProductCategoryId = null;
                }
                else if (input.ProductBrandId.HasValue && input.ProductBrandId != Guid.Empty)
                {
                    input.ProductVariantId = null;
                    input.ProductCategoryId = null;
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var validationMessage = await ValidateDiscountItemAsync(input.ParentId.Value, input.ProductVariantId, input.ProductId, input.ProductBrandId, input.ProductCategoryId);
                if (validationMessage != null)
                {
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = validationMessage
                    };
                }

                var discountItem = new DiscountItemDto
                {
                    DiscountId = input.ParentId.Value,
                    ProductId = input.ProductId == Guid.Empty ? null : input.ProductId,
                    ProductBrandId = input.ProductBrandId == Guid.Empty ? null : input.ProductBrandId,
                    ProductCategoryId = input.ProductCategoryId == Guid.Empty ? null : input.ProductCategoryId,
                    ProductVariantId = input.ProductVariantId == Guid.Empty ? null : input.ProductVariantId,
                    SortOrder = input.SortOrder
                };
                if (discountItem.SortOrder <= 0)
                {
                    discountItem.SortOrder = (await Query().MaxAsync(x => (int?)x.SortOrder) ?? 0) + 1;
                }
                var result = await CreateWithViewAsync(discountItem, GetDiscountItemById, x => x.Id);
                if (result.Success)
                    result.Message = successMessage;
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<DiscountItemCrud>
                {
                    Success = false,
                    Message = errorMessage
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<DiscountItemCrud>> UpdateAsync(DiscountItemCrud input)
        {
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }

                if (input.ProductVariantId.HasValue && input.ProductVariantId != Guid.Empty)
                {
                    input.ProductId = Guid.Empty;
                    input.ProductBrandId = null;
                    input.ProductCategoryId = null;
                }
                else if (input.ProductId != Guid.Empty)
                {
                    input.ProductVariantId = null;
                    input.ProductBrandId = null;
                    input.ProductCategoryId = null;
                }
                else if (input.ProductBrandId.HasValue && input.ProductBrandId != Guid.Empty)
                {
                    input.ProductVariantId = null;
                    input.ProductCategoryId = null;
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var validationMessage = await ValidateDiscountItemAsync(input.ParentId.Value, input.ProductVariantId, input.ProductId, input.ProductBrandId, input.ProductCategoryId);
                if (validationMessage != null)
                {
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = validationMessage
                    };
                }

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new DiscountItemDto
                {
                    Id = input.Id,
                    DiscountId = input.ParentId.Value,
                    ProductId = input.ProductId == Guid.Empty ? null : input.ProductId,
                    ProductBrandId = input.ProductBrandId == Guid.Empty ? null : input.ProductBrandId,
                    ProductCategoryId = input.ProductCategoryId == Guid.Empty ? null : input.ProductCategoryId,
                    ProductVariantId = input.ProductVariantId == Guid.Empty ? null : input.ProductVariantId,
                    SortOrder = input.SortOrder,
                };
             
                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                {
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = result.Message,
                        Errors = result.Errors
                    };
                }

                await _transactionService.CommitAsync();
                var viewResult = await GetDiscountItemById(input.Id);
                if (viewResult == null)
                {
                    return new ResultDto<DiscountItemCrud>
                    {
                        Success = false,
                        Message = "اطلاعات پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<DiscountItemCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<DiscountItemCrud>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdDiscountItems = new List<DiscountItemDto>();
            var errors = new List<string>();
            var(successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var(dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var DiscountItems = dt.ToModelList<DiscountItemCrud>();
                for (int i = 0; i < DiscountItems.Count; i++)
                {
                    var DiscountItem = DiscountItems[i];
                    var context = rowContexts[i];
                    var createResult = await CreateAsync(DiscountItem);
                    if (createResult.Success && createResult.Data != null)
                    {
                        createdDiscountItems.Add(createResult.Data);
                    }
                    else
                    {
                        string errorMsg = createResult.Errors != null && createResult.Errors.Any() ? string.Join("; ", createResult.Errors) : !string.IsNullOrWhiteSpace(createResult.Message) ? createResult.Message : "Unknown error";
                        dt.SetRowError(context.DataTableRowIndex, errorMsg);
                        errors.Add($"Row {context.ExcelRowNumber}: {errorMsg}");
                    }
                }

                await _transactionService.CommitAsync();
                string? errorFileUrl = null;
                if (errors.Any())
                {
                    errorFileUrl = dt.SaveErrorExcel(_env.WebRootPath!, _config);
                }

                return new ResultDto<BulkInsertResult>
                {
                    Success = errors.Count == 0,
                    Message = errors.Count == 0 ? successMessage : errorFileTitle,
                    Data = new BulkInsertResult
                    {
                        InsertedCount = createdDiscountItems.Count,
                        ErrorCount = errors.Count,
                        ErrorFileUrl = errorFileUrl
                    },
                    Errors = errors
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                return new ResultDto<BulkInsertResult>
                {
                    Success = false,
                    Message = errorFileTitle,
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }

        private async Task<string?> ValidateDiscountItemAsync(Guid discountId, Guid? productVariantId, Guid? productId, Guid? productBrandId, Guid? productCategoryId, Guid? excludeId = null)
        {
            // =========================================================
            // 1. Guid.Empty را مثل null در نظر می‌گیریم
            // =========================================================
            productVariantId = productVariantId == Guid.Empty ? null : productVariantId;
            productId = productId == Guid.Empty ? null : productId;
            productBrandId = productBrandId == Guid.Empty ? null : productBrandId;
            productCategoryId = productCategoryId == Guid.Empty ? null : productCategoryId;
            // =========================================================
            // 2. فقط یکی از Targetها باید انتخاب شده باشد
            // =========================================================
            var selectedTargets = new[]
            {
                productVariantId.HasValue,
                productId.HasValue,
                productBrandId.HasValue,
                productCategoryId.HasValue
            };
            var selectedCount = selectedTargets.Count(x => x);
            // هیچ Targetی انتخاب نشده
            if (selectedCount == 0)
            {
                return "یکی از واریانت، محصول، برند یا دسته‌بندی باید انتخاب شود.";
            }

            // بیشتر از یک Target انتخاب شده
            if (selectedCount > 1)
            {
                return "در هر آیتم تخفیف فقط یکی از واریانت، محصول، برند یا دسته‌بندی می‌تواند انتخاب شود.";
            }

            // =========================================================
            // 3. فقط آیتم‌های همین Discount بررسی شوند
            // =========================================================
            var query = Query().Where(x => x.DiscountId == discountId && (!excludeId.HasValue || x.Id != excludeId.Value));
            // =========================================================
            // 4. Product Variant
            // =========================================================
            if (productVariantId.HasValue)
            {
                var exists = await query.AnyAsync(x => x.ProductVariantId == productVariantId.Value);
                if (exists)
                {
                    return "این واریانت قبلاً برای این تخفیف ثبت شده است.";
                }

                return null;
            }

            // =========================================================
            // 5. Product
            // =========================================================
            if (productId.HasValue)
            {
                var exists = await query.AnyAsync(x => x.ProductId == productId.Value);
                if (exists)
                {
                    return "این محصول قبلاً برای این تخفیف ثبت شده است.";
                }

                return null;
            }

            // =========================================================
            // 6. Brand
            // =========================================================
            if (productBrandId.HasValue)
            {
                var exists = await query.AnyAsync(x => x.ProductBrandId == productBrandId.Value);
                if (exists)
                {
                    return "این برند قبلاً برای این تخفیف ثبت شده است.";
                }

                return null;
            }

            // =========================================================
            // 7. Category
            // =========================================================
            if (productCategoryId.HasValue)
            {
                var exists = await query.AnyAsync(x => x.ProductCategoryId == productCategoryId.Value);
                if (exists)
                {
                    return "این دسته‌بندی قبلاً برای این تخفیف ثبت شده است.";
                }

                return null;
            }

            return null;
        }

        public async Task<byte[]> ExportAsync(bool exportCurrentDiscountItem, int DiscountItemNumber, int DiscountItemSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>
            // 2️⃣ Paging و Mapping به DTO
            List<DiscountItemCrud> data;
            if (exportCurrentDiscountItem)
            {
                data = query.Skip((DiscountItemNumber - 1) * DiscountItemSize).Take(DiscountItemSize).ToList();
            }
            else
            {
                data = query.ToList();
            }

            var resource = _mapper.Map<List<DiscountItemCrud>>(data);
            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(LookupEntities.DiscountItem, // نام مدل DTO
 data.Count + 5);
            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);
            return resultBytes;
        }

        public async Task<DiscountItemCrud?> GetDiscountItemById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlDiscountItemView, DiscountItemCrud>(id);
        }
    }
}