using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class CmsConfigurationGqlResolver : ICmsConfigurationGqlResolver
{
    ICmsConfigurationService _CmsConfigurationService;
    public CmsConfigurationGqlResolver(ICmsConfigurationService CmsConfigurationService)
    {
        _CmsConfigurationService = CmsConfigurationService;
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
    [GraphQLName("cmsConfigurationView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<CmsConfigurationCrud>> cmsConfigurationView()
    {
        var query = await _CmsConfigurationService
            .GetAllViewQueryable<SqlCmsConfigurationView, SqlCmsConfigurationView, CmsConfigurationCrud>();
        return query.Select(x => new CmsConfigurationCrud
        {
            Id = x.Id,
            EnableSeo = x.EnableSeo,
            EnableNews = x.EnableNews,
            EnableMultiLanguage = x.EnableMultiLanguage,
            EnableComments = x.EnableComments,
            EnableCache = x.EnableCache,
            SiteType = x.SiteType,
            IsActive = x.IsActive,
            EnableShop = x.EnableShop,
            EnableBlog = x.EnableBlog,
            DefaultTheme=x.DefaultTheme ?? "",
            CreatedAtPersian = x.CreatedAtPersian ?? "",
            UpdatedAtPersian = x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            EnablePrivacy = x.EnablePrivacy ,
            EnableFaq = x.EnableFaq ,
            EnableDynamicPages= x.EnableDynamicPages ,
            ShouldInsert = x.ShouldInsert
        });
    }

}
