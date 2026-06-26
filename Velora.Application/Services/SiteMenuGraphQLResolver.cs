using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class SiteMenuGqlResolver : ISiteMenuGqlResolver
{
    ISiteMenuService _SiteMenuService;
    public SiteMenuGqlResolver(ISiteMenuService SiteMenuService)
    {
        _SiteMenuService = SiteMenuService;
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
    [GraphQLName("siteMenuView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SiteMenuCrud>> siteMenuView()
    {
        var query = await _SiteMenuService
            .GetAllViewQueryable<VwSiteMenuForm, VwSiteMenuForm, SiteMenuCrud>();
        return query.Select(x => new SiteMenuCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            SortOrder = x.SortOrder,
            Icon=x.Icon??"",
            Link1Url = x.Link1Url??"",
            IconColor = x.IconColor??"",
            Link1Color = x.Link1Color??"",
            Link1OpenInNewTab=x.Link1OpenInNewTab,
            Link1TargetId=x.Link1TargetId,
            Link1Text=x.Link1Text??"",
            Link1TypeId=x.Link1TypeId,
            ParentName = x.ParentName??"",
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
