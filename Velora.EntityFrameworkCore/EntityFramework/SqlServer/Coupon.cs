using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("Coupon", Schema = "cms")]
public partial class Coupon
{
    [Key]
    public Guid Id { get; set; }

    public Guid DiscountId { get; set; }

    [StringLength(100)]
    public string Code { get; set; } = null!;

    public int? UsageLimit { get; set; }

    public int UsedCount { get; set; }

    public bool IsSingleUsePerUser { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("Coupon")]
    public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

    [ForeignKey("DiscountId")]
    [InverseProperty("Coupons")]
    public virtual Discount Discount { get; set; } = null!;
}
