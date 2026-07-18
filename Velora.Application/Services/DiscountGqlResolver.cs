using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class DiscountGqlResolver : IDiscountGqlResolver
{
    IDiscountService _DiscountService;
    public DiscountGqlResolver(IDiscountService DiscountService)
    {
        _DiscountService = DiscountService;
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
    [GraphQLName("DiscountView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<DiscountCrud>> discountView()
    {
        var query = await _DiscountService
            .GetAllViewQueryable<SqlDiscountView, SqlDiscountView, DiscountCrud>();
        return query.Select(x => new DiscountCrud
        {
            Id = x.Id,
            IsActive = x.IsActive,
            StartDate = x.StartDate,
            Name = x.Name??"",
            DiscountType = x.DiscountType,
            DiscountTypeName = x.DiscountTypeName??"",
            DiscountValue = x.DiscountValue,
            EndDate = x.EndDate,
            EndDatePersian = x.EndDatePersian??"",
            ShouldInsert = x.ShouldInsert,
            StartDatePersian = x.StartDatePersian??"",
            CreatedAtPersian= x.CreatedAtPersian??"",
            CreatedByName= x.CreatedByName ?? "",
            UpdatedAtPersian=x.UpdatedAtPersian??"",
            UpdatedByName= x.UpdatedByName ?? ""
        });
    }

}
