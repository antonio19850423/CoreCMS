using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class DiscountItemGqlResolver : IDiscountItemGqlResolver
{
    IDiscountItemService _DiscountItemService;
    public DiscountItemGqlResolver(IDiscountItemService DiscountItemService)
    {
        _DiscountItemService = DiscountItemService;
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
    [GraphQLName("discountItemView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<DiscountItemCrud>> discountItemView()
    {
        var query = await _DiscountItemService
            .GetAllViewQueryable<VwDiscountItemForm, VwDiscountItemForm, DiscountItemCrud>();
        return query.Select(x => new DiscountItemCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            SortOrder = x.SortOrder,
            ProductVariantId = x.ProductVariantId,
            ProductName = x.ProductName??"",
            ProductCategoryId = x.ProductCategoryId,
            ProductBrandId = x.ProductBrandId,
            ProductId = x.ProductId,
            CreatedAtPersian = x.CreatedAtPersian??"",
            UpdatedAtPersian = x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName??"",
            ParentName=x.ParentName??"",
            ProductBrandName=x.ProductBrandName??"",
            ProductCategoryName=x.ProductCategoryName??"",
            ProductVariantName=x.ProductVariantName??"",
            UpdatedByName=x.UpdatedByName?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
