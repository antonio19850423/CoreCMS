using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class PaymentGatewayGqlResolver : IPaymentGatewayGqlResolver
{
    IPaymentGatewayService _PaymentGatewayService;
    public PaymentGatewayGqlResolver(IPaymentGatewayService PaymentGatewayService)
    {
        _PaymentGatewayService = PaymentGatewayService;
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
    [GraphQLName("paymentGatewayView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PaymentGatewayCrud>> paymentGatewayView()
    {
        var query = await _PaymentGatewayService
            .GetAllViewQueryable<SqlPaymentGatewayView, SqlPaymentGatewayView, PaymentGatewayCrud>();
        return query.Select(x => new PaymentGatewayCrud
        {
            Id = x.Id,
            SettingsJson = x.SettingsJson??"",
            ProviderType = x.ProviderType,
            Name = x.Name??"",
            LogoUrl = x.LogoUrl ?? "",
            IsDefault = x.IsDefault,
            IsActive = x.IsActive,
            CallbackUrl = x.CallbackUrl ??"",
            Description = x.Description ??"",
            DisplayOrder = x.DisplayOrder,
            GatewayCode = x.GatewayCode ??"",
            ProviderTypeTitle=x.ProviderTypeTitle ??"",
            ShouldInsert = x.ShouldInsert,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ??"",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",

        });
    }

}
