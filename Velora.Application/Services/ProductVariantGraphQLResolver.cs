using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ProductVariantGqlResolver : IProductVariantGqlResolver
{
    IProductVariantService _ProductVariantService;
    public ProductVariantGqlResolver(IProductVariantService ProductVariantService)
    {
        _ProductVariantService = ProductVariantService;
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
    [GraphQLName("productVariantView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductVariantCrud>> productVariantView()
    {
        var query = await _ProductVariantService
            .GetAllViewQueryable<SqlProductVariantView, SqlProductVariantView, ProductVariantCrud>();
        return query.Select(x => new ProductVariantCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            SortOrder = x.SortOrder,
            ComparePrice = x.ComparePrice,
            Barcode = x.Barcode??"",
            Sku = x.Sku??"",
            IsDefault = x.IsDefault,
            Image=x.Image??"",
            Name = x.Name??"",
            Price= x.Price,
            ShouldInsert = x.ShouldInsert,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ??"",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ProductName=x.ProductName??""
        });
    }

}
