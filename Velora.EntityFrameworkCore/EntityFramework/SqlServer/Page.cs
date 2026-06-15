using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("Pages", Schema = "cms")]
[Index("Slug", Name = "UQ__Pages__BC7B5FB63220DBFD", IsUnique = true)]
public partial class Page
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string Slug { get; set; } = null!;

    public Guid? PageTemplateId { get; set; }

    public bool IsHome { get; set; }

    public bool IsPublished { get; set; }

    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    [StringLength(300)]
    public string? CanonicalUrl { get; set; }

    [StringLength(300)]
    public string? OgImageUrl { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("Page")]
    public virtual ICollection<ContentItem> ContentItems { get; set; } = new List<ContentItem>();

    [ForeignKey("PageTemplateId")]
    [InverseProperty("Pages")]
    public virtual PageTemplate? PageTemplate { get; set; }

    [InverseProperty("Page")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
