using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductVariant", Schema = "cms")]
[Index("IsDefault", Name = "IX_ProductVariant_IsDefault")]
[Index("ProductId", Name = "IX_ProductVariant_ProductId")]
[Index("Sku", Name = "IX_ProductVariant_Sku")]
public partial class ProductVariant
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    [StringLength(100)]
    public string? Sku { get; set; }

    [StringLength(100)]
    public string? Barcode { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [StringLength(300)]
    public string? Image { get; set; }

    public int SortOrder { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ComparePrice { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductVariants")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("ProductVariant")]
    public virtual ICollection<ProductInventoryTransaction> ProductInventoryTransactions { get; set; } = new List<ProductInventoryTransaction>();
}
