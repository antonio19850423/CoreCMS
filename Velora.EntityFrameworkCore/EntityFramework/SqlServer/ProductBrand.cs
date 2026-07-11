using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductBrand", Schema = "cms")]
[Index("SortOrder", Name = "IX_ProductBrand_SortOrder")]
[Index("Name", Name = "UQ_ProductBrand_Name", IsUnique = true)]
[Index("Slug", Name = "UQ_ProductBrand_Slug", IsUnique = true)]
public partial class ProductBrand
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string? Slug { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(300)]
    public string? Logo { get; set; }

    [StringLength(300)]
    public string? Website { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }
}
