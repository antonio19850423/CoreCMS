using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwPaymentForm
{
    public Guid Id { get; set; }

    [StringLength(1000)]
    public string? AddressText { get; set; }

    [StringLength(100)]
    public string? CouponCode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CouponDiscountAmount { get; set; }

    public Guid? CouponId { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    [StringLength(50)]
    public string? OrderCode { get; set; }

    public DateTime? OrderedAt { get; set; }

    [StringLength(100)]
    public string? ReceiverFirstName { get; set; }

    [StringLength(100)]
    public string? ReceiverLastName { get; set; }

    [StringLength(20)]
    public string? ReceiverNationalCode { get; set; }

    [StringLength(20)]
    public string? ReceiverPhone { get; set; }

    [StringLength(200)]
    public string? ShippingMethodName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ShippingPrice { get; set; }

    [StringLength(201)]
    public string? UserFullName { get; set; }

    public int Status { get; set; }

    [StringLength(18)]
    public string ShoppingCartStatusTitle { get; set; } = null!;

    public Guid? UserId { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PaymentAmount { get; set; }

    public Guid? BankAccountId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? GatewayId { get; set; }

    [StringLength(200)]
    public string? GatewayTrackingCode { get; set; }

    [StringLength(200)]
    public string? GatewayTransactionId { get; set; }

    public DateTime? PaidAt { get; set; }

    [StringLength(19)]
    public string? PaidAtPersian { get; set; }

    [StringLength(13)]
    public string PaymentMethodTitle { get; set; } = null!;

    public int? PaymentMethod { get; set; }

    public int? PaymentStatus { get; set; }

    [StringLength(16)]
    public string PaymentStatusTitle { get; set; } = null!;

    [StringLength(500)]
    public string? ReceiptFile { get; set; }

    public Guid? ShoppingCartId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}
