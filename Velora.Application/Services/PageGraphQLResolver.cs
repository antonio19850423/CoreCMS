using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class PageGqlResolver : IPageGqlResolver
{
    IPageService _PageService;
    public PageGqlResolver(IPageService PageService)
    {
        _PageService = PageService;
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
    [GraphQLName("pageView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PageCrud>> pageView()
    {
        var query = await _PageService
            .GetAllViewQueryable<SqlPageView, SqlPageView, PageCrud>();
        return query.Select(x => new PageCrud
        {
            Id = x.Id,
            CanonicalUrl = x.CanonicalUrl??"",
            IsHome = x.IsHome,
            IsPublished = x.IsPublished,
            MetaDescription = x.MetaDescription??"",
            MetaKeywords = x.MetaKeywords ?? "",
            MetaTitle = x.MetaTitle ?? "",
            Name = x.Name ?? "",
            OgImageUrl=x.OgImageUrl ?? "",
            PageTemplateId = x.PageTemplateId,
            PageTemplateName = x.PageTemplateName ?? "",
            Slug=x.Slug ?? "",
            CreatedAtPersian = x.CreatedAtPersian??"",
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            IsActive=x.IsActive,
            IsDynamic=x.IsDynamic
        });
    }

}
