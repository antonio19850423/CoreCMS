using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwShoppingCartForm
{
    public Guid Id { get; set; }

    public Guid? AddressId { get; set; }

    [StringLength(200)]
    public string CartToken { get; set; } = null!;

    [StringLength(100)]
    public string? CouponCode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CouponDiscountAmount { get; set; }

    public Guid? CouponId { get; set; }

    public DateTime CreateAt { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public DateTime? ExpireAt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    [StringLength(50)]
    public string? OrderCode { get; set; }

    public DateTime? OrderedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    public int? PaymentMethod { get; set; }

    [StringLength(100)]
    public string? ReceiverFirstName { get; set; }

    [StringLength(100)]
    public string? ReceiverLastName { get; set; }

    [StringLength(20)]
    public string? ReceiverNationalCode { get; set; }

    [StringLength(20)]
    public string? ReceiverPhone { get; set; }

    public Guid? ShippingMethodId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ShippingPrice { get; set; }

    public int Status { get; set; }

    public DateTime? UpdateAt { get; set; }

    public Guid? UserId { get; set; }

    [StringLength(200)]
    public string? ShippingMethodName { get; set; }

    [StringLength(1000)]
    public string? AddressText { get; set; }

    [StringLength(19)]
    public string? PaidAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }
}
