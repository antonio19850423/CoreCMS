using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ProductGqlResolver : IProductGqlResolver
{
    IProductService _ProductService;
    public ProductGqlResolver(IProductService ProductService)
    {
        _ProductService = ProductService;
    }
    /// <summary>
    /// قوانین کلی GraphQL Resolver:
    /// - نام کلاس باید به GqlResolver ختم شود
    /// - نام Query باید به صورت EntityName + View و به شکل camelCase باشد
    /// - تمام فیلدهای nullable باید مقدار پیش‌فرض داشته باشند (جلوگیری از null)
    /// - View باید از مدل Sql<Entity>View استفاده کند
    /// - Entity و View باید در globalUsing.cs ثبت شده باشند
    /// - در تنظیمات GraphQL باید از AddTypeExtension استفاده شود
    /// - منطق بیزینسی داخل Resolver قرار نگیرد (فقط Mapping و Query)
    /// - عملیات Read/List فقط از طریق GraphQL انجام می‌شود (نه Service)
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [GraphQLName("productView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductCrud>> productView()
    {
        var query = await _ProductService
            .GetAllViewQueryable<SqlProductView, SqlProductView, ProductCrud>();
        return query.Select(x => new ProductCrud
        {
            Id = x.Id,
            IsActive = x.IsActive,
            SortOrder = x.SortOrder,
            Weight = x.Weight,
            Thumbnail = x.Thumbnail??"",
            Summary = x.Summary??"",
            Slug = x.Slug??"",
            Barcode = x.Barcode ?? "",
            BrandId = x.BrandId,
            BrandName = x.BrandName??"",
            CategoryId=x.CategoryId,
            CategoryName= x.CategoryName ?? "",
            Description= x.Description ?? "",
            IsFeatured= x.IsFeatured,
            IsPublished= x.IsPublished,
            MainImage= x.MainImage ??"",
            Name= x.Name ??"",
            Price= x.Price ,
            ProductTypeId=x.ProductTypeId,
            ProductTypeName=x.ProductTypeName??"",
            SaleCount=x.SaleCount,
            SeoDescription=x.SeoDescription ??"",
            SeoTitle=x.SeoTitle ??"",
            ShouldInsert=x.ShouldInsert,
            Sku=x.Sku ??"",
            ViewCount = x.ViewCount
        });
    }

}
