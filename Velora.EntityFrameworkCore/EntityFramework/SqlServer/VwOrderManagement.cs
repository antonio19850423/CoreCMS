using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwOrderManagement
{
    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    [StringLength(50)]
    public string? OrderCode { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? OrderedAt { get; set; }

    [StringLength(19)]
    public string? OrderedAtPersian { get; set; }

    public int? OrderStatus { get; set; }

    [StringLength(13)]
    public string OrderStatusTitle { get; set; } = null!;

    [StringLength(201)]
    public string? BuyerName { get; set; }

    [StringLength(100)]
    public string? ReceiverFirstName { get; set; }

    [StringLength(100)]
    public string? ReceiverLastName { get; set; }

    [StringLength(201)]
    public string ReceiverFullName { get; set; } = null!;

    [StringLength(20)]
    public string? ReceiverNationalCode { get; set; }

    [StringLength(20)]
    public string? ReceiverPhone { get; set; }

    [StringLength(1000)]
    public string? AddressText { get; set; }

    public Guid? ShippingMethodId { get; set; }

    [StringLength(200)]
    public string? ShippingMethodName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ShippingPrice { get; set; }

    public Guid? CouponId { get; set; }

    [StringLength(100)]
    public string? CouponCode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CouponDiscountAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TaxAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DutyAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    public Guid? PaymentId { get; set; }

    public bool? HasPayment { get; set; }

    public int? PaymentMethod { get; set; }

    [StringLength(13)]
    public string PaymentMethodTitle { get; set; } = null!;

    public int? PaymentStatus { get; set; }

    [StringLength(16)]
    public string PaymentStatusTitle { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PaymentAmount { get; set; }

    [StringLength(200)]
    public string? GatewayTrackingCode { get; set; }

    public DateTime? PaidAt { get; set; }

    [StringLength(19)]
    public string? PaidAtPersian { get; set; }

    public int? LastPaymentStatusOldStatus { get; set; }

    public int? LastPaymentStatusNewStatus { get; set; }

    public DateTime? LastPaymentStatusChangeAt { get; set; }

    [StringLength(19)]
    public string? LastPaymentStatusChangeAtPersian { get; set; }

    public Guid? LastPaymentStatusChangedById { get; set; }

    [StringLength(201)]
    public string? LastPaymentStatusChangedBy { get; set; }

    [StringLength(1000)]
    public string? LastPaymentStatusDescription { get; set; }
}
