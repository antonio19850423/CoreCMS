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
    public class CouponUsageService : GenericService<SqlCouponUsage, SqlCouponUsage, CouponUsageDto>, ICouponUsageService
    {
        private readonly ISqlRepository<SqlCouponUsage> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly ICouponService _couponService;
        protected readonly IShoppingCartService _shoppingCartService;
        public CouponUsageService(
              ISqlRepository<SqlCouponUsage> sqlRepository,
              IPosgreSqlRepository<SqlCouponUsage> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IShoppingCartService shoppingCartService, ICouponService couponService)
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
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
        }
        public async Task<IQueryable<CouponUsageCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlCouponUsageView, SqlCouponUsageView, CouponUsageCrud>();
        }

        public async Task<ResultDto<CouponUsageDto>> CreateAsync(CouponUsageCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var CouponUsage = new CouponUsageDto
                {
                    CouponId=input.ParentId,
                    OrderId=input.OrderId,
                    UsedAt=DateTime.Now,
                    UserId=input.UserId,
                };

                var CouponUsageResult = await CreateAsync(CouponUsage);
                await _transactionService.CommitAsync();
                return CouponUsageResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CouponUsageDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<CouponUsageDto>> UpdateAsync(CouponUsageCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new CouponUsageDto
                {
                    Id = input.Id,
                    CouponId = input.ParentId,
                    OrderId = input.OrderId,
                    UsedAt = DateTime.Now,
                    UserId = input.UserId,
                };

                var CouponUsageResult = await UpdateAsync(updateDto, input.Id);
                await _transactionService.CommitAsync();
                return CouponUsageResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<CouponUsageDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ShoppingCartDto>> ApplyCouponAsync(
    Guid shoppingCartId,
    string couponCode)
        {
            try
            {
                if (shoppingCartId == Guid.Empty)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "شناسه سبد خرید معتبر نیست."
                    };
                }

                if (string.IsNullOrWhiteSpace(couponCode))
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "کد تخفیف را وارد کنید."
                    };
                }

                couponCode = couponCode.Trim().ToUpperInvariant();

                // -------------------------------------------------------
                // 1. دریافت سبد خرید
                // -------------------------------------------------------

                var cart = await _shoppingCartService
                    .GetByIdAsync(shoppingCartId);

                if (cart == null)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "سبد خرید پیدا نشد."
                    };
                }


                // -------------------------------------------------------
                // 2. بررسی مالک سبد خرید
                // -------------------------------------------------------

                var currentUserId = _currentUserService.GetUserId();

                if (!currentUserId.HasValue)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "کاربر وارد سیستم نشده است."
                    };
                }

                if (cart.UserId != currentUserId.Value)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "این سبد خرید متعلق به شما نیست."
                    };
                }

                // -------------------------------------------------------
                // 3. دریافت کوپن
                // -------------------------------------------------------

                var coupon = await _couponService.GetByCodeAsync(couponCode);

                if (coupon == null)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "کد تخفیف معتبر نیست."
                    };
                }

                // -------------------------------------------------------
                // 4. فعال بودن کوپن
                // -------------------------------------------------------

                if (!coupon.IsActive)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "این کد تخفیف فعال نیست."
                    };
                }

                // -------------------------------------------------------
                // 5. بررسی تاریخ شروع
                // -------------------------------------------------------

                var now = DateTime.UtcNow;

                if (coupon.StartDate.HasValue &&
                    coupon.StartDate.Value > now)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "زمان استفاده از این کد تخفیف هنوز شروع نشده است."
                    };
                }

                // -------------------------------------------------------
                // 6. بررسی تاریخ پایان
                // -------------------------------------------------------

                if (coupon.EndDate.HasValue &&
                    coupon.EndDate.Value < now)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "مهلت استفاده از این کد تخفیف به پایان رسیده است."
                    };
                }
                if (coupon.CanCombineWithDiscount!=true)
                {
                    var hasDiscount = await _shoppingCartService.CartHasDiscountAsync(shoppingCartId);

                    if (hasDiscount)
                    {
                        return new ResultDto<ShoppingCartDto>
                        {
                            Success = false,
                            Message = "این کوپن با تخفیف محصولات قابل استفاده همزمان نیست."
                        };
                    }
                }
                // -------------------------------------------------------
                // 7. بررسی تعداد کل استفاده
                // -------------------------------------------------------

                var totalUsageCount = await 
                    Query()
                    .CountAsync(x => x.CouponId == coupon.Id);

                if (coupon.UsageLimit.HasValue &&
                    totalUsageCount >= coupon.UsageLimit.Value)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "ظرفیت استفاده از این کد تخفیف تکمیل شده است."
                    };
                }

                // -------------------------------------------------------
                // 8. بررسی استفاده قبلی همین کاربر
                // -------------------------------------------------------

                if (coupon.IsSingleUsePerUser)
                {
                    var userAlreadyUsed = await 
                        Query()
                        .AnyAsync(x =>
                            x.CouponId == coupon.Id &&
                            x.UserId == currentUserId.Value);

                    if (userAlreadyUsed)
                    {
                        return new ResultDto<ShoppingCartDto>
                        {
                            Success = false,
                            Message = "شما قبلاً از این کد تخفیف استفاده کرده‌اید."
                        };
                    }
                }

                // -------------------------------------------------------
                // 9. محاسبه مبلغ سبد
                // -------------------------------------------------------

                // این مقدار باید مبلغ کالاها بعد از تخفیف‌های خود محصول
                // و قبل از مالیات باشد.
                var cartAmount = await _shoppingCartService.GetCartAmountForCouponAsync(
                    shoppingCartId);

                if (cartAmount <= 0)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "مبلغ سبد خرید برای اعمال کد تخفیف معتبر نیست."
                    };
                }

                // -------------------------------------------------------
                // 10. حداقل مبلغ سفارش
                // -------------------------------------------------------

                if (coupon.MinimumOrderAmount.HasValue &&
                    cartAmount < coupon.MinimumOrderAmount.Value)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            $"حداقل مبلغ سفارش برای استفاده از این کد تخفیف " +
                            $"باید {coupon.MinimumOrderAmount.Value:N0} تومان باشد."
                    };
                }

                // -------------------------------------------------------
                // 11. محاسبه تخفیف
                // -------------------------------------------------------

                decimal discountAmount = 0;

                if (coupon.CouponType == 1)
                {
                    // درصدی
                    discountAmount =
                        cartAmount * (coupon.CouponValue / 100m);
                }
                else
                {
                    // مبلغ ثابت
                    discountAmount = coupon.CouponValue;
                }

                // -------------------------------------------------------
                // 12. جلوگیری از بیشتر شدن تخفیف از مبلغ سفارش
                // -------------------------------------------------------

                if (discountAmount > cartAmount)
                {
                    discountAmount = cartAmount;
                }

                // -------------------------------------------------------
                // 13. MaximumDiscountAmount
                // -------------------------------------------------------

                if (coupon.MaximumDiscountAmount.HasValue &&
                    discountAmount > coupon.MaximumDiscountAmount.Value)
                {
                    discountAmount = coupon.MaximumDiscountAmount.Value;
                }

                // -------------------------------------------------------
                // 14. جلوگیری از مقدار منفی
                // -------------------------------------------------------

                if (discountAmount < 0)
                {
                    discountAmount = 0;
                }

                discountAmount = Math.Round(
                    discountAmount,
                    2,
                    MidpointRounding.AwayFromZero);

                // -------------------------------------------------------
                // 15. اگر همین کوپن قبلاً روی سبد وجود دارد
                // -------------------------------------------------------

                if (cart.CouponId.HasValue &&
                    cart.CouponId.Value == coupon.Id)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "این کد تخفیف قبلاً روی سبد خرید اعمال شده است."
                    };
                }

                // -------------------------------------------------------
                // 16. اگر کوپن دیگری روی سبد وجود دارد
                // -------------------------------------------------------

                if (cart.CouponId.HasValue &&
                    cart.CouponId.Value != coupon.Id)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "در حال حاضر یک کد تخفیف روی این سبد خرید اعمال شده است."
                    };
                }

                // -------------------------------------------------------
                // 17. اعمال کوپن روی ShoppingCart
                // -------------------------------------------------------
              await  _shoppingCartService.ApplyCouponToCart(cart, coupon, discountAmount);
              await  CreateIfNotExistsAsync(coupon.Id, cart.Id, cart.UserId);
                await   _transactionService.CommitAsync();


                return new ResultDto<ShoppingCartDto>
                {
                    Success = true,
                    Message = "کد تخفیف با موفقیت اعمال شد."
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<ShoppingCartDto>
                {
                    Success = false,
                    Message = "خطایی هنگام اعمال کد تخفیف رخ داد.",
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ResultDto<ShoppingCartDto>> RemoveCouponAsync(
    Guid shoppingCartId)
        {
            try
            {
                if (shoppingCartId == Guid.Empty)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "شناسه سبد خرید معتبر نیست."
                    };
                }

                // دریافت سبد
                var cart = await _shoppingCartService
                    .GetByIdAsync(shoppingCartId);

                if (cart == null)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "سبد خرید پیدا نشد."
                    };
                }

                // اگر تبدیل به سفارش شده باشد
                if (cart.Status == 2)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "این سبد خرید به سفارش تبدیل شده و امکان حذف کد تخفیف وجود ندارد."
                    };
                }

                // اگر اصلاً کوپنی روی سبد نیست
                if (!cart.CouponId.HasValue)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "کدی برای حذف روی سبد خرید وجود ندارد."
                    };
                }

                var couponId = cart.CouponId.Value;

                // حذف اطلاعات کوپن از سبد
                cart.CouponId = null;
                cart.CouponCode = null;
                cart.CouponDiscountAmount = 0;

                await _shoppingCartService.UpdateAsync(
                    _mapper.Map<ShoppingCartDto>(cart),
                    cart.Id);
               await RemoveIfExistsAsync(couponId, cart.Id, cart.UserId);
                // اگر CouponUsage برای این سبد/سفارش ثبت شده باشد
                // باید بر اساس ساختار واقعی ارتباط آن حذف شود.
                //
                // چون CouponUsage فعلی شما OrderId دارد و هنوز
                // هنگام حذف از Cart الزاماً OrderId نداریم،
                // این قسمت باید بر اساس Order ساخته‌شده انجام شود.

                await _transactionService.CommitAsync();

                return new ResultDto<ShoppingCartDto>
                {
                    Success = true,
                    Message = "کد تخفیف با موفقیت حذف شد."
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<ShoppingCartDto>
                {
                    Success = false,
                    Message = "خطایی هنگام حذف کد تخفیف رخ داد.",
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ResultDto<bool>> RemoveIfExistsAsync(
    Guid couponId,
    Guid orderId,
    Guid? userId)
        {
            try
            {
                if (couponId == Guid.Empty)
                {
                    return new ResultDto<bool>
                    {
                        Success = false,
                        Message = "شناسه کوپن معتبر نیست."
                    };
                }

                if (orderId == Guid.Empty)
                {
                    return new ResultDto<bool>
                    {
                        Success = false,
                        Message = "شناسه سفارش معتبر نیست."
                    };
                }

                if (userId == Guid.Empty)
                {
                    return new ResultDto<bool>
                    {
                        Success = false,
                        Message = "شناسه کاربر معتبر نیست."
                    };
                }

                var couponUsage = await Query()
                    .FirstOrDefaultAsync(x =>
                        x.CouponId == couponId &&
                        x.OrderId == orderId &&
                        x.UserId == userId);

                // وجود ندارد
                if (couponUsage == null)
                {
                    return new ResultDto<bool>
                    {
                        Success = true,
                        Data = false,
                        Message = "مصرفی برای این کوپن پیدا نشد."
                    };
                }

                await DeleteAsync(couponUsage.Id);

                await _transactionService.CommitAsync();

                return new ResultDto<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "مصرف کوپن با موفقیت حذف شد."
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "خطایی هنگام حذف مصرف کوپن رخ داد.",
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ResultDto<CouponUsageDto>> CreateIfNotExistsAsync(
    Guid couponId,
    Guid orderId,
    Guid? userId)
        {
            try
            {
                if (couponId == Guid.Empty)
                {
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = false,
                        Message = "شناسه کوپن معتبر نیست."
                    };
                }

                if (orderId == Guid.Empty)
                {
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = false,
                        Message = "شناسه سفارش معتبر نیست."
                    };
                }

                if (userId == Guid.Empty)
                {
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = false,
                        Message = "شناسه کاربر معتبر نیست."
                    };
                }

                // بررسی وجود قبلی
                var existingCouponUsage = await Query()
                    .FirstOrDefaultAsync(x =>
                        x.CouponId == couponId &&
                        x.OrderId == orderId &&
                        x.UserId == userId);

                if (existingCouponUsage != null)
                {
                    return new ResultDto<CouponUsageDto>
                    {
                        Success = true,
                        Message = "مصرف این کوپن قبلاً ثبت شده است."
                    };
                }

                // استفاده از همان متد CreateAsync که خودت داری
                var input = new CouponUsageCrud
                {
                    ParentId = couponId,
                    OrderId = orderId,
                    UserId = userId.Value,
                    UsedAt = DateTime.Now,
                };

                var result = await CreateAsync(input);

                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<CouponUsageDto>
                {
                    Success = false,
                    Message = "خطایی هنگام ثبت مصرف کوپن رخ داد.",
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdCouponUsages= new List<CouponUsageDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var CouponUsages = dt.ToModelList<CouponUsageCrud>();

                for (int i = 0; i < CouponUsages.Count; i++)
                {
                    var CouponUsage = CouponUsages[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(CouponUsage);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdCouponUsages.Add(createResult.Data);
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
                        InsertedCount = createdCouponUsages.Count,
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

        public async Task<byte[]> ExportAsync(
bool exportCurrentCouponUsage,
int CouponUsageNumber,
int CouponUsageSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<CouponUsageCrud> data;

            if (exportCurrentCouponUsage)
            {
                data = query
                    .Skip((CouponUsageNumber - 1) * CouponUsageSize)
                    .Take(CouponUsageSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<CouponUsageCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.CouponUsage, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

    }

}
