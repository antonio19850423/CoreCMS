using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("SectionGroupItems", Schema = "cms")]
public partial class SectionGroupItem
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    public Guid? GroupId { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsTest { get; set; }

    [InverseProperty("SectionGroupItem")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();
}
