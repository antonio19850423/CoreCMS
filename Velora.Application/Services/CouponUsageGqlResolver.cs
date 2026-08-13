using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class CouponUsageGqlResolver : ICouponUsageGqlResolver
{
    ICouponUsageService _CouponUsageService;
    public CouponUsageGqlResolver(ICouponUsageService CouponUsageService)
    {
        _CouponUsageService = CouponUsageService;
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
    [GraphQLName("CouponUsageView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<CouponUsageCrud>> CouponUsageView()
    {
        var query = await _CouponUsageService
            .GetAllViewQueryable<SqlCouponUsageView, SqlCouponUsageView, CouponUsageCrud>();
        return query.Select(x => new CouponUsageCrud
        {
            Id = x.Id,
            UserId = x.UserId,
            UsedAt = x.UsedAt,
            OrderId = x.OrderId,
            ParentId = x.ParentId,
            Code = x.Code??"",
            CouponDiscountAmount=x.CouponDiscountAmount,
            CouponTypeName = x.CouponTypeName??"",
            CouponValue = x.CouponValue,
            FinalAmount = x.FinalAmount,
            ShouldInsert = x.ShouldInsert,
            UsedAtPersian = x.UsedAtPersian??"",
            UsedByName = x.UsedByName ?? "" 

        });
    }

}
