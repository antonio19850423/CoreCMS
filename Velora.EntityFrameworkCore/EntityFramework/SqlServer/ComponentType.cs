using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ComponentTypes", Schema = "cms")]
[Index("Code", Name = "UQ__Componen__A25C5AA7D24569F4", IsUnique = true)]
public partial class ComponentType
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Code { get; set; } = null!;

    [StringLength(30)]
    public string Type { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("ComponentType")]
    public virtual ICollection<PageTemplateComponent> PageTemplateComponents { get; set; } = new List<PageTemplateComponent>();

    [InverseProperty("ComponentType")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();

    [InverseProperty("ComponentType")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
