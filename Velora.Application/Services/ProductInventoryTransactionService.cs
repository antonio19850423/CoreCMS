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
    public class ProductInventoryTransactionService : GenericService<SqlProductInventoryTransaction, SqlProductInventoryTransaction, ProductInventoryTransactionDto>, IProductInventoryTransactionService
    {
        private readonly ISqlRepository<SqlProductInventoryTransaction> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductInventoryTransactionService _roleProductInventoryTransactionService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IPaymentService _paymentService;
        protected readonly IShoppingCartItemService _shoppingCartItemService;
        private readonly IProductVariantService _productVariantService;


        public ProductInventoryTransactionService(
              ISqlRepository<SqlProductInventoryTransaction> sqlRepository,
              IPosgreSqlRepository<SqlProductInventoryTransaction> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IPaymentService paymentService, IShoppingCartItemService shoppingCartItemService, IProductVariantService productVariantService)
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
            _paymentService = paymentService;
            _shoppingCartItemService = shoppingCartItemService;
            _productVariantService = productVariantService;
        }
        public async Task<IQueryable<ProductInventoryTransactionCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlProductInventoryTransactionView, SqlProductInventoryTransactionView, ProductInventoryTransactionCrud>();
        }
        public async Task<IQueryable<SqlProductInventoryTransactionView>> GetInventoryTransactionQuery()
        {
            return _sqlRepository
                .GetViewQueryable<SqlProductInventoryTransactionView>();
        }

        public async Task<ResultDto<ProductInventoryTransactionDto>> CreateAsync(ProductInventoryTransactionCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductInventoryTransactionDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var ProductInventoryTransaction = new ProductInventoryTransactionDto
                {
                    ChangeQuantity= input.ChangeQuantity.Value,
                    Note= input.Note,
                    OperationType= input.OperationType.Value,
                    ProductId =  input.ProductId,
                    ProductVariantId =input.ProductVariantId,
                    ReasonId=input.ReasonId.Value,
                    ReferenceDetailId=input.ReferenceDetailId,
                    ReferenceId=input.ReferenceId,

                };

                var ProductInventoryTransactionResult = await CreateAsync(ProductInventoryTransaction);
                if (!ProductInventoryTransactionResult.Success)
                    return ProductInventoryTransactionResult;
                return ProductInventoryTransactionResult;
            }
            catch (Exception ex)
            {
                var result = new ResultDto<ProductInventoryTransactionDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }
        public async Task<ProductInventoryTransactionCrud?> GetProductInventoryTransactionById(Guid id)
        {
            return await GetSqlViewByIdQueryable<SqlProductInventoryTransactionView, ProductInventoryTransactionCrud>(id);
        }

        public async Task<ResultDto<ProductInventoryTransactionCrud>> CreateAdvancedAsync(
            ProductInventoryTransactionCrud input)
        {
            var validation = await _modelValidationService.ValidateAsync(input);

            if (!validation.Success)
            {
                return new ResultDto<ProductInventoryTransactionCrud>
                {
                    Success = false,
                    Message = await _messageService.Value.GetMessageAsync(
                        LocalizationKeys.ValidationFailed,
                        "Form has errors. Please fix them."),
                    Errors = validation.Data
                };
            }

            Guid? productId = input.ParentId != Guid.Empty
                ? input.ParentId
                : input.ProductId;

            // اگر واریانت انتخاب شده، محصول اصلی آن را پیدا کن
            if (input.ProductVariantId.HasValue &&
                input.ProductVariantId.Value != Guid.Empty)
            {
                var variant = await _productVariantService.GetByIdAsync(input.ProductVariantId);

                if (variant == null || variant.Data == null)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = "تنوع محصول معتبر نیست.",
                        Errors = new List<string>
        {
             "تنوع محصول معتبر نیست."
        }
                    };
                }

                productId = variant.Data.ProductId;

                if (productId == Guid.Empty)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = "محصول مرتبط با تنوع انتخاب‌شده معتبر نیست.",
                        Errors = new List<string>
                {
                    "ProductId برای تنوع محصول پیدا نشد."
                }
                    };
                }
            }
            else
            {
                // انتخاب محصول بدون واریانت
                input.ProductVariantId = null;
            }

            // بررسی انتخاب محصول یا واریانت
            if (!productId.HasValue || productId.Value == Guid.Empty)
            {
                return new ResultDto<ProductInventoryTransactionCrud>
                {
                    Success = false,
                    Message = "محصول یا تنوع محصول باید مقدار داشته باشد.",
                    Errors = new List<string>
            {
                "محصول یا تنوع محصول باید مقدار داشته باشد."
            }
                };
            }

            // بررسی موجودی فقط برای عملیات کاهشی
            if (input.OperationType == 2)
            {
                var availableInventory = await GetInventoryAsync(
                    productId.Value,
                    input.ProductVariantId
                );

                if (input.ChangeQuantity > availableInventory)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = $"امکان کاهش {input.ChangeQuantity} عدد وجود ندارد. حداکثر تعداد قابل کاهش در حال حاضر {availableInventory} عدد است.",
                        Errors = new List<string>
                {
                    $"موجودی قابل کاهش: {availableInventory}"
                }
                    };
                }
            }

            var productInventoryTransaction = new ProductInventoryTransactionDto
            {
                ChangeQuantity = input.ChangeQuantity.Value,
                Note = input.Note,
                OperationType = input.OperationType.Value,
                ProductId = productId.Value,
                ProductVariantId = input.ProductVariantId,
                ReasonId = input.ReasonId.Value,
                ReferenceDetailId = input.ReferenceDetailId,
                ReferenceId = input.ReferenceId
            };

            return await CreateWithViewAsync(
                productInventoryTransaction,
                GetProductInventoryTransactionById,
                x => x.Id
            );
        }
        public async Task<ResultDto<ProductInventoryTransactionCrud>> UpdateAsync(
            ProductInventoryTransactionCrud input)
        {
            var (successMessage, errorMessage) =
                await _messageService.Value.GetSaveMessagesAsync();

            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(
                            LocalizationKeys.IdRequired)
                    };
                }

                var validation = await _modelValidationService.ValidateAsync(input);

                if (!validation.Success)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(
                            LocalizationKeys.ValidationFailed,
                            "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };
                }

                // دریافت تراکنش قبلی
                var existingTransaction = await GetByIdAsync(input.Id);

                if (existingTransaction == null || existingTransaction.Data == null)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = "تراکنش موجودی موردنظر پیدا نشد."
                    };
                }

                var existingData = existingTransaction.Data;

                // فقط فیلدهای مجاز برای ویرایش تغییر می‌کنند
                var updateDto = new ProductInventoryTransactionDto
                {
                    Id = existingData.Id,
                    ProductId = existingData.ProductId,
                    ProductVariantId = existingData.ProductVariantId,
                    OperationType = existingData.OperationType,
                    ChangeQuantity = existingData.ChangeQuantity,

                    Note = input.Note,
                    ReasonId = input.ReasonId.Value,
                    ReferenceId = input.ReferenceId,
                    ReferenceDetailId = input.ReferenceDetailId
                };

                var updateResult = await UpdateAsync(updateDto, input.Id);

                if (!updateResult.Success)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = updateResult.Message,
                        Errors = updateResult.Errors
                    };
                }

                // دریافت View نهایی
                var viewResult = await GetProductInventoryTransactionById(input.Id);

                if (viewResult == null)
                {
                    return new ResultDto<ProductInventoryTransactionCrud>
                    {
                        Success = false,
                        Message = "اطلاعات تراکنش پس از ویرایش قابل دریافت نیست."
                    };
                }

                return new ResultDto<ProductInventoryTransactionCrud>
                {
                    Success = true,
                    Message = successMessage,
                    Data = viewResult
                };
            }
            catch (Exception ex)
            {
                var result = new ResultDto<ProductInventoryTransactionCrud>
                {
                    Success = false,
                    Message = errorMessage
                };

                result.Errors.Add(ex.Message);
                return result;
            }
        }
        public async Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream)
        {
            var createdProductInventoryTransactions= new List<ProductInventoryTransactionDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var ProductInventoryTransactions = dt.ToModelList<ProductInventoryTransactionCrud>();

                for (int i = 0; i < ProductInventoryTransactions.Count; i++)
                {
                    var ProductInventoryTransaction = ProductInventoryTransactions[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(ProductInventoryTransaction);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdProductInventoryTransactions.Add(createResult.Data);
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
                        InsertedCount = createdProductInventoryTransactions.Count,
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
        public async Task<int> GetAvailableQuantityAsync(
    Guid productId,
    Guid? productVariantId = null)
        {

            var query =
                await GetAllViews();



            var transactions =
                query.Where(x =>
                    x.ProductId == productId &&
                    x.ProductVariantId == productVariantId);



            var quantity =
                await transactions
                .SumAsync(x =>
                    x.OperationType.Value == 1
                    ? x.ChangeQuantity
                    : -x.ChangeQuantity);



            return quantity.Value;
        }





        public async Task<Dictionary<Guid, int>> GetInventoryAsync(
            List<Guid> productIds)
        {
            var query = await GetAllQuery();


            var result =
                await query
                .Where(x => productIds.Contains(x.ProductId))
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,

                    Quantity =
                        x.Sum(t =>
                            t.OperationType == 1
                            ? t.ChangeQuantity
                            : -t.ChangeQuantity)
                })
                .ToListAsync();


            return result.ToDictionary(
                x => x.ProductId,
                x => x.Quantity
            );
        }

        public async Task<int> GetInventoryAsync(
            Guid productId,
            Guid? productVariantId = null)
        {
            // =========================
            // Physical Inventory
            // =========================

            var inventoryQuery =
                await GetAllQuery();

            inventoryQuery =
                inventoryQuery.Where(x =>
                    x.ProductId == productId);

            if (productVariantId.HasValue)
            {
                inventoryQuery =
                    inventoryQuery.Where(x =>
                        x.ProductVariantId == productVariantId.Value);
            }
            else
            {
                inventoryQuery =
                    inventoryQuery.Where(x =>
                        !x.ProductVariantId.HasValue);
            }

            var physicalStock =
                await inventoryQuery.SumAsync(x =>
                    x.OperationType == 1
                        ? x.ChangeQuantity
                        : -x.ChangeQuantity);


            // =========================
            // Pending Reserved Inventory
            // =========================

            var pendingDate =
                DateTime.Now.AddDays(-3);

            var pendingCartIds =
                await _paymentService
                    .Query()
                    .Where(x =>
                        x.PaymentStatus ==
                            (int)PaymentStatus.Pending
                        &&
                        x.CreatedAt >= pendingDate
                        &&
                        x.ShoppingCartId.HasValue)
                    .Select(x =>
                        x.ShoppingCartId!.Value)
                    .Distinct()
                    .ToListAsync();

            var pendingReservedStock = 0;

            if (pendingCartIds.Any())
            {
                var cartItemsQuery =
                    _shoppingCartItemService
                        .Query()
                        .Where(x =>
                            pendingCartIds.Contains(
                                x.ShoppingCartId)
                            &&
                            x.ProductId == productId);

                if (productVariantId.HasValue)
                {
                    cartItemsQuery =
                        cartItemsQuery.Where(x =>
                            x.VariantId ==
                            productVariantId.Value);
                }
                else
                {
                    cartItemsQuery =
                        cartItemsQuery.Where(x =>
                            !x.VariantId.HasValue);
                }

                pendingReservedStock =
                    await cartItemsQuery
                        .SumAsync(x => x.Quantity);
            }


            // =========================
            // Available Inventory
            // =========================

            return Math.Max(
                0,
                physicalStock - pendingReservedStock);
        }
        public async Task<int> GetInventoryAsync(Guid productId)
{
    var query = await GetAllQuery();


    var quantity =
        await query

        .Where(x => x.ProductId == productId)

        .SumAsync(x =>
            x.OperationType == 1
            ? x.ChangeQuantity
            : -x.ChangeQuantity
        );


    return quantity;
}
        public async Task<byte[]> ExportAsync(
bool exportCurrentProductInventoryTransaction,
int ProductInventoryTransactionNumber,
int ProductInventoryTransactionSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ProductInventoryTransactionCrud> data;

            if (exportCurrentProductInventoryTransaction)
            {
                data = query
                    .Skip((ProductInventoryTransactionNumber - 1) * ProductInventoryTransactionSize)
                    .Take(ProductInventoryTransactionSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ProductInventoryTransactionCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.ProductInventoryTransaction, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }



    }

}
