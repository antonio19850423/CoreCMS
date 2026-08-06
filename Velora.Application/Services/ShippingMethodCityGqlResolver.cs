using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ShippingMethodCityGqlResolver : IShippingMethodCityGqlResolver
{
    IShippingMethodCityService _ShippingMethodCityService;
    public ShippingMethodCityGqlResolver(IShippingMethodCityService ShippingMethodCityService)
    {
        _ShippingMethodCityService = ShippingMethodCityService;
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
    [GraphQLName("shippingMethodCityView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ShippingMethodCityCrud>> shippingMethodCityView()
    {
        var query = await _ShippingMethodCityService
            .GetAllViewQueryable<SqlShippingMethodCityView, SqlShippingMethodCityView, ShippingMethodCityCrud>();
        return query.Select(x => new ShippingMethodCityCrud
        {
            Id = x.Id,
            CityId = x.CityId,
            ParentId = x.ParentId,
            CityTitle = x.CityTitle??"",
            IsActive = x.IsActive,  
            ShippingMethodTitle = x.ShippingMethodTitle??"",
            SortOrder=x.SortOrder,
            ShouldInsert = x.ShouldInsert,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ??"",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",

        });
    }

}
