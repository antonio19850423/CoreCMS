using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductSalesManagement
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? VariantId { get; set; }

    [StringLength(200)]
    public string? ProductName { get; set; }

    [StringLength(150)]
    public string? VariantName { get; set; }

    public int? OrderCount { get; set; }

    public int? SoldQuantity { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? GrossSalesAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? ProductDiscountAmount { get; set; }

    [Column(TypeName = "decimal(38, 6)")]
    public decimal? CouponDiscountAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? TotalDiscountAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? NetSalesAmount { get; set; }
}
