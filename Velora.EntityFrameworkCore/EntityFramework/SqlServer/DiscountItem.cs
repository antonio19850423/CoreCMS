using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("DiscountItem", Schema = "cms")]
public partial class DiscountItem
{
    [Key]
    public Guid Id { get; set; }

    public Guid DiscountId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? ProductVariantId { get; set; }

    public Guid? ProductCategoryId { get; set; }

    public Guid? ProductBrandId { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("DiscountId")]
    [InverseProperty("DiscountItems")]
    public virtual Discount Discount { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("DiscountItems")]
    public virtual Product? Product { get; set; }

    [ForeignKey("ProductBrandId")]
    [InverseProperty("DiscountItems")]
    public virtual ProductBrand? ProductBrand { get; set; }

    [ForeignKey("ProductCategoryId")]
    [InverseProperty("DiscountItems")]
    public virtual ProductCategory? ProductCategory { get; set; }

    [ForeignKey("ProductVariantId")]
    [InverseProperty("DiscountItems")]
    public virtual ProductVariant? ProductVariant { get; set; }
}
