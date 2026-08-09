using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class DiscountService : GenericService<SqlDiscount, SqlDiscount, DiscountDto>, IDiscountService
    {
        private readonly ISqlRepository<SqlDiscount> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IDiscountService _roleDiscountService;
        protected readonly ICurrentUserService _currentUserService;
        public DiscountService(
              ISqlRepository<SqlDiscount> sqlRepository,
              IPosgreSqlRepository<SqlDiscount> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService)
              : base(sqlRepository, pgRepository, mapper, configuration, messageService, currentUserService)
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
        public async Task<IQueryable<DiscountCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlDiscountView, SqlDiscountView, DiscountCrud>();
        }

        public async Task<ResultDto<DiscountDto>> CreateAsync(DiscountCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                if (input.StartDate >= input.EndDate)
                {
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = "تاریخ پایان باید بعد از تاریخ شروع باشد."
                    };
                }

                var hasOverlap = await HasDateOverlapAsync(
                    input.StartDate,
                    input.EndDate
                );

                if (hasOverlap)
                {
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = "بازه زمانی این تخفیف با یک تخفیف فعال دیگر تداخل دارد."
                    };
                }

                var Discount = new DiscountDto
                {
                   DiscountType = input.DiscountType,
                   DiscountValue = input.DiscountValue,
                   EndDate = input.EndDate,
                   IsActive = input.IsActive,
                   Name = input.Name,
                   StartDate = input.StartDate,
                };

                var DiscountResult = await CreateAsync(Discount);
                if (!DiscountResult.Success)
                    return DiscountResult;

                await _transactionService.CommitAsync();
                return DiscountResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<DiscountDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<DiscountDto>> UpdateAsync(DiscountCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                if (input.StartDate >= input.EndDate)
                {
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = "تاریخ پایان باید بعد از تاریخ شروع باشد."
                    };
                }

                var hasOverlap = await HasDateOverlapAsync(
                    input.StartDate,
                    input.EndDate,
                    input.Id
                );

                if (hasOverlap)
                {
                    return new ResultDto<DiscountDto>
                    {
                        Success = false,
                        Message = "بازه زمانی این تخفیف با یک تخفیف فعال دیگر تداخل دارد."
                    };
                }
                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new DiscountDto
                {
                    Id = input.Id,
                    DiscountType = input.DiscountType,
                    DiscountValue = input.DiscountValue,
                    EndDate = input.EndDate,
                    IsActive = input.IsActive,
                    Name = input.Name,
                    StartDate = input.StartDate,
                };

                var DiscountResult = await UpdateAsync(updateDto, input.Id);
                if (!DiscountResult.Success)
                    return DiscountResult;

                await _transactionService.CommitAsync();
                return DiscountResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<DiscountDto>
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
            var createdDiscounts= new List<DiscountDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Discounts = dt.ToModelList<DiscountCrud>();

                for (int i = 0; i < Discounts.Count; i++)
                {
                    var Discount = Discounts[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(Discount);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdDiscounts.Add(createResult.Data);
                    }
                    else
                    {
                        string errorMsg =
                            createResult.Errors != null && createResult.Errors.Any()
                                ? string.Join("; ", createResult.Errors)
                                : !string.IsNullOrWhiteSpace(createResult.Message)
                                    ? createResult.Message
                                    : "Unknown error";

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
                    Message = errors.Count == 0
                        ? successMessage
                        : errorFileTitle,
                    Data = new BulkInsertResult
                    {
                        InsertedCount = createdDiscounts.Count,
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
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<List<ActiveDiscountDto>> GetActiveDiscountsAsync()
        {
            var now = DateTime.Now;

            var discounts = await Query()
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    x.StartDate <= now &&
                    x.EndDate >= now)
                .Include(x => x.DiscountItems)
                .ToListAsync();

            return discounts
                .Select(x => new ActiveDiscountDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DiscountType = x.DiscountType,
                    DiscountValue = x.DiscountValue,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,

                    Items = x.DiscountItems
                        .Select(item => new ActiveDiscountItemDto
                        {
                            Id = item.Id,
                            DiscountId = item.DiscountId,

                            ProductId = item.ProductId,
                            ProductVariantId = item.ProductVariantId,
                            ProductBrandId = item.ProductBrandId,
                            ProductCategoryId = item.ProductCategoryId,

                            SortOrder = item.SortOrder
                        })
                        .ToList()
                })
                .ToList();
        }
        public DiscountCalculationResultDto CalculateDiscount(
            DiscountCalculationInput input,
            IReadOnlyList<ActiveDiscountDto> activeDiscounts)
        {
            var candidates = activeDiscounts
                .SelectMany(discount =>
                    discount.Items.Select(item => new
                    {
                        Discount = discount,
                        Item = item
                    }))
                .Where(x =>
                    (x.Item.ProductVariantId.HasValue &&
                     x.Item.ProductVariantId == input.ProductVariantId)

                    ||

                    (x.Item.ProductId.HasValue &&
                     x.Item.ProductId == input.ProductId)

                    ||

                    (x.Item.ProductBrandId.HasValue &&
                     x.Item.ProductBrandId == input.ProductBrandId)

                    ||

                    (x.Item.ProductCategoryId.HasValue &&
                     x.Item.ProductCategoryId == input.ProductCategoryId)
                )
                .ToList();

            if (!candidates.Any())
            {
                return new DiscountCalculationResultDto
                {
                    HasDiscount = false,
                    OriginalPrice = input.Price,
                    FinalPrice = input.Price
                };
            }

            // =========================================================
            // اولویت تخفیف
            //
            // Variant  = 4
            // Product  = 3
            // Brand    = 2
            // Category = 1
            // =========================================================

            var selected = candidates
                .OrderByDescending(x =>
                    x.Item.ProductVariantId.HasValue ? 4 :
                    x.Item.ProductId.HasValue ? 3 :
                    x.Item.ProductBrandId.HasValue ? 2 :
                    x.Item.ProductCategoryId.HasValue ? 1 : 0)
                .ThenBy(x => x.Item.SortOrder)
                .First();

            var discount = selected.Discount;

            decimal discountAmount;

            // =========================================================
            // محاسبه مقدار تخفیف
            // =========================================================

            if (discount.DiscountType == 1)
            {
                // درصدی
                discountAmount =
                    input.Price * discount.DiscountValue / 100m;
            }
            else
            {
                // مبلغ ثابت
                discountAmount =
                    discount.DiscountValue;
            }

            // تخفیف نباید بیشتر از قیمت محصول باشد
            discountAmount = Math.Min(
                discountAmount,
                input.Price);

            var finalPrice =
                input.Price - discountAmount;

            return new DiscountCalculationResultDto
            {
                HasDiscount = true,

                DiscountId = discount.Id,

                DiscountItemId = selected.Item.Id,

                DiscountType = discount.DiscountType,

                DiscountValue = discount.DiscountValue,

                OriginalPrice = input.Price,

                DiscountAmount = discountAmount,

                FinalPrice = finalPrice
            };
        }
        public async Task<byte[]> ExportAsync(
bool exportCurrentDiscount,
int DiscountNumber,
int DiscountSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<DiscountCrud> data;

            if (exportCurrentDiscount)
            {
                data = query
                    .Skip((DiscountNumber - 1) * DiscountSize)
                    .Take(DiscountSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<DiscountCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.Discount, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        private async Task<bool> HasDateOverlapAsync(
    DateTime startDate,
    DateTime endDate,
    Guid? excludeId = null)
        {
            return await Query()
                .AnyAsync(x =>
                    x.IsActive &&
                    (!excludeId.HasValue || x.Id != excludeId.Value) &&

                    // بررسی تداخل دو بازه زمانی
                    x.StartDate <= endDate &&
                    x.EndDate >= startDate
                );
        }
    }

}
