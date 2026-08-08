using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwCouponForm
{
    public Guid Id { get; set; }

    public byte CouponType { get; set; }

    [StringLength(9)]
    public string CouponTypeName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CouponValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MinimumOrderAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MaximumDiscountAmount { get; set; }

    [StringLength(19)]
    public string? StartDatePersian { get; set; }

    [StringLength(19)]
    public string? EndDatePersian { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [StringLength(100)]
    public string Code { get; set; } = null!;

    public bool? CanCombineWithDiscount { get; set; }

    public bool IsSingleUsePerUser { get; set; }

    public int? UsageLimit { get; set; }

    public int UsedCount { get; set; }

    public bool IsActive { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
