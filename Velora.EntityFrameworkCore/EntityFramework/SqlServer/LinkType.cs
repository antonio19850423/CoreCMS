using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("LinkType", Schema = "cms")]
public partial class LinkType
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Code { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsTest { get; set; }

    [InverseProperty("Link1Type")]
    public virtual ICollection<SectionItem> SectionItemLink1Types { get; set; } = new List<SectionItem>();

    [InverseProperty("Link2Type")]
    public virtual ICollection<SectionItem> SectionItemLink2Types { get; set; } = new List<SectionItem>();

    [InverseProperty("Link3Type")]
    public virtual ICollection<SectionItem> SectionItemLink3Types { get; set; } = new List<SectionItem>();

    [InverseProperty("Link4Type")]
    public virtual ICollection<SectionItem> SectionItemLink4Types { get; set; } = new List<SectionItem>();

    [InverseProperty("Link1Type")]
    public virtual ICollection<Section> SectionLink1Types { get; set; } = new List<Section>();

    [InverseProperty("Link2Type")]
    public virtual ICollection<Section> SectionLink2Types { get; set; } = new List<Section>();

    [InverseProperty("Link3Type")]
    public virtual ICollection<Section> SectionLink3Types { get; set; } = new List<Section>();

    [InverseProperty("Link4Type")]
    public virtual ICollection<Section> SectionLink4Types { get; set; } = new List<Section>();

    [InverseProperty("Link1Type")]
    public virtual ICollection<SiteMenu> SiteMenus { get; set; } = new List<SiteMenu>();
}
