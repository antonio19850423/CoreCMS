using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductVariantAttribute", Schema = "cms")]
[Index("ProductAttributeId", Name = "IX_ProductVariantAttribute_ProductAttribute")]
[Index("ProductVariantId", Name = "IX_ProductVariantAttribute_ProductVariant")]
public partial class ProductVariantAttribute
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductVariantId { get; set; }

    public Guid ProductAttributeId { get; set; }

    [StringLength(300)]
    public string Value { get; set; } = null!;

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("ProductAttributeId")]
    [InverseProperty("ProductVariantAttributes")]
    public virtual ProductAttribute ProductAttribute { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("ProductVariantAttributes")]
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
