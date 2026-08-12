using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ShoppingCarts", Schema = "cms")]
public partial class ShoppingCart
{
    [Key]
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    [StringLength(200)]
    public string CartToken { get; set; } = null!;

    public Guid? TenantId { get; set; }

    public int Status { get; set; }

    public DateTime? ExpireAt { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    [StringLength(50)]
    public string? OrderCode { get; set; }

    [StringLength(100)]
    public string? ReceiverFirstName { get; set; }

    [StringLength(100)]
    public string? ReceiverLastName { get; set; }

    [StringLength(20)]
    public string? ReceiverNationalCode { get; set; }

    [StringLength(20)]
    public string? ReceiverPhone { get; set; }

    public Guid? AddressId { get; set; }

    public Guid? ShippingMethodId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ShippingPrice { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? CouponCode { get; set; }

    public Guid? CouponId { get; set; }

    public int? PaymentMethod { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    public DateTime? OrderedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    [InverseProperty("ShoppingCart")]
    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
}
