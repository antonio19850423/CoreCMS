using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class PageTemplateComponentGqlResolver : IPageTemplateComponentGqlResolver
{
    IPageTemplateComponentService _PageTemplateComponentService;
    public PageTemplateComponentGqlResolver(IPageTemplateComponentService PageTemplateComponentService)
    {
        _PageTemplateComponentService = PageTemplateComponentService;
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
    [GraphQLName("pageTemplateComponentView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PageTemplateComponentCrud>> pageTemplateComponentView()
    {
        var query = await _PageTemplateComponentService
            .GetAllViewQueryable<SqlPageTemplateComponentView, SqlPageTemplateComponentView, PageTemplateComponentCrud>();
        return query.Select(x => new PageTemplateComponentCrud
        {
            Id = x.Id,
            ComponentTypeId = x.ComponentTypeId,
            ComponentTypeName = x.ComponentTypeName??"",
            ComponentVariant = x.ComponentVariant??"",
            PageTemplateId = x.PageTemplateId,
            PageTemplateName = x.PageTemplateName ?? "",
            IsEditable = x.IsEditable,
            SortOrder=x.SortOrder,
            CreatedAtPersian = x.CreatedAtPersian??"",
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
