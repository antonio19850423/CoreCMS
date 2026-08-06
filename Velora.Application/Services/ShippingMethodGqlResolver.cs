using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ShippingMethodGqlResolver : IShippingMethodGqlResolver
{
    IShippingMethodService _ShippingMethodService;
    public ShippingMethodGqlResolver(IShippingMethodService ShippingMethodService)
    {
        _ShippingMethodService = ShippingMethodService;
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
    [GraphQLName("shippingMethodView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ShippingMethodCrud>> shippingMethodView()
    {
        var query = await _ShippingMethodService
            .GetAllViewQueryable<SqlShippingMethodView, SqlShippingMethodView, ShippingMethodCrud>();
        return query.Select(x => new ShippingMethodCrud
        {
            Id = x.Id,
            Description = x.Description??"",
            SortOrder = x.SortOrder,
            Price = x.Price,
            Name = x.Name??"",
            IsNationwide = x.IsNationwide,
            IsDefault = x.IsDefault,
            EstimatedDays = x.EstimatedDays,
            IsActive=x.IsActive,
            ShouldInsert = x.ShouldInsert,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ??"",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",

        });
    }

}
