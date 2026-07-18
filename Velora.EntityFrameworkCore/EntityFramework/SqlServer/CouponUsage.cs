using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("CouponUsage", Schema = "cms")]
public partial class CouponUsage
{
    [Key]
    public Guid Id { get; set; }

    public Guid CouponId { get; set; }

    public Guid UserId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime UsedAt { get; set; }

    [ForeignKey("CouponId")]
    [InverseProperty("CouponUsages")]
    public virtual Coupon Coupon { get; set; } = null!;
}
