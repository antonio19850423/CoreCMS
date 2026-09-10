using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class PaymentGqlResolver : IPaymentGqlResolver
{
    IPaymentService _PaymentService;
    public PaymentGqlResolver(IPaymentService PaymentService)
    {
        _PaymentService = PaymentService;
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
    [GraphQLName("paymentView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PaymentCrud>> paymentView()
    {
        var query = await _PaymentService
            .GetAllViewQueryable<SqlPaymentView, SqlPaymentView, PaymentCrud>();
        return query.Select(x => new PaymentCrud
        {
            Id = x.Id,
            AddressText = x.AddressText??"",
            BankAccountId = x.BankAccountId,
            CouponCode = x.CouponCode??"",
            CouponDiscountAmount = x.CouponDiscountAmount,
            CouponId = x.CouponId,
            CreatedAt = x.CreatedAt,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName = x.CreatedByName??"",
            Description = x.Description??"",
            FinalAmount = x.FinalAmount,
            GatewayId = x.GatewayId,
            GatewayTrackingCode = x.GatewayTrackingCode ?? "",
            GatewayTransactionId= x.GatewayTransactionId ?? "",
            OrderCode= x.OrderCode ?? "",
            OrderedAt= x.OrderedAt,
            CreatedBy= x.CreatedBy,
            PaidAt= x.PaidAt,
            PaidAtPersian=x.PaidAtPersian??"",
            PaymentAmount= x.PaymentAmount,
            PaymentMethod= x.PaymentMethod,
            PaymentMethodTitle= x.PaymentMethodTitle ??"",
            PaymentStatus= x.PaymentStatus,
            PaymentStatusTitle= x.PaymentStatusTitle??"",
            ReceiptFile= x.ReceiptFile ??"",
            ReceiverFirstName= x.ReceiverFirstName ??"",
            ReceiverLastName= x.ReceiverLastName ??"",
            ReceiverNationalCode= x.ReceiverNationalCode ??"",
            ReceiverPhone= x.ReceiverPhone ??"",
            ShippingMethodName= x.ShippingMethodName ??"",
            ShippingPrice= x.ShippingPrice,
            ShoppingCartId= x.ShoppingCartId ,
            ShoppingCartStatusTitle= x.ShoppingCartStatusTitle ??"",
            ShouldInsert= x.ShouldInsert,
            Status= x.Status,
            UpdatedAt= x.UpdatedAt ,
            UpdatedBy= x.UpdatedBy ,
            UserFullName= x.UserFullName ??"",
            UserId = x.UserId
            
        });
    }

}
