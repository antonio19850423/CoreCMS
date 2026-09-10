using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class PaymentStatusLogGqlResolver : IPaymentStatusLogGqlResolver
{
    IPaymentStatusLogService _PaymentStatusLogService;
    public PaymentStatusLogGqlResolver(IPaymentStatusLogService PaymentStatusLogService)
    {
        _PaymentStatusLogService = PaymentStatusLogService;
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
    [GraphQLName("paymentStatusLogView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PaymentStatusLogCrud>> paymentStatusLogView()
    {
        var query = await _PaymentStatusLogService
            .GetAllViewQueryable<SqlPaymentStatusLogView, SqlPaymentStatusLogView, PaymentStatusLogCrud>();
        return query.Select(x => new PaymentStatusLogCrud
        {
            Id = x.Id,
            ShouldInsert = x.ShouldInsert,
            CreatedAt = x.CreatedAt,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedBy = x.CreatedBy,
            CreatedByName = x.CreatedByName??"",
            Description = x.Description??"",
            NewStatus = x.NewStatus,
            NewStatusTitle= x.NewStatusTitle??"",
            OldStatus = x.OldStatus,
            OldStatusTitle= x.OldStatusTitle ?? "",
            ParentId = x.ParentId
            
        });
    }

}
