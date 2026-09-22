using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
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
        private readonly IPaymentService _paymentService;
        private readonly Lazy<ICouponUsageService> _couponUsageService;
        private readonly Lazy<ICouponService> _couponService;
        private readonly IUserAddressService _addressService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly IShippingMethodCityService _shippingMethodCityService;
        private readonly IContentService _contentService;


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
              ICurrentUserService currentUserService, IPaymentService paymentService, Lazy<ICouponUsageService> couponUsageService, Lazy<ICouponService> couponService, IUserAddressService addressService, IShippingMethodService shippingMethodService, IShippingMethodCityService shippingMethodCityService, IContentService contentService)
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
            _cookieService = cookieService;
            _shoppingCartItemService = shoppingCartItemService;
            _paymentService = paymentService;
            _couponUsageService = couponUsageService;
            _couponService = couponService;
            _addressService = addressService;
            _shippingMethodService = shippingMethodService;
            _shippingMethodCityService = shippingMethodCityService;
            _contentService = contentService;
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
                    AddressText = input.AddressText,
                    ShippingMethodName = input.ShippingMethodName,
                    OrderStatus = input.OrderStatus,

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
                    ShippingMethodName = input.ShippingMethodName,
                    AddressText = input.AddressText,
                    OrderStatus = input.OrderStatus,

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

                ShoppingCart? cart = null;

                // ==========================================
                // پیدا کردن سبد خرید
                // ==========================================

                if (userId.HasValue)
                {
                    // کاربر لاگین کرده است.
                    // در این حالت فقط UserId ملاک مالکیت سبد است.
                    cart =
                        await cartQuery.FirstOrDefaultAsync(x =>
                            x.UserId == userId.Value &&
                            x.Status == (int)ShoppingCartStatus.Cart);
                }
                else if (!string.IsNullOrEmpty(cartToken))
                {
                    // کاربر Guest است.
                    // در این حالت CartToken فقط برای سبدهای بدون UserId استفاده می‌شود.
                    cart =
                        await cartQuery.FirstOrDefaultAsync(x =>
                            x.CartToken == cartToken &&
                            x.UserId == null &&
                            x.Status == (int)ShoppingCartStatus.Cart);
                }

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
                // اعتبارسنجی کوپن
                // ==========================================

                string? couponMessage = null;
                var couponChanged = false;

                if (cart.CouponId.HasValue)
                {
                    var coupon =
                        await _couponService.Value.GetByIdAsync(
                            cart.CouponId.Value);

                    if (coupon == null)
                    {
                        cart.CouponId = null;
                        cart.CouponCode = null;
                        cart.CouponDiscountAmount = 0;

                        couponMessage =
                            "کد تخفیف دیگر معتبر نیست.";

                        couponChanged = true;
                    }
                    else
                    {
                        var validation =
                            await _couponUsageService.Value.ValidateCouponAsync(
                                coupon.Data,
                                cart.Id,
                                userId);

                        if (!validation.IsValid)
                        {
                            cart.CouponId = null;
                            cart.CouponCode = null;
                            cart.CouponDiscountAmount = 0;

                            await _couponUsageService.Value.RemoveIfExistsAsync(
                                coupon.Data.Id,
                                cart.Id,
                                cart.UserId);

                            couponMessage =
                                validation.Message
                                ?? "کد تخفیف دیگر معتبر نیست.";

                            couponChanged = true;
                        }
                    }
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
                    await _productTypeService.GetIdByCodeAsync(
                        Velora.Application.Shared.Constants.ProductTypes.Download);

                var items =
                    new List<ShoppingCartItemViewDto>();

                // ==========================================
                // ساخت آیتم‌های سبد
                // ==========================================

                foreach (var item in cart.ShoppingCartItems)
                {
                    var unitPrice =
                        item.Variant != null
                            ? item.Variant.Price
                            : item.Product.Price ?? 0;

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

                    var hasPriceChanged =
                        item.UnitPrice != unitPrice;

                    var hasDiscountChanged =
                        item.DiscountAmount != discount.DiscountAmount
                        || item.FinalUnitPrice != discount.FinalPrice;

                    var currentStock =
                        await _productInventoryTransactionService.GetInventoryAsync(
                            item.ProductId,
                            item.VariantId);

                    var isOutOfStock =
                        currentStock <= 0;

                    var isQuantityAvailable =
                        currentStock >= item.Quantity;

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
                                discount.FinalPrice,

                            CartUnitPrice =
                                item.UnitPrice,

                            CartDiscountAmount =
                                item.DiscountAmount,

                            CartFinalUnitPrice =
                                item.FinalUnitPrice,

                            HasPriceChanged =
                                hasPriceChanged,

                            HasDiscountChanged =
                                hasDiscountChanged,

                            CurrentStock =
                                currentStock,

                            IsOutOfStock =
                                isOutOfStock,

                            IsQuantityAvailable =
                                isQuantityAvailable
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
                // ذخیره تغییرات کوپن
                // ==========================================

                if (couponChanged)
                {
                    await _transactionService.CommitAsync();
                }

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

                        CouponId =
                            cart.CouponId,

                        CouponCode =
                            cart.CouponCode,

                        CouponDiscountAmount =
                            cart.CouponDiscountAmount ?? 0,

                        CouponMessage =
                            couponMessage
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
            // 1. ShoppingCart کاربر
            // ============================================

            ShoppingCart? userCart = null;

            if (userId.HasValue)
            {
                userCart =
                    await Query()
                        .Include(x => x.ShoppingCartItems)
                        .FirstOrDefaultAsync(x =>
                            x.UserId == userId.Value &&
                            x.Status == (int)ShoppingCartStatus.Cart);
            }

            // ============================================
            // 2. ShoppingCart مهمان
            // ============================================

            ShoppingCart? guestCart = null;

            if (!string.IsNullOrWhiteSpace(cartToken))
            {
                guestCart =
                    await Query()
                        .Include(x => x.ShoppingCartItems)
                        .FirstOrDefaultAsync(x =>
                            x.CartToken == cartToken &&
                            x.UserId == null &&
                            x.Status == (int)ShoppingCartStatus.Cart);
            }

            // ============================================
            // 3. اگر User Cart وجود دارد
            // ============================================

            if (userCart != null)
            {
                if (guestCart != null)
                {
                    foreach (var guestItem in guestCart.ShoppingCartItems.ToList())
                    {
                        var userItem =
                            userCart.ShoppingCartItems
                                .FirstOrDefault(x =>
                                    x.ProductId == guestItem.ProductId &&
                                    x.VariantId == guestItem.VariantId);

                        // ============================================
                        // Item مشابه
                        // ============================================

                        if (userItem != null)
                        {
                            userItem.Quantity =
                                userItem.Quantity + guestItem.Quantity;

                            userItem.UpdatedAt = DateTime.Now;

                            await _shoppingCartItemService.UpdateAsync(
                                _mapper.Map<ShoppingCartItemDto>(userItem),
                                userItem.Id);

                            await _shoppingCartItemService.DeleteAsync(
                                guestItem.Id);
                        }
                        else
                        {
                            guestItem.ShoppingCartId = userCart.Id;

                            await _shoppingCartItemService.UpdateAsync(
                                _mapper.Map<ShoppingCartItemDto>(guestItem),
                                guestItem.Id);
                        }
                    }

                    userCart.UpdateAt = DateTime.Now;

                    // ابتدا تغییر ShoppingCartItem ها ذخیره شود
                    await _transactionService.CommitAsync();

                    // ============================================
                    // حالا Guest Cart خالی است
                    // ============================================

                    await DeleteAsync(guestCart.Id);

                    await _transactionService.CommitAsync();
                }

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
            // 4. User Cart وجود ندارد
            // Guest Cart مستقیماً تبدیل به User Cart می‌شود
            // ============================================

            if (guestCart == null)
            {
                return await GetCartAsync(
                    userId,
                    null);
            }

            guestCart.UserId = userId;
            guestCart.UpdateAt = DateTime.Now;

            await UpdateAsync(
                _mapper.Map<ShoppingCartDto>(guestCart),
                guestCart.Id);

            await _transactionService.CommitAsync();

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

                ExpireAt = DateTime.Now.AddDays(30),
                CreateAt = DateTime.Now,
            };


            await CreateAsync(
                    _mapper.Map<ShoppingCartDto>(entity));



            return entity;
        }


        public async Task<ShoppingCart?> GetCartEntityAsync(
            Guid? userId,
            string? cartToken)
        {
            return await Query()
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x =>
                    (
                        userId.HasValue &&
                        x.UserId == userId.Value &&
                        x.Status == (int)ShoppingCartStatus.Cart
                    )
                    ||
                    (
                        !string.IsNullOrEmpty(cartToken) &&
                        x.CartToken == cartToken &&
                        x.UserId == null &&
                        x.Status == (int)ShoppingCartStatus.Cart
                    )
                );
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
        public async Task<ResultDto<ShoppingCartDto>> CreateOrderAsync(
            Guid? userId,
            string? cartToken,
            CreateOrderRequestDto input,
            CancellationToken cancellationToken)
        {
            var (successMessage, errorMessage) =
                await _messageService.Value.GetSaveMessagesAsync();

            try
            {
                // ============================================
                // 1. دریافت سبد خرید
                // ============================================

                var cart =
              await Query()
                  .Include(x => x.ShoppingCartItems)
                      .ThenInclude(x => x.Product)
                  .Include(x => x.ShoppingCartItems)
                      .ThenInclude(x => x.Variant)
                  .FirstOrDefaultAsync(x =>
                      x.Status == (int)ShoppingCartStatus.Cart
                      &&
                      !string.IsNullOrWhiteSpace(x.CartToken)
                      &&
                      x.CartToken == cartToken
                      &&
                      (
                          !userId.HasValue
                          || x.UserId == userId.Value
                      ),
                  cancellationToken);

                if (cart == null)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            "سبد خرید پیدا نشد."
                    };
                }

                if (cart.Status == (int)ShoppingCartStatus.ConvertedToOrder)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            "این سفارش قبلاً ثبت شده است. از بخش پیگیری سفارش آن را دنبال کنید."
                    };
                }

                if (cart.Status != (int)ShoppingCartStatus.Cart)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            "وضعیت سبد خرید معتبر نیست."
                    };
                }



                // ============================================
                // 2. بررسی خالی نبودن سبد
                // ============================================

                if (!cart.ShoppingCartItems.Any())
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            "سبد خرید شما خالی است."
                    };
                }



                // ============================================
                // 3. اعتبارسنجی آدرس
                // ============================================

                var addressValidation =
                    await ValidateAddressAsync(
                        userId,
                        input.AddressId);


                if (!addressValidation.Success)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            addressValidation.Message
                    };
                }



                // ============================================
                // 4. اعتبارسنجی روش ارسال
                // ============================================

                var shippingValidation =
                    await ValidateShippingAsync(
                        input.ShippingMethodId,
                        input.AddressId);


                if (!shippingValidation.Success)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            shippingValidation.Message
                    };
                }



                // ============================================
                // 5. اعتبارسنجی اطلاعات گیرنده
                // ============================================

                var receiverValidation =
                    ValidateReceiver(input);


                if (!receiverValidation.Success)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            receiverValidation.Message
                    };
                }

                var couponValidation =
                    await ValidateCouponAsync(
                        cart,
                        userId);

                if (!couponValidation.Success)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            couponValidation.Message
                    };
                }

                // ============================================
                // 6. اعتبارسنجی پرداخت
                // ============================================

                var paymentValidation =
                    ValidatePayment(input);


                if (!paymentValidation.Success)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            paymentValidation.Message
                    };
                }



                // ============================================
                // 7. اعتبارسنجی کالاها
                // قیمت
                // تخفیف
                // موجودی
                // ============================================

                var cartItemsValidation =
                    await ValidateCartItemsAsync(
                        cart,
                        userId,
                        cancellationToken);


                if (!cartItemsValidation.Success)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            cartItemsValidation.Message
                    };
                }

                var siteData = await _contentService.GetSiteInfoAsync();

                var taxPercentage =
                    siteData?.Data.HasTax == true
                        ? siteData.Data.TaxPercentage ?? 0
                        : 0;

                var dutyPercentage =
                    siteData?.Data.HasDuty == true
                        ? siteData.Data.DutyPercentage ?? 0
                        : 0;

                // ============================================
                // 8. محاسبه مبلغ کالاها
                // ============================================

                var productsAmount =
                    cart.ShoppingCartItems.Sum(x =>
                        x.FinalUnitPrice * x.Quantity);



                // ============================================
                // 9. مبلغ کوپن
                // ============================================

                var couponDiscountAmount =
                    cart.CouponDiscountAmount ?? 0;


                couponDiscountAmount =
                    Math.Max(
                        0,
                        Math.Min(
                            couponDiscountAmount,
                            productsAmount));

                // ============================================
                // 10. مبلغ بعد از تخفیف
                // ============================================

                var amountAfterCoupon =
                    Math.Max(
                        0,
                        productsAmount - couponDiscountAmount);

                // ============================================
                // 11. محاسبه مالیات و عوارض
                // ============================================

                var taxAmount =
                    taxPercentage > 0
                        ? (amountAfterCoupon * taxPercentage) / 100
                        : 0;

                var dutyAmount =
                    dutyPercentage > 0
                        ? (amountAfterCoupon * dutyPercentage) / 100
                        : 0;

                // ============================================
                // 10. هزینه ارسال
                // ============================================

                var shipping =
                    await _shippingMethodService.Query()
                    .FirstOrDefaultAsync(x =>
                        x.Id == input.ShippingMethodId &&
                        x.IsActive,
                        cancellationToken);



                if (shipping == null)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            "روش ارسال معتبر نیست."
                    };
                }


                var shippingPrice =
                    shipping.Price;



                // ============================================
                // 11. مبلغ نهایی
                // ============================================

                var finalAmount =
                    amountAfterCoupon +
                    taxAmount +
                    dutyAmount +
                    shippingPrice;

                // ============================================
                // 12. بررسی مبلغ نهایی ذخیره شده
                // ============================================

                // ============================================
                // 12. بررسی مبلغ نهایی ارسالی از فرانت
                // ============================================

                if (Math.Abs(input.FinalAmount - finalAmount) > 10)
                {
                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message =
                            "مبلغ سفارش تغییر کرده است. لطفاً سبد خرید را بروزرسانی کنید."
                    };
                }

                // ============================================
                // 16. ثبت نهایی سفارش با Stored Procedure
                // ============================================

                var orderResult =
                    await ExecuteStoredProcedureAsync<ConvertToOrderResultDto>(
                        "dbo.ShoppingCart_ConvertToOrder",

                        new SqlParameter(
                            "@ShoppingCartId",
                            cart.Id),

                        new SqlParameter(
                            "@ReceiverFirstName",
                            input.ReceiverFirstName),

                        new SqlParameter(
                            "@ReceiverLastName",
                            input.ReceiverLastName),

                        new SqlParameter(
                            "@ReceiverNationalCode",
                            input.ReceiverNationalCode),

                        new SqlParameter(
                            "@ReceiverPhone",
                            input.ReceiverPhone),

                        new SqlParameter(
                            "@PaymentMethod",
                            input.PaymentMethod),

                        new SqlParameter(
                            "@ShippingMethodId",
                            input.ShippingMethodId),

                        new SqlParameter(
                            "@AddressId",
                            input.AddressId),

                        new SqlParameter(
                            "@PaymentStatus",
                            (int)PaymentStatus.Pending),

                        new SqlParameter(
                            "@FinalAmount",
                            finalAmount),

                        new SqlParameter(
                            "@ReceiptFile",
                            (object?)input.ReceiptUrl ?? DBNull.Value)
                    );

                // ============================================
                // 17. بررسی نتیجه Stored Procedure
                // ============================================

                if (orderResult == null)
                {
                    await _transactionService.RollbackAsync();

                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "در ثبت سفارش مشکلی پیش آمد. لطفاً مجدداً تلاش کنید."
                    };
                }


                // ============================================
                // 18. بررسی موفقیت Stored Procedure
                // ============================================

                if (!orderResult.Success)
                {
                    await _transactionService.RollbackAsync();

                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = orderResult.Message
                    };
                }


                // ============================================
                // 19. دریافت سفارش ثبت‌شده
                // ============================================

                var createdCart =
                    await Query()
                    .Include(x => x.ShoppingCartItems)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.ShoppingCartItems)
                        .ThenInclude(x => x.Variant)
                    .FirstOrDefaultAsync(
                        x => x.Id == cart.Id,
                        cancellationToken);


                // ============================================
                // 20. بررسی دریافت سفارش
                // ============================================

                if (createdCart == null)
                {
                    await _transactionService.RollbackAsync();

                    return new ResultDto<ShoppingCartDto>
                    {
                        Success = false,
                        Message = "سفارش ثبت شد اما اطلاعات سفارش قابل دریافت نیست."
                    };
                }


                // ============================================
                // 21. Commit نهایی Transaction
                // ============================================

                await _transactionService.CommitAsync();


                // ============================================
                // 22. خروجی موفق
                // ============================================

                return new ResultDto<ShoppingCartDto>
                {
                    Success = true,

                    Message = orderResult.Message,

                    Data = _mapper.Map<ShoppingCartDto>(createdCart)
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<ShoppingCartDto>
                {
                    Success = false,
                    Message =
                        errorMessage,

                    Errors =
            {
                ex.Message
            }
                };
            }
        }
        private async Task<ResultDto<bool>> ValidateAddressAsync(
    Guid? userId,
    Guid addressId)
        {
            var address =
                await _addressService.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == addressId &&
                    x.UserId == userId);


            if (address == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "آدرس انتخاب شده معتبر نیست."
                };
            }


            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };
        }
        private async Task<ResultDto<bool>> ValidateShippingAsync(
            Guid shippingMethodId,
            Guid addressId)
        {
            var address =
                await _addressService.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == addressId);


            if (address == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "آدرس پیدا نشد."
                };
            }

            var shipping =
                await _shippingMethodService.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == shippingMethodId &&
                    x.IsActive);

            if (shipping == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "روش ارسال انتخاب شده معتبر نیست."
                };
            }


            // اگر سراسری نیست، باید شهر بررسی شود
            if (!shipping.IsNationwide)
            {
                var available =
                    await _shippingMethodCityService.Query()
                    .AnyAsync(x =>
                        x.ShippingMethodId == shippingMethodId &&
                        x.CityId == address.CityId);


                if (!available)
                {
                    return new ResultDto<bool>
                    {
                        Success = false,
                        Message =
                        "این روش ارسال برای شهر انتخاب شده فعال نیست."
                    };
                }
            }


            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };
        }
        private ResultDto<bool> ValidateReceiver(
           CreateOrderRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input.ReceiverFirstName))
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        "نام گیرنده الزامی است."
                };
            }


            if (string.IsNullOrWhiteSpace(input.ReceiverLastName))
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        "نام خانوادگی گیرنده الزامی است."
                };
            }


            if (string.IsNullOrWhiteSpace(input.ReceiverPhone))
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        "شماره تماس گیرنده الزامی است."
                };
            }


            if (string.IsNullOrWhiteSpace(input.ReceiverNationalCode))
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        "کد ملی گیرنده الزامی است."
                };
            }


            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };
        }

        private ResultDto<bool> ValidatePayment(CreateOrderRequestDto input)
        {
            if (!Enum.IsDefined(
                typeof(PaymentMethod),
                input.PaymentMethod))
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        "روش پرداخت نامعتبر است."
                };
            }


            if (input.PaymentMethod == (int)PaymentMethod.CardToCard)
            {
                if (string.IsNullOrWhiteSpace(input.ReceiptUrl))
                {
                    return new ResultDto<bool>
                    {
                        Success = false,
                        Message =
                            "ارسال رسید پرداخت برای پرداخت کارت به کارت الزامی است."
                    };
                }
            }


            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };
        }
        private async Task<ResultDto<bool>> ValidateCouponAsync(
            ShoppingCart cart,
            Guid? userId)
        {
            if (!cart.CouponId.HasValue)
            {
                return new ResultDto<bool>
                {
                    Success = true,
                    Data = true
                };
            }

            var coupon =
                await _couponService.Value.GetByIdAsync(
                    cart.CouponId.Value);

            if (coupon == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        "کد تخفیف دیگر معتبر نیست. لطفاً آن را حذف کنید."
                };
            }

            var validation =
                await _couponUsageService.Value.ValidateCouponAsync(
                    coupon.Data,
                    cart.Id,
                    userId);

            if (!validation.IsValid)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message =
                        validation.Message
                        ?? "کد تخفیف دیگر معتبر نیست. لطفاً آن را حذف کنید."
                };
            }

            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };
        }
        private async Task<ResultDto<bool>> ValidateCartItemsAsync(
    ShoppingCart cart,
    Guid? userId,
    CancellationToken cancellationToken = default)
        {
            try
            {
                var activeDiscounts =
                    await _discountService.GetActiveDiscountsAsync();


                foreach (var item in cart.ShoppingCartItems)
                {
                    // ============================================
                    // 1. بررسی وجود محصول
                    // ============================================

                    if (item.Product == null)
                    {
                        return new ResultDto<bool>
                        {
                            Success = false,
                            Message =
                                "یکی از محصولات سبد خرید دیگر موجود نیست."
                        };
                    }


                    // ============================================
                    // 2. قیمت فعلی محصول
                    // ============================================

                    var currentPrice =
                        item.Variant != null
                            ? item.Variant.Price
                            : item.Product.Price ?? 0;



                    if (currentPrice != item.UnitPrice)
                    {
                        return new ResultDto<bool>
                        {
                            Success = false,
                            Message =
                                $"قیمت محصول {item.Product.Name} تغییر کرده است. لطفاً سبد خرید را بروزرسانی کنید."
                        };
                    }



                    // ============================================
                    // 3. محاسبه تخفیف فعلی
                    // ============================================

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
                                    currentPrice
                            },
                            activeDiscounts);



                    if (item.DiscountAmount != discount.DiscountAmount ||
                        item.FinalUnitPrice != discount.FinalPrice)
                    {
                        return new ResultDto<bool>
                        {
                            Success = false,
                            Message =
                                $"تخفیف محصول {item.Product.Name} تغییر کرده است. لطفاً سبد خرید را بروزرسانی کنید."
                        };
                    }



                    // ============================================
                    // 4. بررسی موجودی
                    // ============================================

                    var inventory =
                        await _productInventoryTransactionService
                            .GetInventoryAsync(
                                item.ProductId,
                                item.VariantId);



                    if (inventory <= 0)
                    {
                        return new ResultDto<bool>
                        {
                            Success = false,
                            Message =
                                $"محصول {item.Product.Name} ناموجود شده است."
                        };
                    }



                    if (inventory < item.Quantity)
                    {
                        return new ResultDto<bool>
                        {
                            Success = false,
                            Message =
                                $"تعداد موجودی محصول {item.Product.Name} کمتر از مقدار انتخابی است."
                        };
                    }
                }



                // ============================================
                // همه موارد صحیح است
                // ============================================

                return new ResultDto<bool>
                {
                    Success = true,
                    Data = true
                };

            }
            catch (Exception ex)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        private async Task<string> GenerateUniqueOrderCodeAsync()
        {
            const string characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            while (true)
            {
                var code = new char[7];

                for (var i = 0; i < code.Length; i++)
                {
                    code[i] = characters[Random.Shared.Next(characters.Length)];
                }

                var orderCode = new string(code);

                var exists = await Query()
                    .AnyAsync(x => x.OrderCode == orderCode);

                if (!exists)
                {
                    return orderCode;
                }
            }
        }
    }
}
