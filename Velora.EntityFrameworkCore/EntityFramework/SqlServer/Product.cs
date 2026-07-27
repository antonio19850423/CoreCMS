using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("Product", Schema = "cms")]
[Index("BrandId", Name = "IX_Product_Brand")]
[Index("CategoryId", Name = "IX_Product_Category")]
[Index("IsFeatured", Name = "IX_Product_IsFeatured")]
[Index("IsPublished", Name = "IX_Product_IsPublished")]
[Index("Name", Name = "IX_Product_Name")]
[Index("ProductTypeId", Name = "IX_Product_ProductType")]
[Index("Slug", Name = "UQ_Product_Slug", IsUnique = true)]
public partial class Product
{
    [Key]
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Guid? BrandId { get; set; }

    public Guid ProductTypeId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(250)]
    public string Slug { get; set; } = null!;

    [StringLength(100)]
    public string? Sku { get; set; }

    [StringLength(100)]
    public string? Barcode { get; set; }

    [StringLength(500)]
    public string? Summary { get; set; }

    public string? Description { get; set; }

    [StringLength(300)]
    public string? Thumbnail { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Weight { get; set; }

    [StringLength(200)]
    public string? SeoTitle { get; set; }

    [StringLength(500)]
    public string? SeoDescription { get; set; }

    public int? ViewCount { get; set; }

    public int? SaleCount { get; set; }

    public int SortOrder { get; set; }

    public bool? IsPublished { get; set; }

    public bool? IsFeatured { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [StringLength(300)]
    public string? MainImage { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Products")]
    public virtual ProductBrand? Brand { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual ProductCategory Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<DiscountItem> DiscountItems { get; set; } = new List<DiscountItem>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductAttributeValue> ProductAttributeValues { get; set; } = new List<ProductAttributeValue>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductFile> ProductFiles { get; set; } = new List<ProductFile>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductInventoryTransaction> ProductInventoryTransactions { get; set; } = new List<ProductInventoryTransaction>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductTagMapping> ProductTagMappings { get; set; } = new List<ProductTagMapping>();

    [ForeignKey("ProductTypeId")]
    [InverseProperty("Products")]
    public virtual ProductType ProductType { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    [InverseProperty("Product")]
    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
}
