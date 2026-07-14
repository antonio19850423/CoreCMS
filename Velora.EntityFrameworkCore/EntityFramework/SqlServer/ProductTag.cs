using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductTag", Schema = "cms")]
[Index("SortOrder", Name = "IX_ProductTag_SortOrder")]
[Index("Name", Name = "UQ_ProductTag_Name", IsUnique = true)]
[Index("Slug", Name = "UQ_ProductTag_Slug", IsUnique = true)]
public partial class ProductTag
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string? Slug { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(50)]
    public string? TextColor { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("ProductTag")]
    public virtual ICollection<ProductTagMapping> ProductTagMappings { get; set; } = new List<ProductTagMapping>();
}
