using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwInventoryManagement
{
    public Guid Id { get; set; }

    public Guid? VariantId { get; set; }

    [StringLength(200)]
    public string ProductName { get; set; } = null!;

    [StringLength(150)]
    public string? VariantName { get; set; }

    [StringLength(100)]
    public string? Sku { get; set; }

    [StringLength(100)]
    public string? Barcode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int InventoryInQuantity { get; set; }

    public int ReservedQuantity { get; set; }

    public int PaidQuantity { get; set; }

    public int? CurrentQuantity { get; set; }

    public int? LowStockThreshold { get; set; }

    public int? LowStockQuantity { get; set; }

    public bool? IsOutOfStock { get; set; }

    public bool? IsLowStock { get; set; }

    public bool? IsInStock { get; set; }

    public int InventoryStatusPriority { get; set; }

    [StringLength(11)]
    public string InventoryStatusTitle { get; set; } = null!;

    [Column(TypeName = "decimal(29, 2)")]
    public decimal? InventoryValue { get; set; }

    public bool? ProductIsActive { get; set; }

    public bool? VariantIsActive { get; set; }
}
