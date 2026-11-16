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
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Repositories;
using Velora.Application.Shared.Services;
using Velora.Infrastructure.ORM.Interfaces.MyApp.Orm.Interfaces;

namespace Velora.Application.Services
{
    public class ProductReviewService : GenericService<SqlProductReview, SqlProductReview, ProductReviewDto>, IProductReviewService
    {
        private readonly ISqlRepository<SqlProductReview> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductReviewService _roleProductReviewService;
        protected readonly ICurrentUserService _currentUserService;
        public ProductReviewService(
              ISqlRepository<SqlProductReview> sqlRepository,
              IPosgreSqlRepository<SqlProductReview> pgRepository,
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
        public async Task<IQueryable<ProductReviewCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlProductReviewView, SqlProductReviewView, ProductReviewCrud>();
        }

        public async Task<ResultDto<ProductReviewDto>> CreateAsync(ProductReviewCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductReviewDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var ProductReview = new ProductReviewDto
                {
                    Title = input.Title,
                    Comment = input.Comment,
                    IsApproved  = input.IsApproved,
                    UserId = input.UserId,
                    ProductId = input.ProductId,
                    Rate = input.Rate,

                };

                var ProductReviewResult = await CreateAsync(ProductReview);
                await _transactionService.CommitAsync();
                return ProductReviewResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductReviewDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ProductReviewDto>> UpdateAsync(ProductReviewCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ProductReviewDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductReviewDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new ProductReviewDto
                {
                    Id = input.Id,
                    Title = input.Title,
                    Comment = input.Comment,
                    IsApproved = input.IsApproved,
                    UserId = input.UserId,
                    ProductId = input.ProductId,
                    Rate = input.Rate,
                };

                var ProductReviewResult = await UpdateAsync(updateDto, input.Id);
                await _transactionService.CommitAsync();
                return ProductReviewResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductReviewDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }
        public async Task<ResultDto<ProductReviewDto>> CreateUserReviewAsync(
            CreateProductReviewDto input)
        {
            var (successMessage, errorMessage) =
                await _messageService.Value.GetSaveMessagesAsync();

            try
            {
                // ========================================
                // Validation
                // ========================================

                if (input == null)
                {
                    throw new BusinessException(
                        "اطلاعات نظر ارسال نشده است.");
                }

                // ProductId
                if (input.ProductId == Guid.Empty)
                {
                    throw new BusinessException(
                        "محصول انتخاب نشده است.");
                }

                // Rate
                if (input.Rate < 1 || input.Rate > 5)
                {
                    throw new BusinessException(
                        "امتیاز باید بین ۱ تا ۵ باشد.");
                }

                // Comment
                if (string.IsNullOrWhiteSpace(input.Comment))
                {
                    throw new BusinessException(
                        "متن نظر الزامی است.");
                }

                // حذف فاصله‌های ابتدا و انتهای متن
                input.Comment = input.Comment.Trim();

                // محدودیت طول نظر
                if (input.Comment.Length < 3)
                {
                    throw new BusinessException(
                        "متن نظر باید حداقل ۳ کاراکتر باشد.");
                }

                if (input.Comment.Length > 2000)
                {
                    throw new BusinessException(
                        "متن نظر نمی‌تواند بیشتر از ۲۰۰۰ کاراکتر باشد.");
                }


                // ========================================
                // Authentication
                // ========================================

                var userId =
                    _currentUserService.GetUserId();

                if (userId == Guid.Empty)
                {
                    throw new BusinessException(
                        "کاربر احراز هویت نشده است.");
                }


                // ========================================
                // Recent Review
                // ========================================

                var hasRecentReview =
                    await HasRecentReviewAsync(
                        input.ProductId);

                if (hasRecentReview)
                {
                    throw new BusinessException(
                        "شما در ۲۴ ساعت گذشته برای این محصول نظر ثبت کرده‌اید.");
                }


                // ========================================
                // Create Review
                // ========================================

                var productReview =
                    new ProductReviewDto
                    {
                        Title = string.IsNullOrWhiteSpace(input.Title)
                            ? null
                            : input.Title.Trim(),

                        Comment = input.Comment,

                        ProductId = input.ProductId,

                        Rate = input.Rate,

                        // اطلاعات حساس از Frontend گرفته نمی‌شود
                        UserId = userId,

                        // ابتدا نیاز به تأیید کارشناس دارد
                        IsApproved = false
                    };


                var productReviewResult =
                    await CreateAsync(productReview);


                await _transactionService.CommitAsync();


                return productReviewResult;
            }
            catch (BusinessException)
            {
                await _transactionService.RollbackAsync();

                throw;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                var result =
                    new ResultDto<ProductReviewDto>
                    {
                        Success = false,
                        Message = errorMessage
                    };

                result.Errors.Add(ex.Message);

                return result;
            }
        }
        public async Task<bool> HasRecentReviewAsync(Guid productId)
        {
            var userId = _currentUserService.GetUserId();

            if (userId == Guid.Empty)
            {
                return false;
            }

            var fromDate = DateTime.Now.AddHours(-24);

            return await Query()
                .AnyAsync(x =>
                    x.ProductId == productId &&
                    x.UserId == userId &&
                    x.CreatedAt >= fromDate);
        }
        public async Task<ResultDto<ProductReviewListResultDto>> GetUserReviewsAsync(
            Guid productId,
            int page,
            int pageSize)
        {
            try
            {
                var query =
                    (await GetAllViews())
                    .Where(x =>
                        x.IsApproved &&
                        x.ProductId == productId);

                // ===============================
                // Sort
                // ===============================

                query =
                    query.OrderByDescending(x => x.CreatedAt);


                // ===============================
                // Total Count
                // ===============================

                var total =
                    await query.CountAsync();


                // ===============================
                // Pagination
                // ===============================

                var reviews =
                    await query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(x => new CreateProductReviewDto
                        {
                            Title = x.Title,

                            Comment = x.Comment,

                            Rate = x.Rate,

                            UserName = x.UserName,

                            PersianDate = x.CreatedAtPersian
                        })
                        .ToListAsync();


                // ===============================
                // Result
                // ===============================

                return new ResultDto<ProductReviewListResultDto>
                {
                    Success = true,

                    Data = new ProductReviewListResultDto
                    {
                        Items = reviews,

                        TotalCount = total,

                        Page = page,

                        PageSize = pageSize
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<ProductReviewListResultDto>
                {
                    Success = false,

                    Message = "خطا در دریافت نظرات کاربران",

                    Errors =
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdProductReviews= new List<ProductReviewDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ProductReviews = dt.ToModelList<ProductReviewCrud>();

                for (int i = 0; i < ProductReviews.Count; i++)
                {
                    var ProductReview = ProductReviews[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ProductReview);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdProductReviews.Add(createResult.Data);
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
                        InsertedCount = createdProductReviews.Count,
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
        public async Task<ResultDto<ProductRatingSummaryDto>> GetRatingSummaryAsync(
    Guid productId)
        {
            try
            {
                // ===============================
                // Query
                // ===============================

                var query =
                    (await GetAllViews())
                    .Where(x =>
                        x.ProductId == productId &&
                        x.IsApproved);


                // ===============================
                // Total Reviews
                // ===============================

                var totalReviews =
                    await query.CountAsync();


                // ===============================
                // No Reviews
                // ===============================

                if (totalReviews == 0)
                {
                    return new ResultDto<ProductRatingSummaryDto>
                    {
                        Success = true,

                        Data = new ProductRatingSummaryDto
                        {
                            AverageRate = 0,

                            TotalReviews = 0,

                            SatisfactionPercentage = 0,

                            FiveStarCount = 0,

                            FourStarCount = 0,

                            ThreeStarCount = 0,

                            TwoStarCount = 0,

                            OneStarCount = 0
                        }
                    };
                }


                // ===============================
                // Average
                // ===============================

                var averageRate =
                    await query.AverageAsync(
                        x => (decimal)x.Rate);


                // ===============================
                // Rating Distribution
                // ===============================

                var fiveStarCount =
                    await query.CountAsync(
                        x => x.Rate == 5);

                var fourStarCount =
                    await query.CountAsync(
                        x => x.Rate == 4);

                var threeStarCount =
                    await query.CountAsync(
                        x => x.Rate == 3);

                var twoStarCount =
                    await query.CountAsync(
                        x => x.Rate == 2);

                var oneStarCount =
                    await query.CountAsync(
                        x => x.Rate == 1);


                // ===============================
                // Satisfaction
                // ===============================

                var satisfiedReviews =
                    fiveStarCount +
                    fourStarCount;


                var satisfactionPercentage =
                    (int)Math.Round(
                        satisfiedReviews * 100m /
                        totalReviews);


                // ===============================
                // Result
                // ===============================

                return new ResultDto<ProductRatingSummaryDto>
                {
                    Success = true,

                    Data = new ProductRatingSummaryDto
                    {
                        AverageRate =
                            Math.Round(
                                averageRate,
                                1),

                        TotalReviews =
                            totalReviews,

                        SatisfactionPercentage =
                            satisfactionPercentage,

                        FiveStarCount =
                            fiveStarCount,

                        FourStarCount =
                            fourStarCount,

                        ThreeStarCount =
                            threeStarCount,

                        TwoStarCount =
                            twoStarCount,

                        OneStarCount =
                            oneStarCount
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<ProductRatingSummaryDto>
                {
                    Success = false,

                    Message =
                        "خطا در دریافت امتیاز محصول",

                    Errors =
                {
                    ex.Message
                }
                };
            }
        }
        public async Task<byte[]> ExportAsync(
bool exportCurrentProductReview,
int ProductReviewNumber,
int ProductReviewSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ProductReviewCrud> data;

            if (exportCurrentProductReview)
            {
                data = query
                    .Skip((ProductReviewNumber - 1) * ProductReviewSize)
                    .Take(ProductReviewSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ProductReviewCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ProductReview, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

    }

}
