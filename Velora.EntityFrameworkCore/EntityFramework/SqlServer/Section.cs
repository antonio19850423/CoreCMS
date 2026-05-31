using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("Sections", Schema = "cms")]
public partial class Section
{
    [Key]
    public Guid Id { get; set; }

    public Guid PageId { get; set; }

    public Guid ComponentTypeId { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    [StringLength(512)]
    public string? ImageUrl { get; set; }

    public int? ColumnsCount { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [StringLength(50)]
    public string? BackgroundColor { get; set; }

    [StringLength(50)]
    public string? HeaderColor { get; set; }

    [StringLength(50)]
    public string? SubtitleColor { get; set; }

    [StringLength(50)]
    public string? DescriptionColor { get; set; }

    [StringLength(100)]
    public string? Link1Text { get; set; }

    [StringLength(300)]
    public string? Link1Url { get; set; }

    [StringLength(50)]
    public string? Link1Color { get; set; }

    [StringLength(100)]
    public string? Link2Text { get; set; }

    [StringLength(300)]
    public string? Link2Url { get; set; }

    [StringLength(50)]
    public string? Link2Color { get; set; }

    [StringLength(100)]
    public string? Link3Text { get; set; }

    [StringLength(300)]
    public string? Link3Url { get; set; }

    [StringLength(50)]
    public string? Link3Color { get; set; }

    [StringLength(100)]
    public string? Link4Text { get; set; }

    [StringLength(300)]
    public string? Link4Url { get; set; }

    [StringLength(50)]
    public string? Link4Color { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    [StringLength(150)]
    public string? IconAlt { get; set; }

    [StringLength(150)]
    public string? ImageAlt { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("Sections")]
    public virtual ComponentType ComponentType { get; set; } = null!;

    [ForeignKey("PageId")]
    [InverseProperty("Sections")]
    public virtual Page Page { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();
}
