using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductForm
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? Barcode { get; set; }

    public Guid? BrandId { get; set; }

    [StringLength(150)]
    public string? BrandName { get; set; }

    public Guid CategoryId { get; set; }

    [StringLength(150)]
    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsFeatured { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public bool IsPublished { get; set; }

    public Guid ProductTypeId { get; set; }

    [StringLength(100)]
    public string? ProductTypeName { get; set; }

    public int SaleCount { get; set; }

    [StringLength(500)]
    public string? SeoDescription { get; set; }

    [StringLength(200)]
    public string? SeoTitle { get; set; }

    [StringLength(100)]
    public string? Sku { get; set; }

    [StringLength(250)]
    public string Slug { get; set; } = null!;

    public int SortOrder { get; set; }

    [StringLength(500)]
    public string? Summary { get; set; }

    [StringLength(300)]
    public string? Thumbnail { get; set; }

    public int ViewCount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Weight { get; set; }

    [StringLength(300)]
    public string? MainImage { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
