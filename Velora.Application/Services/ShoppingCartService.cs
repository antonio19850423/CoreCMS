using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ShoppingCartService : GenericService<SqlShoppingCart, SqlShoppingCart, ShoppingCartDto>, IShoppingCartService
    {
        private readonly ISqlRepository<SqlShoppingCart> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IShoppingCartService _roleShoppingCartService;
        protected readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        private readonly ICookieService _cookieService;
        private readonly IDiscountService _discountService;
        private readonly IProductInventoryTransactionService _productInventoryTransactionService;
        private readonly IProductTypeService _productTypeService;
        private readonly IShoppingCartItemService _shoppingCartItemService;
        
        public ShoppingCartService(
              ISqlRepository<SqlShoppingCart> sqlRepository,
              IPosgreSqlRepository<SqlShoppingCart> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              IProductService productService,
              ICookieService cookieService,
              IDiscountService discountService,
              IProductInventoryTransactionService productInventoryTransactionService,
              IProductTypeService productTypeService,
              IShoppingCartItemService shoppingCartItemService,
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
            _productService = productService;
            _productTypeService = productTypeService;
            _discountService = discountService;
            _productInventoryTransactionService = productInventoryTransactionService;
            _cookieService= cookieService;
            _shoppingCartItemService = shoppingCartItemService;
        }
        public async Task<IQueryable<ShoppingCartCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<VwShoppingCartForm, VwShoppingCartForm, ShoppingCartCrud>();
        }
        public async Task<ResultDto<ShoppingCartDto>> CreateAsync(ShoppingCartCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                var ShoppingCart = new ShoppingCartDto
                {
                    AddressId = input.AddressId,
                    CartToken = input.CartToken,
                    CouponCode = input.CouponCode,
                    CouponDiscountAmount = input.CouponDiscountAmount,
                    CouponId = input.CouponId,
                    Description = input.Description,
                    ExpireAt = input.ExpireAt,
                    FinalAmount = input.FinalAmount,
                    OrderCode = input.OrderCode,
                    OrderedAt = input.OrderedAt,
                    PaidAt = input.PaidAt,
                    PaymentMethod = input.PaymentMethod,
                    ReceiverFirstName = input.ReceiverFirstName,
                    ReceiverLastName = input.ReceiverLastName,
                     ReceiverNationalCode = input.ReceiverNationalCode,
                     ReceiverPhone = input.ReceiverPhone,
                     ShippingMethodId = input.ShippingMethodId,
                     ShippingPrice = input.ShippingPrice,
                     Status = input.Status,
                     UserId = input.UserId,


                };

                var result = await CreateAsync(ShoppingCart);
                if (!result.Success)
                    return result;

                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ShoppingCartDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ShoppingCartDto>> UpdateAsync(ShoppingCartCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var userUpdateDto = new ShoppingCartDto
                {
                    Id = input.Id,
                    AddressId = input.AddressId,
                    CartToken = input.CartToken,
                    CouponCode = input.CouponCode,
                    CouponDiscountAmount = input.CouponDiscountAmount,
                    CouponId = input.CouponId,
                    Description = input.Description,
                    ExpireAt = input.ExpireAt,
                    FinalAmount = input.FinalAmount,
                    OrderCode = input.OrderCode,
                    OrderedAt = input.OrderedAt,
                    PaidAt = input.PaidAt,
                    PaymentMethod = input.PaymentMethod,
                    ReceiverFirstName = input.ReceiverFirstName,
                    ReceiverLastName = input.ReceiverLastName,
                    ReceiverNationalCode = input.ReceiverNationalCode,
                    ReceiverPhone = input.ReceiverPhone,
                    ShippingMethodId = input.ShippingMethodId,
                    ShippingPrice = input.ShippingPrice,
                    Status = input.Status,
                    UserId = input.UserId,

                };

                var result = await UpdateAsync(userUpdateDto, input.Id);
                if (!result.Success)
                    return result;
                await _transactionService.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ShoppingCartDto>
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
            var createdShoppingCarts = new List<ShoppingCartDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ShoppingCarts = dt.ToModelList<ShoppingCartCrud>();

                for (int i = 0; i < ShoppingCarts.Count; i++)
                {
                    var ShoppingCart = ShoppingCarts[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ShoppingCart);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdShoppingCarts.Add(createResult.Data);
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
                        InsertedCount = createdShoppingCarts.Count,
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
bool exportCurrentShoppingCart,
int ShoppingCartNumber,
int ShoppingCartSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ShoppingCartCrud> data;

            if (exportCurrentShoppingCart)
            {
                data = query
                    .Skip((ShoppingCartNumber - 1) * ShoppingCartSize)
                    .Take(ShoppingCartSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ShoppingCartCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ShoppingCart, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<ResultDto<ShoppingCartViewDto>> GetCartAsync(
            Guid? userId,
            string? cartToken)
        {
            try
            {
                var cartQuery =
                    Query()
                        .Include(x => x.ShoppingCartItems)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.Brand)
                        .Include(x => x.ShoppingCartItems)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.Category)
                        .Include(x => x.ShoppingCartItems)
                            .ThenInclude(x => x.Variant);

                var cart = await cartQuery.FirstOrDefaultAsync(x =>
                                (
                                    (userId.HasValue && x.UserId == userId)
                                    ||
                                    (!string.IsNullOrEmpty(cartToken) && x.CartToken == cartToken)
                                )
                                &&
                                x.Status == (int)ShoppingCartStatus.Cart);

                if (cart == null)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = true,
                        Data = new ShoppingCartViewDto
                        {
                            Items = new(),
                            IsAllDownloadable = false
                        }
                    };
                }


                // ==========================================
                // دریافت تخفیف‌های فعال
                // ==========================================

                var activeDiscounts =
                    await _discountService.GetActiveDiscountsAsync();


                // ==========================================
                // دریافت ID نوع محصول دانلودی
                // ==========================================

                var downloadableProductTypeId =
                    await _productTypeService.GetIdByCodeAsync(Velora.Application.Shared.Constants.ProductTypes.Download);


                var items =
                    new List<ShoppingCartItemViewDto>();


                // ==========================================
                // ساخت آیتم‌های سبد
                // ==========================================

                foreach (var item in cart.ShoppingCartItems)
                {
                    // قیمت واقعی آیتم
                    var unitPrice =
                        item.Variant != null
                            ? item.Variant.Price
                            : item.Product.Price ?? 0;


                    // محاسبه تخفیف
                    var discount =
                        _discountService.CalculateDiscount(
                            new DiscountCalculationInput
                            {
                                ProductId =
                                    item.ProductId,

                                ProductVariantId =
                                    item.VariantId,

                                ProductBrandId =
                                    item.Product.BrandId,

                                ProductCategoryId =
                                    item.Product.CategoryId,

                                Price =
                                    unitPrice
                            },
                            activeDiscounts);


                    items.Add(
                        new ShoppingCartItemViewDto
                        {
                            Id =
                                item.Id,

                            ShoppingCartId =
                                item.ShoppingCartId,

                            ProductId =
                                item.ProductId,

                            ProductTypeId =
                                item.Product.ProductTypeId,

                            VariantId =
                                item.VariantId,

                            ProductName =
                                item.Product.Name,

                            VariantName =
                                item.Variant?.Name,

                            ImageUrl =
                                item.Variant?.Image
                                ??
                                item.Product.MainImage
                                ??
                                item.Product.Thumbnail,

                            UnitPrice =
                                unitPrice,

                            Quantity =
                                item.Quantity,

                            DiscountId =
                                discount.DiscountId,

                            Discount =
                                discount.DiscountAmount,

                            DiscountType =
                                discount.DiscountType,

                            DiscountValue =
                                discount.DiscountValue,

                            DiscountAmount =
                                discount.DiscountAmount,

                            FinalPrice =
                                discount.FinalPrice
                        });
                }


                // ==========================================
                // آیا تمام محصولات دانلودی هستند؟
                // ==========================================

                var isAllDownloadable =
                    items.Count > 0 &&
                    downloadableProductTypeId.HasValue &&
                    items.All(x =>
                        x.ProductTypeId ==
                        downloadableProductTypeId.Value);


                // ==========================================
                // ساخت DTO نهایی
                // ==========================================

                var dto =
                    new ShoppingCartViewDto
                    {
                        Id =
                            cart.Id,

                        CartToken =
                            cart.CartToken,

                        Items =
                            items,

                        IsAllDownloadable =
                            isAllDownloadable,
                        CouponId = cart.CouponId,

                        CouponCode = cart.CouponCode,

                        CouponDiscountAmount = cart.CouponDiscountAmount ?? 0
                    };


                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = true,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = false,

                    Message =
                        ex.Message,

                    Errors =
                        new List<string>
                        {
                    ex.Message
                        }
                };
            }
        }

        public async Task<ResultDto<ShoppingCartViewDto>> AddAsync(
            Guid? userId,
            string? cartToken,
            ShoppingCartRequestDto input)
        {
            try
            {
                // ==========================================
                // 1. دریافت یا ایجاد سبد
                // ==========================================

                var cart =
                    await GetOrCreateCartAsync(
                        userId,
                        cartToken);


                // ==========================================
                // 2. دریافت محصول
                // ==========================================

                var product =
                    await _productService
                    .Query()
                    .Include(x => x.ProductVariants)
                    .FirstOrDefaultAsync(x =>
                        x.Id == input.ProductId);

                if (product == null)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message = "محصول یافت نشد"
                    };
                }


                // ==========================================
                // 3. قیمت فعلی محصول
                // ==========================================

                decimal unitPrice =
                    product.Price ?? 0;


                // ==========================================
                // 4. اگر Variant انتخاب شده
                // ==========================================

                ProductVariant? variant = null;

                if (input.VariantId.HasValue)
                {
                    variant =
                        product.ProductVariants
                        .FirstOrDefault(x =>
                            x.Id == input.VariantId.Value);

                    if (variant == null)
                    {
                        return new ResultDto<ShoppingCartViewDto>
                        {
                            Success = false,
                            Message = "واریانت انتخاب شده یافت نشد"
                        };
                    }

                    unitPrice = variant.Price;
                }


                // ==========================================
                // 5. دریافت موجودی واقعی از Inventory
                // ==========================================

                var productIds =
                    new List<Guid>
                    {
                product.Id
                    };


                var inventories =
                    await _productInventoryTransactionService
                        .GetInventoryAsync(productIds);


                inventories.TryGetValue(
                    product.Id,
                    out var stock);


                // ==========================================
                // 6. بررسی موجودی
                // ==========================================

                if (stock <= 0)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message = "این محصول در حال حاضر موجود نیست."
                    };
                }


                // ==========================================
                // 7. پیدا کردن آیتم موجود در سبد
                // ==========================================

                var item =
                    await _shoppingCartItemService
                    .Query()
                    .FirstOrDefaultAsync(x =>
                        x.ShoppingCartId == cart.Id
                        &&
                        x.ProductId == input.ProductId
                        &&
                        x.VariantId == input.VariantId);


                // ==========================================
                // 8. محاسبه تعداد نهایی
                // ==========================================

                var finalQuantity =
                    item != null
                        ? item.Quantity + input.Quantity
                        : input.Quantity;


                // ==========================================
                // 9. بررسی موجودی بر اساس تعداد نهایی
                // ==========================================

                if (finalQuantity > stock)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message =
                            $"تعداد انتخاب شده بیشتر از موجودی انبار است. موجودی فعلی: {stock} عدد"
                    };
                }


                // ==========================================
                // 10. دریافت تخفیف فعال
                // ==========================================

                var activeDiscounts =
                    await _discountService
                        .GetActiveDiscountsAsync();


                // ==========================================
                // 11. محاسبه تخفیف
                // ==========================================

                var discount =
                    _discountService.CalculateDiscount(
                        new DiscountCalculationInput
                        {
                            ProductId =
                                product.Id,

                            ProductVariantId =
                                input.VariantId,

                            ProductBrandId =
                                product.BrandId,

                            ProductCategoryId =
                                product.CategoryId,

                            Price =
                                unitPrice
                        },
                        activeDiscounts);


                // ==========================================
                // 12. اگر آیتم قبلاً وجود دارد
                // ==========================================

                if (item != null)
                {
                    item.Quantity =
                        finalQuantity;

                    item.UnitPrice =
                        unitPrice;

                    item.ProductTypeId =
                        product.ProductTypeId;

                    item.DiscountId =
                        discount.DiscountId;

                    item.DiscountItemId =
                        discount.DiscountItemId;

                    item.DiscountType =
                        discount.DiscountType;

                    item.DiscountValue =
                        discount.DiscountValue;

                    item.DiscountAmount =
                        discount.DiscountAmount;

                    item.FinalUnitPrice = discount.FinalPrice ?? unitPrice;

                    item.UpdatedAt =
                        DateTime.Now;


                    await _shoppingCartItemService
                        .UpdateAsync(
                            item,
                            item.Id);
                }

                // ==========================================
                // 13. ایجاد آیتم جدید
                // ==========================================

                else
                {
                    var newItem =
                        new ShoppingCartItem
                        {
                            Id =
                                Guid.NewGuid(),

                            ShoppingCartId =
                                cart.Id,

                            ProductId =
                                input.ProductId,

                            VariantId =
                                input.VariantId,

                            Quantity =
                                input.Quantity,

                            UnitPrice =
                                unitPrice,

                            ProductTypeId =
                                product.ProductTypeId,

                            DiscountId =
                                discount.DiscountId,

                            DiscountItemId =
                                discount.DiscountItemId,

                            DiscountType =
                                discount.DiscountType,

                            DiscountValue =
                                discount.DiscountValue,

                            DiscountAmount =
                                discount.DiscountAmount,

                            FinalUnitPrice = discount.FinalPrice ?? unitPrice
                        };


                    await _shoppingCartItemService
                        .CreateAsync(
                            _mapper.Map<ShoppingCartItemDto>(
                                newItem));
                }


                // ==========================================
                // 14. Commit
                // ==========================================

                await _transactionService.CommitAsync();


                // ==========================================
                // 15. دریافت مجدد سبد
                // ==========================================

                return await GetCartAsync(
                    userId,
                    cartToken);
            }
            catch (Exception ex)
            {
                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ResultDto<ShoppingCartViewDto>> UpdateQuantityAsync(
           Guid? userId,
           string? cartToken,
           Guid itemId,
           int quantity)
        {
            try
            {
                // ============================================
                // اعتبارسنجی تعداد
                // ============================================

                if (quantity <= 0)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message = "تعداد محصول باید بیشتر از صفر باشد."
                    };
                }


                // ============================================
                // دریافت آیتم سبد
                // ============================================

                var item =
                    await _shoppingCartItemService
                    .Query()
                    .FirstOrDefaultAsync(x =>
                        x.Id == itemId);

                if (item == null)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message = "آیتم پیدا نشد"
                    };
                }


                // ============================================
                // دریافت محصول و واریانت‌ها
                // ============================================

                var product =
                    await _productService
                    .Query()
                    .Include(x => x.ProductVariants)
                    .FirstOrDefaultAsync(x =>
                        x.Id == item.ProductId);

                if (product == null)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message = "محصول یافت نشد"
                    };
                }


                // ============================================
                // تعیین قیمت
                // ============================================

                decimal unitPrice =
                    product.Price ?? 0;


                if (item.VariantId.HasValue)
                {
                    var variant =
                        product.ProductVariants
                        .FirstOrDefault(x =>
                            x.Id == item.VariantId.Value);

                    if (variant == null)
                    {
                        return new ResultDto<ShoppingCartViewDto>
                        {
                            Success = false,
                            Message = "واریانت انتخاب شده یافت نشد"
                        };
                    }

                    unitPrice = variant.Price;
                }


                // ============================================
                // بررسی موجودی
                // ============================================

                var productIds =
                    new List<Guid>
                    {
                product.Id
                    };


                var inventories =
                    await _productInventoryTransactionService
                        .GetInventoryAsync(productIds);


                inventories.TryGetValue(
                    product.Id,
                    out var inventory);


                // ============================================
                // اگر موجودی وجود ندارد
                // ============================================

                if (inventory == null || inventory <= 0)
                {
                    // حذف آیتم از سبد
                    await _shoppingCartItemService
                        .DeleteAsync(item.Id);

                    await _transactionService.CommitAsync();

                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message = "این محصول در حال حاضر موجودی ندارد."
                    };
                }


                // ============================================
                // موجودی برای تعداد درخواستی کافی نیست
                // ============================================

                if (inventory < quantity)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = false,
                        Message =
                            $"موجودی محصول کافی نیست. حداکثر تعداد قابل سفارش: {inventory}"
                    };
                }


                // ============================================
                // دریافت تخفیف‌های فعال
                // ============================================

                var activeDiscounts =
                    await _discountService
                        .GetActiveDiscountsAsync();


                // ============================================
                // محاسبه مجدد تخفیف
                // ============================================

                var discount =
                    _discountService.CalculateDiscount(
                        new DiscountCalculationInput
                        {
                            ProductId =
                                product.Id,

                            ProductVariantId =
                                item.VariantId,

                            ProductBrandId =
                                product.BrandId,

                            ProductCategoryId =
                                product.CategoryId,

                            Price =
                                unitPrice
                        },
                        activeDiscounts);


                // ============================================
                // بروزرسانی ShoppingCartItem
                // ============================================

                await _shoppingCartItemService
                    .Query()
                    .Where(x =>
                        x.Id == itemId)
                    .ExecuteUpdateAsync(x =>
                        x.SetProperty(
                            p => p.Quantity,
                            quantity)

                        // نوع محصول
                        .SetProperty(
                            p => p.ProductTypeId,
                            product.ProductTypeId)

                        // قیمت فعلی
                        .SetProperty(
                            p => p.UnitPrice,
                            unitPrice)

                        // اطلاعات تخفیف
                        .SetProperty(
                            p => p.DiscountId,
                            discount.DiscountId)

                        .SetProperty(
                            p => p.DiscountItemId,
                            discount.DiscountItemId)

                        .SetProperty(
                            p => p.DiscountType,
                            discount.DiscountType)

                        .SetProperty(
                            p => p.DiscountValue,
                            discount.DiscountValue)

                        .SetProperty(
                            p => p.DiscountAmount,
                            discount.DiscountAmount)

                        // قیمت نهایی هر واحد
                        .SetProperty(
                            p => p.FinalUnitPrice,
                            discount.FinalPrice)

                        // تاریخ بروزرسانی
                        .SetProperty(
                            p => p.UpdatedAt,
                            DateTime.Now)
                    );


                // ============================================
                // Commit
                // ============================================

                await _transactionService.CommitAsync();


                // ============================================
                // دریافت مجدد سبد
                // ============================================

                return await GetCartAsync(
                    userId,
                    cartToken);
            }
            catch (Exception ex)
            {
                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ResultDto<ShoppingCartViewDto>> RemoveAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId)
        {

            var item =
                await _shoppingCartItemService
                .Query()
                .FirstOrDefaultAsync(x => x.Id == itemId);



            if (item != null)
            {

                await _shoppingCartItemService
                    .DeleteAsync(item.Id);

            }

            await _transactionService.CommitAsync();

            return await GetCartAsync(
                userId,
                cartToken);

        }

        public async Task<ResultDto<bool>> ClearAsync(
            Guid? userId,
            string? cartToken)
        {

            var cart =
                await GetCartEntityAsync(
                    userId,
                    cartToken);



            if (cart != null)
            {

                var ids =
                    cart.ShoppingCartItems
                    .Select(x => x.Id)
                    .ToList();


                foreach (var id in ids)
                {
                    await _shoppingCartItemService
                        .DeleteAsync(id);
                }

            }

            await _transactionService.CommitAsync();

            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };

        }

        public async Task<ResultDto<ShoppingCartViewDto>> MergeAsync(
            Guid? userId,
            string? cartToken)
        {
            // ============================================
            // 1. ابتدا Cart کاربر را بر اساس UserId پیدا کن
            // ============================================

            ShoppingCart? userCart = null;

            if (userId.HasValue)
            {
                userCart =
                    await Query()
                    .Include(x => x.ShoppingCartItems)
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId);
            }


            // ============================================
            // اگر Cart کاربر وجود دارد
            // ============================================

            if (userCart != null)
            {
                // همان CartToken موجود در دیتابیس
                // دوباره در Cookie قرار بگیرد

                if (!string.IsNullOrWhiteSpace(userCart.CartToken))
                {
                    _cookieService.Set(
                        CookieKeys.CartToken,
                        userCart.CartToken,
                        30);
                }


                return await GetCartAsync(
                    userId,
                    userCart.CartToken);
            }


            // ============================================
            // 2. اگر Cart کاربر وجود نداشت
            //    بر اساس CartToken سبد مهمان را پیدا کن
            // ============================================

            ShoppingCart? guestCart = null;

            if (!string.IsNullOrWhiteSpace(cartToken))
            {
                guestCart =
                    await Query()
                    .Include(x => x.ShoppingCartItems)
                    .FirstOrDefaultAsync(x =>
                        x.CartToken == cartToken);
            }


            // ============================================
            // سبد مهمان هم وجود ندارد
            // ============================================

            if (guestCart == null)
            {
                return await GetCartAsync(
                    userId,
                    null);
            }


            // ============================================
            // 3. سبد مهمان را به کاربر متصل کن
            // ============================================

            guestCart.UserId = userId;
            guestCart.UpdateAt = DateTime.Now;


            await UpdateAsync(
                    _mapper.Map<ShoppingCartDto>(guestCart),
                    guestCart.Id);


            await _transactionService.CommitAsync();


            // ============================================
            // 4. همان CartToken را دوباره در Cookie قرار بده
            // ============================================

            if (!string.IsNullOrWhiteSpace(guestCart.CartToken))
            {
                _cookieService.Set(
                    CookieKeys.CartToken,
                    guestCart.CartToken,
                    30);
            }


            return await GetCartAsync(
                userId,
                guestCart.CartToken);
        }

        public async Task<ResultDto<int>> GetCountAsync(
            Guid? userId,
            string? cartToken)
        {

            var cart =
                await GetCartEntityAsync(
                    userId,
                    cartToken);



            return new ResultDto<int>
            {
                Success = true,

                Data =
                    cart?
                    .ShoppingCartItems
                    .Sum(x => x.Quantity)
                    ?? 0
            };

        }

        private async Task<ShoppingCart> GetOrCreateCartAsync(
       Guid? userId,
       string? cartToken)
        {
            // اگر userId خالی باشد تبدیل به null شود
            if (userId == Guid.Empty)
                userId = null;


            // اگر Token نداریم ایجاد کنیم
            if (string.IsNullOrWhiteSpace(cartToken))
            {
                cartToken = Guid.NewGuid().ToString();
            }



            var cart =
                await GetCartEntityAsync(
                    userId,
                    cartToken);



            if (cart != null)
                return cart;



            var entity = new ShoppingCart
            {
                Id = Guid.NewGuid(),

                UserId = userId,

                CartToken = cartToken,

                Status = 1,

                ExpireAt = DateTime.Now.AddDays(30)
            };


            await CreateAsync(
                    _mapper.Map<ShoppingCartDto>(entity));



            return entity;
        }

        private async Task<ShoppingCart?> GetCartEntityAsync(
            Guid? userId,
            string? cartToken)
        {

            return await Query()
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x =>
                    (userId.HasValue &&
                     x.UserId == userId)

                    ||

                    (!string.IsNullOrEmpty(cartToken)
                     &&
                     x.CartToken == cartToken));

        }

        public async Task<SqlShoppingCart?> GetByIdAsync(Guid shoppingCartId)
        {
            if (shoppingCartId == Guid.Empty)
                return null;

            return await Query()
                .FirstOrDefaultAsync(x => x.Id == shoppingCartId);
        }
        public async Task<bool> CartHasDiscountAsync(Guid shoppingCartId)
        {
            return await _shoppingCartItemService
                .Query()
                .AnyAsync(x =>
                    x.ShoppingCartId == shoppingCartId &&
                    x.DiscountAmount > 0);
        }
        public async Task<decimal> GetCartAmountForCouponAsync(
    Guid shoppingCartId)
        {
            return await _shoppingCartItemService
                .Query()
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .SumAsync(x => x.FinalUnitPrice * x.Quantity);
        }
        public async Task ApplyCouponToCart(
    ShoppingCart cart,
    Coupon coupon,
    decimal discountAmount)
        {
            cart.CouponId = coupon.Id;
            cart.CouponCode = coupon.Code;
            cart.CouponDiscountAmount = discountAmount;
            await UpdateAsync(
             _mapper.Map<ShoppingCartDto>(cart),
             cart.Id);

        }

    }
}
