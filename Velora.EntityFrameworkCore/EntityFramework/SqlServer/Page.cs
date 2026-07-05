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

    public bool? IsDynamic { get; set; }

    [InverseProperty("Page")]
    public virtual ICollection<ContentItem> ContentItems { get; set; } = new List<ContentItem>();

    [InverseProperty("Link1Target")]
    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    [ForeignKey("PageTemplateId")]
    [InverseProperty("Pages")]
    public virtual PageTemplate? PageTemplate { get; set; }

    [InverseProperty("Link1Target")]
    public virtual ICollection<SectionItem> SectionItemLink1Targets { get; set; } = new List<SectionItem>();

    [InverseProperty("Link2Target")]
    public virtual ICollection<SectionItem> SectionItemLink2Targets { get; set; } = new List<SectionItem>();

    [InverseProperty("Link3Target")]
    public virtual ICollection<SectionItem> SectionItemLink3Targets { get; set; } = new List<SectionItem>();

    [InverseProperty("Link4Target")]
    public virtual ICollection<SectionItem> SectionItemLink4Targets { get; set; } = new List<SectionItem>();

    [InverseProperty("Link1Target")]
    public virtual ICollection<Section> SectionLink1Targets { get; set; } = new List<Section>();

    [InverseProperty("Link2Target")]
    public virtual ICollection<Section> SectionLink2Targets { get; set; } = new List<Section>();

    [InverseProperty("Link3Target")]
    public virtual ICollection<Section> SectionLink3Targets { get; set; } = new List<Section>();

    [InverseProperty("Link4Target")]
    public virtual ICollection<Section> SectionLink4Targets { get; set; } = new List<Section>();

    [InverseProperty("Page")]
    public virtual ICollection<Section> SectionPages { get; set; } = new List<Section>();

    [InverseProperty("Link1Target")]
    public virtual ICollection<SiteMenu> SiteMenus { get; set; } = new List<SiteMenu>();
}
