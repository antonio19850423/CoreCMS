using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class SiteSettingGqlResolver : ISiteSettingGqlResolver
{
    ISiteSettingService _SiteSettingService;
    public SiteSettingGqlResolver(ISiteSettingService SiteSettingService)
    {
        _SiteSettingService = SiteSettingService;
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
    [GraphQLName("siteSettingView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SiteSettingCrud>> siteSettingView()
    {
        var query = await _SiteSettingService
            .GetAllViewQueryable<SqlSiteSettingView, SqlSiteSettingView, SiteSettingCrud>();
        return query.Select(x => new SiteSettingCrud
        {
            Id = x.Id,
            Phone2 = x.Phone??"",
            Address2 = x.Address??"",
            Address= x.Address ?? "",
            Address2Title = x.Address2Title ?? "",
            AddressTitle = x.AddressTitle ?? "",
            Phone = x.Phone ?? "",
            MobileTitle = x.MobileTitle ?? "",
            DarkLogoAlt = x.DarkLogoAlt ?? "",
            DarkLogoUrl = x.DarkLogoUrl ?? "",
            DefaultMetaDescription = x.DefaultMetaDescription ?? "",
            DefaultMetaKeywords = x.DefaultMetaKeywords ?? "",
            DefaultMetaTitle = x.DefaultMetaTitle ?? "",
            DomainName = x.DomainName ?? "",
            Email = x.Email ?? "",
            FaviconUrl = x.FaviconUrl ?? "",
            Fax= x.Fax ?? "", 
            FaxTitle= x.FaxTitle ?? "",
            IsActive = x.IsActive,
            LogoAlt = x.LogoAlt ?? "",
            LogoUrl = x.LogoUrl ?? "",
            Mobile= x.Mobile ?? "",
            Phone2Title= x.Phone2Title ?? "",
            PhoneTitle= x.PhoneTitle ?? "",
            SiteName= x.SiteName ?? "",
            CreatedAtPersian = x.CreatedAtPersian ?? "",
            UpdatedAtPersian = x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
