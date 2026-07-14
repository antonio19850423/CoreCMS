using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductDiscount", Schema = "cms")]
[Index("StartDate", "EndDate", Name = "IX_ProductDiscount_Date")]
[Index("ProductId", Name = "IX_ProductDiscount_Product")]
[Index("ProductVariantId", Name = "IX_ProductDiscount_ProductVariant")]
public partial class ProductDiscount
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? ProductVariantId { get; set; }

    public byte DiscountType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MaximumDiscountAmount { get; set; }

    public int Priority { get; set; }

    public int? UsageLimit { get; set; }

    public int UsedCount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductDiscounts")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("ProductDiscounts")]
    public virtual ProductVariant? ProductVariant { get; set; }
}
