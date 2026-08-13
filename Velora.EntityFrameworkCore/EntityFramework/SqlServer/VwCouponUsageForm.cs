using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwCouponUsageForm
{
    public Guid Id { get; set; }

    [StringLength(9)]
    public string CouponTypeName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CouponValue { get; set; }

    [StringLength(100)]
    public string? Code { get; set; }

    public Guid ParentId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime UsedAt { get; set; }

    public Guid UserId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CouponDiscountAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? FinalAmount { get; set; }

    [StringLength(19)]
    public string? UsedAtPersian { get; set; }

    [StringLength(201)]
    public string? UsedByName { get; set; }
}
