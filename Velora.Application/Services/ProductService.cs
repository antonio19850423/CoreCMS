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
    public class ProductService : GenericService<SqlProduct, SqlProduct, ProductDto>, IProductService
    {
        private readonly ISqlRepository<SqlProduct> _sqlrepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IModelValidationService _modelValidationService;
        protected readonly Lazy<ILocalizationMessageService> _messageService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly Lazy<IExcelTemplateService> _excelTemplateService;
        private readonly IProductService _roleProductService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly IProductTagService _ProductTagService;
        protected readonly IProductTagMappingService _productTagMappingService;
        protected readonly IProductInventoryTransactionService _productInventoryService;
        

        public ProductService(
              ISqlRepository<SqlProduct> sqlRepository,
              IPosgreSqlRepository<SqlProduct> pgRepository,
              IMapper mapper,
              IConfiguration configuration, ITransactionService transactionService, IWebHostEnvironment env,
              Lazy<ILocalizationMessageService> messageService, IModelValidationService modelValidationService, IConfiguration config, Lazy<IExcelTemplateService> excelTemplateService,
              ICurrentUserService currentUserService, IProductTagService ProductTagService, IProductTagMappingService productTagMappingService, IProductInventoryTransactionService productInventoryService)
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
            _ProductTagService = ProductTagService;
            _productTagMappingService = productTagMappingService;
            _productInventoryService = productInventoryService;
        }
        public async Task<IQueryable<ProductCrud>> GetAllViews()
        {
            return await GetAllViewQueryable<SqlProductView, SqlProductView, ProductCrud>();
        }

        public async Task<ResultDto<ProductDto>> CreateAsync(ProductCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };



                var Product = new ProductDto
                {
                    SortOrder = input.SortOrder,
                    Barcode = input.Barcode,
                    BrandId = input.BrandId,
                    CategoryId = input.CategoryId,
                    Description = input.Description,
                    IsActive = input.IsActive,
                    IsFeatured = input.IsFeatured,
                    IsPublished = input.IsPublished,
                    MainImage = input.MainImage,
                    Name = input.Name,
                    Price = input.Price,
                    ProductTypeId = input.ProductTypeId,
                    SeoDescription = input.SeoDescription,
                    SeoTitle = input.SeoTitle,
                    Sku = input.Sku,
                    Slug = input.Slug,
                    Summary = input.Summary,
                    Thumbnail = input.Thumbnail,
                    Weight = input.Weight,
                    
                };

                var ProductResult = await CreateAsync(Product);
                if (!ProductResult.Success)
                    return ProductResult;
                var ProductId = ProductResult.Data.Id;


                var ProductTagIds = input.ProductTagIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(Guid.Parse)
                       .ToList();

                var ProductTags = await _productTagMappingService.GetByProductTagMappingsAsync(ProductId);

                var tagsToRemove = ProductTags
                    .Where(r => !ProductTagIds.Contains(r.ProductTagId))
                    .ToList();

                foreach (var r in tagsToRemove)
                {
                    await _productTagMappingService.DeleteAsync(r.Id);
                }

                if (ProductTagIds!=null&&ProductTagIds.Any())
                {
                    foreach (var Tag in ProductTagIds)
                    {
                        var ProductTag = new ProductTagMappingDto
                        {
                            ProductTagId = Tag,
                            ProductId = ProductId
                        };
                        await _productTagMappingService.CreateAsync(ProductTag);
                    }

                }
                await _transactionService.CommitAsync();
                return ProductResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductDto>
                {
                    Success = false,
                    Message = errorMessage,
                };
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<ResultDto<ProductDto>> UpdateAsync(ProductCrud input)
        {
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            try
            {
                if (input.Id == null)
                {
                    return new ResultDto<ProductDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.IdRequired)
                    };
                }
                var validation = await _modelValidationService.ValidateAsync(input);
                if (!validation.Success)
                    return new ResultDto<ProductDto>
                    {
                        Success = false,
                        Message = await _messageService.Value.GetMessageAsync(LocalizationKeys.ValidationFailed, "Form has errors. Please fix them."),
                        Errors = validation.Data
                    };

                // 1️⃣ به‌روزرسانی کاربر
                var updateDto = new ProductDto
                {
                    Id = input.Id,
                    SortOrder = input.SortOrder,
                    Barcode = input.Barcode,
                    BrandId = input.BrandId,
                    CategoryId = input.CategoryId,
                    Description = input.Description,
                    IsActive = input.IsActive,
                    IsFeatured = input.IsFeatured,
                    IsPublished = input.IsPublished,
                    MainImage = input.MainImage,
                    Name = input.Name,
                    Price = input.Price,
                    ProductTypeId = input.ProductTypeId,
                    SeoDescription = input.SeoDescription,
                    SeoTitle = input.SeoTitle,
                    Sku = input.Sku,
                    Slug = input.Slug,
                    Summary = input.Summary,
                    Thumbnail = input.Thumbnail,
                    Weight = input.Weight,
                };

                var ProductResult = await UpdateAsync(updateDto, input.Id);
                if (!ProductResult.Success)
                    return ProductResult;
                var ProductId = ProductResult.Data.Id;

                var ProductTagIds = input.ProductTagIds?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Guid.Parse)
                    .Distinct()
                    .ToList() ?? new List<Guid>();

                var existingProductTags = await _productTagMappingService
                    .GetByProductTagMappingsAsync(ProductId);


                // حذف مواردی که دیگر وجود ندارند
                var tagsToRemove = existingProductTags
                    .Where(x => !ProductTagIds.Contains(x.ProductTagId))
                    .ToList();

                foreach (var tag in tagsToRemove)
                {
                    await _productTagMappingService.DeleteAsync(tag.Id);
                }


                // اضافه کردن فقط موارد جدید
                var existingTagIds = existingProductTags
                    .Select(x => x.ProductTagId)
                    .ToHashSet();


                var tagsToAdd = ProductTagIds
                    .Where(x => !existingTagIds.Contains(x))
                    .ToList();


                foreach (var tagId in tagsToAdd)
                {
                    await _productTagMappingService.CreateAsync(new ProductTagMappingDto
                    {
                        ProductId = ProductId,
                        ProductTagId = tagId
                    });
                }
                await _transactionService.CommitAsync();
                return ProductResult;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                var result = new ResultDto<ProductDto>
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
            var createdProducts= new List<ProductDto>();
            var errors = new List<string>();
            var (successMessage, errorMessage) = await _messageService.Value.GetSaveMessagesAsync();
            var errorFileTitle = await _messageService.Value.GetMessageAsync(LocalizationKeys.ErrorFile);
            try
            {
                var (dt, rowContexts) = excelStream.LoadExcelWithErrors();
                var Products = dt.ToModelList<ProductCrud>();

                for (int i = 0; i < Products.Count; i++)
                {
                    var Product = Products[i];
                    var context = rowContexts[i];

                    var createResult = await CreateAsync(Product);

                    if (createResult.Success && createResult.Data != null)
                    {
                        createdProducts.Add(createResult.Data);
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
                        InsertedCount = createdProducts.Count,
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
bool exportCurrentProduct,
int ProductNumber,
int ProductSize)
        {
            // 1️⃣ گرفتن همه داده‌ها از query
            var query = await GetAllViews(); // IQueryable<Resource>

            // 2️⃣ Paging و Mapping به DTO
            List<ProductCrud> data;

            if (exportCurrentProduct)
            {
                data = query
                    .Skip((ProductNumber - 1) * ProductSize)
                    .Take(ProductSize)
                    .ToList();
            }
            else
            {
                data = query.ToList();
            }
            var resource = _mapper.Map<List<ProductCrud>>(data);

            // 3️⃣ تولید Template اکسل با Lookup (مثلاً 5 ردیف خالی اضافه)
            var templateBytes = await _excelTemplateService.Value.GenerateTemplateWithLookupsAsync(
                LookupEntities.Product, // نام مدل DTO
                data.Count + 5
            );

            // 4️⃣ پر کردن داده‌ها در Template با Extension Method
            var resultBytes = templateBytes.FillDataIntoTemplate(data, startRow: 3);

            return resultBytes;
        }

        public async Task<ResultDto<ProductListResultDto>> GetProductsAsync(
            int page,
            int pageSize,
            string? categorySlug,
            string? brandSlug,
            string? search,
            string sort,
            decimal? minPrice = null,
            decimal? maxPrice = null)
        {
            try
            {

                var query =
                    Query()

                    .Include(x => x.Category)

                    .Include(x => x.Brand)

                    .Include(x => x.ProductFiles)
                    .Include(x => x.ProductVariants)
                    .Include(x => x.ProductTagMappings)
                        .ThenInclude(x => x.ProductTag)

                    .Where(x =>
                        x.IsPublished == true &&
                        x.IsActive == true);



                // ===============================
                // Category Filter
                // ===============================

                if (!string.IsNullOrWhiteSpace(categorySlug))
                {
                    query =
                        query.Where(x =>
                            x.Category != null &&
                            x.Category.Slug == categorySlug);
                }



                // ===============================
                // Brand Filter
                // ===============================

                if (!string.IsNullOrWhiteSpace(brandSlug))
                {
                    query =
                        query.Where(x =>
                            x.Brand != null &&
                            x.Brand.Slug == brandSlug);
                }



                // ===============================
                // Search
                // ===============================

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query =
                        query.Where(x =>
                            x.Name.Contains(search));
                }


                // ===============================
                // Price Filter
                // ===============================

                if (minPrice.HasValue)
                {
                    query =
                        query.Where(x =>
                            (x.ProductVariants.Any()
                                ? x.ProductVariants.Min(v => v.Price)
                                : x.Price) >= minPrice.Value);
                }


                if (maxPrice.HasValue)
                {
                    query =
                        query.Where(x =>
                            (x.ProductVariants.Any()
                                ? x.ProductVariants.Min(v => v.Price)
                                : x.Price) <= maxPrice.Value);
                }

                // ===============================
                // Sort
                // ===============================

                query =
                    sort switch
                    {
                        "priceAsc" =>
                            query.OrderBy(x =>
                                x.ProductVariants.Any()
                                    ? x.ProductVariants.Min(v => v.Price)
                                    : x.Price),


                        "priceDesc" =>
                            query.OrderByDescending(x =>
                                x.ProductVariants.Any()
                                    ? x.ProductVariants.Max(v => v.Price)
                                    : x.Price),


                        "oldest" =>
                            query.OrderBy(x => x.CreatedAt),


                        _ =>
                            query.OrderByDescending(x => x.CreatedAt)
                    };





                // ===============================
                // Total Count
                // ===============================

                var total =
                    await query.CountAsync();






                // ===============================
                // Pagination
                // ===============================

                var products =
                    await query

                    .Skip((page - 1) * pageSize)

                    .Take(pageSize)

                    .ToListAsync();






                // ===============================
                // Inventory Bulk Calculation
                // ===============================


                var productIds =
                    products
                    .Select(x => x.Id)
                    .ToList();



                var inventories =
                    await _productInventoryService
                        .GetInventoryAsync(productIds);







                // ===============================
                // Mapping
                // ===============================

                var data =
                    products.Select(x =>
                    {

                        inventories.TryGetValue(
                            x.Id,
                            out var inventory);



                        return new ProductListViewDto
                        {

                            Id = x.Id,


                            Name = x.Name,


                            Slug = x.Slug,


                            Summary = x.Summary,


                            Price =
    x.ProductVariants.Count == 1
        ? x.ProductVariants.First().Price
        : x.ProductVariants.Any()
            ? x.ProductVariants.Min(v => v.Price)
            : x.Price ?? 0,



                            MainImage = x.MainImage,


                            Thumbnail = x.Thumbnail,



                            CategoryName =
                                x.Category?.Name,


                            CategorySlug =
                                x.Category?.Slug,



                            BrandName =
                                x.Brand?.Name,


                            BrandSlug =
                                x.Brand?.Slug,



                            Inventory = inventory,



                            CreatedAt =
                                x.CreatedAt,


                            HasVariant =
    x.ProductVariants.Count > 1,


                            DefaultVariantId =
    x.ProductVariants.Count == 1
        ? x.ProductVariants.First().Id
        : null,


                            Gallery =
                                x.ProductFiles

                                .OrderBy(f => f.SortOrder)

                                .Select(m =>
                                    new ProductMediaViewDto
                                    {
                                        Id = m.Id,

                                        FileUrl = m.FileUrl,

                                        ThumbnailUrl =
                                            m.ThumbnailUrl,

                                        IsMain =
                                            m.IsMain,

                                        SortOrder =
                                            m.SortOrder

                                    })
                                .ToList(),

                        };

                    })

                    .ToList();







                return new ResultDto<ProductListResultDto>
                {
                    Success = true,

                    Data = new ProductListResultDto
                    {
                        Items = data,

                        TotalCount = total,

                        Page = page,

                        PageSize = pageSize
                    }
                };

            }
            catch (Exception ex)
            {

                return new ResultDto<ProductListResultDto>
                {
                    Success = false,

                    Message = "خطا در دریافت لیست محصولات",

                    Errors =
            {
                ex.Message
            }
                };

            }
        }


        public async Task<ResultDto<ProductDetailViewDto>> GetProductDetailAsync(string? slug)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(slug))
                {
                    return new ResultDto<ProductDetailViewDto>
                    {
                        Success = false,
                        Message = "شناسه محصول نامعتبر است"
                    };
                }


                var product = await Query()

                    .Include(x => x.Category)

                    .Include(x => x.Brand)


                    .Include(x => x.ProductFiles)


                    .Include(x => x.ProductAttributeValues)
                        .ThenInclude(x => x.ProductAttribute)


                    .Include(x => x.ProductVariants)


                    .Include(x => x.ProductTagMappings)
                        .ThenInclude(x => x.ProductTag)


                    .FirstOrDefaultAsync(x =>
                        x.Slug == slug &&
                        x.IsPublished==true &&
                        x.IsActive==true);



                if (product == null)
                {
                    return new ResultDto<ProductDetailViewDto>
                    {
                        Success = false,
                        Message = "محصول پیدا نشد"
                    };
                }



                // ==========================
                // Inventory
                // ==========================

                var inventory =
                    await _productInventoryService
                        .GetInventoryAsync(product.Id);

                // ======================
                // Variants
                // ======================

                var variants = new List<ProductVariantViewDto>();

                foreach (var x in product.ProductVariants.OrderBy(x => x.SortOrder))
                {
                    var stock =
                        await _productInventoryService
                            .GetInventoryAsync(product.Id, x.Id);

                    variants.Add(new ProductVariantViewDto
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Name = x.Name,
                        Price = x.Price,
                        ComparePrice = x.ComparePrice,
                        Sku = x.Sku,
                        Barcode = x.Barcode,
                        Image = x.Image,
                        IsDefault = x.IsDefault,
                        SortOrder = x.SortOrder,
                        IsActive = x.IsActive,
                        Stock = stock
                    });
                }


                // ==========================
                // Mapping
                // ==========================

                var data = new ProductDetailViewDto
                {

                    Id = product.Id,


                    Name = product.Name,


                    Slug = product.Slug,


                    Summary = product.Summary,


                    Description = product.Description,


                    MainImage = product.MainImage,


                    Thumbnail = product.Thumbnail,



                    Price =
                        product.ProductVariants.Count == 1
                        ?
                        product.ProductVariants.First().Price
                        :
                        product.ProductVariants.Any()
                        ?
                        product.ProductVariants.Min(x => x.Price)
                        :
                        product.Price ?? 0,





                    Category =
                        product.Category == null
                        ?
                        null
                        :
                        new CategoryViewDto
                        {
                            Id = product.Category.Id,

                            Name = product.Category.Name,

                            Slug = product.Category.Slug
                        },



                    Brand =
                        product.Brand == null
                        ?
                        null
                        :
                        new BrandViewDto
                        {
                            Id = product.Brand.Id,

                            Name = product.Brand.Name,

                            Slug = product.Brand.Slug
                        },



                    // ======================
                    // Gallery
                    // ======================

                    Gallery =
                        product.ProductFiles

                        .OrderBy(x => x.SortOrder)

                        .Select(x => new ProductMediaViewDto
                        {
                            Id = x.Id,

                            ProductId = x.ProductId,

                            FileUrl = x.FileUrl,

                            ThumbnailUrl = x.ThumbnailUrl,

                            Title = x.Title,

                            Alt = x.Alt,

                            MediaType = x.MediaType,

                            IsMain = x.IsMain,

                            SortOrder = x.SortOrder

                        })
                        .ToList(),



                    // ======================
                    // Attributes
                    // ======================

                    Attributes =
                        product.ProductAttributeValues

                        .Select(x => new ProductAttributeViewDto
                        {
                            Id = x.Id,

                            Name = x.ProductAttribute.Name,

                            Code = x.ProductAttribute.Code,

                            Value = x.Value

                        })
                        .ToList(),




                    // ======================
                    // Variants
                    // ======================

                    Variants = variants.ToList(),




                    // ======================
                    // Tags
                    // ======================

                    Tags =
                        product.ProductTagMappings

                        .Select(x => new ProductTagViewDto
                        {
                            Id = x.ProductTag.Id,

                            Name = x.ProductTag.Name,

                            Slug = x.ProductTag.Slug

                        })
                        .ToList()

                };



                return new ResultDto<ProductDetailViewDto>
                {
                    Success = true,

                    Data = data
                };

            }
            catch (Exception ex)
            {
                return new ResultDto<ProductDetailViewDto>
                {
                    Success = false,

                    Message = "خطا در دریافت اطلاعات محصول",

                    Errors =
            {
                ex.Message
            }
                };
            }
        }
    }

}
