using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class ProductCategoryGqlResolver : IProductCategoryGqlResolver
{
    IProductCategoryService _ProductCategoryService;
    public ProductCategoryGqlResolver(IProductCategoryService ProductCategoryService)
    {
        _ProductCategoryService = ProductCategoryService;
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
    [GraphQLName("productCategoryView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductCategoryCrud>> productCategoryView()
    {
        var query = await _ProductCategoryService
            .GetAllViewQueryable<VwProductCategoryForm, VwProductCategoryForm, ProductCategoryCrud>();
        return query.Select(x => new ProductCategoryCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            SortOrder = x.SortOrder,
            Icon=x.Icon??"",
            Name = x.Name??"",
            Description=x.Description??"",
            IconColor=x.IconColor??"",
            Slug=x.Slug??"",
            ParentName = x.ParentName??"",
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            SeoDescription = x.SeoDescription??"",
            SeoTitle=x.SeoTitle??"",
            ShouldInsert = x.ShouldInsert
        });
    }

}
