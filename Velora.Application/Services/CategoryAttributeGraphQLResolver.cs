using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class CategoryAttributeGqlResolver : ICategoryAttributeGqlResolver
{
    ICategoryAttributeService _CategoryAttributeService;
    public CategoryAttributeGqlResolver(ICategoryAttributeService CategoryAttributeService)
    {
        _CategoryAttributeService = CategoryAttributeService;
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
    [GraphQLName("categoryAttributeView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<CategoryAttributeCrud>> categoryAttributeView()
    {
        var query = await _CategoryAttributeService
            .GetAllViewQueryable<SqlCategoryAttributeView, SqlCategoryAttributeView, CategoryAttributeCrud>();
        return query.Select(x => new CategoryAttributeCrud
        {
            Id = x.Id,
            SortOrder=x.SortOrder,
            CategoryId = x.CategoryId,
            CategoryName = x.CategoryName??"",
            AttributeId = x.AttributeId,
            AttributeCode = x.AttributeCode??"",
            AttributeName = x.AttributeName??"",
            CategorySlug=x.CategorySlug??"",
            CreatedAtPersian = x.CreatedAtPersian??"",
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
