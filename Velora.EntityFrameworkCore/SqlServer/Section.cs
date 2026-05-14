using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("Sections", Schema = "cms")]
public partial class Section
{
    [Key]
    public int Id { get; set; }

    public int PageId { get; set; }

    public int ComponentTypeId { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    [StringLength(100)]
    public string? ButtonText { get; set; }

    [StringLength(300)]
    public string? ButtonUrl { get; set; }

    public int? ColumnsCount { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("Sections")]
    public virtual ComponentType ComponentType { get; set; } = null!;

    [ForeignKey("PageId")]
    [InverseProperty("Sections")]
    public virtual Page Page { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();
}
