using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwDiscountForm
{
    public Guid Id { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public byte DiscountType { get; set; }

    [StringLength(9)]
    public string DiscountTypeName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [StringLength(19)]
    public string? StartDatePersian { get; set; }

    [StringLength(19)]
    public string? EndDatePersian { get; set; }

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
