using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductCategory", Schema = "cms")]
[Index("ParentId", Name = "IX_ProductCategory_ParentId")]
[Index("SortOrder", Name = "IX_ProductCategory_SortOrder")]
[Index("Slug", Name = "UQ_ProductCategory_Slug", IsUnique = true)]
public partial class ProductCategory
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Slug { get; set; }

    [StringLength(300)]
    public string? Description { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    public Guid? ParentId { get; set; }

    public int SortOrder { get; set; }

    [StringLength(200)]
    public string? SeoTitle { get; set; }

    [StringLength(500)]
    public string? SeoDescription { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<CategoryAttribute> CategoryAttributes { get; set; } = new List<CategoryAttribute>();

    [InverseProperty("Parent")]
    public virtual ICollection<ProductCategory> InverseParent { get; set; } = new List<ProductCategory>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual ProductCategory? Parent { get; set; }
}
