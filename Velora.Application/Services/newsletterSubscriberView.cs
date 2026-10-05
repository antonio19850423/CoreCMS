using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class NewsletterSubscriberGqlResolver : INewsletterSubscriberGqlResolver
{
    INewsletterSubscriberService _NewsletterSubscriberService;
    public NewsletterSubscriberGqlResolver(INewsletterSubscriberService NewsletterSubscriberService)
    {
        _NewsletterSubscriberService = NewsletterSubscriberService;
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
    [GraphQLName("newsletterSubscriberView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<NewsletterSubscriberCrud>> newsletterSubscriberView()
    {
        var query = await _NewsletterSubscriberService
            .GetAllViewQueryable<SqlNewsletterSubscriberView, SqlNewsletterSubscriberView, NewsletterSubscriberCrud>();
        return query.Select(x => new NewsletterSubscriberCrud
        {
            Id = x.Id,
            IsActive = x.IsActive,
            IsConfirmed = x.IsConfirmed,
            Email = x.Email??"",
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            CreatedAtPersian = x.CreatedAtPersian,
            ShouldInsert = x.ShouldInsert,
            UpdatedAtPersian = x.UpdatedAtPersian   
        });
    }

}
