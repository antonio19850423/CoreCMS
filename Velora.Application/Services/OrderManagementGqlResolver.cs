using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class OrderManagementGqlResolver : IOrderManagementGqlResolver
{
    IOrderManagementService _OrderManagementService;
    public OrderManagementGqlResolver(IOrderManagementService OrderManagementService)
    {
        _OrderManagementService = OrderManagementService;
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
    [GraphQLName("orderManagementView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<OrderManagementCrud>> orderManagementView()
    {
        var query = await _OrderManagementService
            .GetAllViewQueryable<SqlOrderManagement, SqlOrderManagement, OrderManagementCrud>();
        return query.Select(x => new OrderManagementCrud
        {
            Id = x.Id,
            ParentId= x.ParentId,
            AddressText = x.AddressText??"",
            BuyerName = x.BuyerName??"",
            CouponCode=x.CouponCode??"",
            CouponDiscountAmount=x.CouponDiscountAmount,
            CouponId=x.CouponId,
            DutyAmount=x.DutyAmount,
            FinalAmount=x.FinalAmount,
            GatewayTrackingCode=x.GatewayTrackingCode??"",
            HasPayment= x.HasPayment,
            LastPaymentStatusChangeAt=x.LastPaymentStatusChangeAt,
            LastPaymentStatusChangeAtPersian= x.LastPaymentStatusChangeAtPersian??"",
            LastPaymentStatusChangedBy= x.LastPaymentStatusChangedBy??"",
            LastPaymentStatusChangedById= x.LastPaymentStatusChangedById,
            LastPaymentStatusNewStatus= x.LastPaymentStatusNewStatus,
            LastPaymentStatusDescription= x.LastPaymentStatusDescription??"",
            LastPaymentStatusOldStatus= x.LastPaymentStatusOldStatus,
            OrderCode=x.OrderCode??"",
            OrderedAt=x.OrderedAt,
            OrderedAtPersian= x.OrderedAtPersian??"",
            OrderStatus=x.OrderStatus,
            OrderStatusTitle= x.OrderStatusTitle??"",
            PaidAt=x.PaidAt,
            PaidAtPersian=x.PaidAtPersian??"",
            PaymentAmount=x.PaymentAmount,
            PaymentId=x.PaymentId,
            PaymentStatus=x.PaymentStatus,
            PaymentMethod=x.PaymentMethod,
            PaymentMethodTitle= x.PaymentMethodTitle??"",
            PaymentStatusTitle= x.PaymentStatusTitle??"",
            ReceiverFirstName= x.ReceiverFirstName??"",
            ReceiverFullName= x.ReceiverFullName??"",
            ReceiverLastName= x.ReceiverLastName??"",
            ReceiverNationalCode= x.ReceiverNationalCode??"",
            ReceiverPhone= x.ReceiverPhone??"",
            ShippingMethodId   = x.ShippingMethodId,
            ShippingMethodName= x.ShippingMethodName??"",
            ShippingPrice= x.ShippingPrice,
            ShouldInsert= x.ShouldInsert,
            TaxAmount= x.TaxAmount,
            UserId = x.UserId
            
            
        });
    }

}
