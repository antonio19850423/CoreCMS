using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("ComponentTypes", Schema = "cms")]
[Index("Code", Name = "UQ__Componen__A25C5AA7CBC3129B", IsUnique = true)]
public partial class ComponentType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Code { get; set; } = null!;

    [StringLength(30)]
    public string Type { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("ComponentType")]
    public virtual ICollection<ComponentFieldMap> ComponentFieldMaps { get; set; } = new List<ComponentFieldMap>();

    [InverseProperty("ComponentType")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();

    [InverseProperty("ComponentType")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
