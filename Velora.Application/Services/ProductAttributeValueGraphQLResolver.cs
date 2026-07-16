using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ProductAttributeValueGqlResolver : IProductAttributeValueGqlResolver
{
    IProductAttributeValueService _ProductAttributeValueService;
    public ProductAttributeValueGqlResolver(IProductAttributeValueService ProductAttributeValueService)
    {
        _ProductAttributeValueService = ProductAttributeValueService;
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
    [GraphQLName("productAttributeValueView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductAttributeValueCrud>> productAttributeValueView()
    {
        var query = await _ProductAttributeValueService
            .GetAllViewQueryable<SqlProductAttributeValueView, SqlProductAttributeValueView, ProductAttributeValueCrud>();
        return query.Select(x => new ProductAttributeValueCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            Value = x.Value??"",
            ProductAttributeCode = x.ProductAttributeCode??"",
            ProductAttributeId=x.ProductAttributeId,
            SortOrder = x.SortOrder,
            ProductAttributeName=x.ProductAttributeName??"",
            ShouldInsert = x.ShouldInsert,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ??"",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
        });
    }

}
