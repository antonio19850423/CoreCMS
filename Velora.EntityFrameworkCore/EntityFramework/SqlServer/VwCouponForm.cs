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

    public Guid ParentId { get; set; }

    [StringLength(200)]
    public string? ParentName { get; set; }

    [StringLength(100)]
    public string Code { get; set; } = null!;

    public DateTime StartDate { get; set; }

    [StringLength(19)]
    public string? StartDatePersian { get; set; }

    public DateTime EndDate { get; set; }

    [StringLength(19)]
    public string? EndDatePersian { get; set; }

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
