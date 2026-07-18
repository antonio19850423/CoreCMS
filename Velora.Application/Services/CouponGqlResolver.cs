using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class CouponGqlResolver : ICouponGqlResolver
{
    ICouponService _CouponService;
    public CouponGqlResolver(ICouponService CouponService)
    {
        _CouponService = CouponService;
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
    [GraphQLName("CouponView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<CouponCrud>> couponView()
    {
        var query = await _CouponService
            .GetAllViewQueryable<VwCouponForm, VwCouponForm, CouponCrud>();
        return query.Select(x => new CouponCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            UsedCount = x.UsedCount,
            UsageLimit = x.UsageLimit,
            StartDate = x.StartDate,
            IsSingleUsePerUser = x.IsSingleUsePerUser,
            IsActive = x.IsActive,
            EndDate = x.EndDate,
            Code = x.Code??"",
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ?? "",
            EndDatePersian=x.EndDatePersian??"",
            ParentName = x.ParentName ??"",
            StartDatePersian= x.StartDatePersian??"",
            UpdatedAtPersian=x.UpdatedAtPersian??"",
            UpdatedByName=x.UpdatedByName?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
