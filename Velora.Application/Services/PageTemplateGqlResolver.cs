using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class PageTemplateGqlResolver : IPageTemplateGqlResolver
{
    IPageTemplateService _PageTemplateService;
    public PageTemplateGqlResolver(IPageTemplateService PageTemplateService)
    {
        _PageTemplateService = PageTemplateService;
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
    [GraphQLName("pageTemplateView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PageTemplateCrud>> pageTemplateView()
    {
        var query = await _PageTemplateService
            .GetAllViewQueryable<SqlPageTemplateView, SqlPageTemplateView, PageTemplateCrud>();
        return query.Select(x => new PageTemplateCrud
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            CreatedAtPersian = x.CreatedAtPersian??"",
            Description = x.Description,
            IsActive = x.IsActive,
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
