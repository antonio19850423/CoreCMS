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

    public string? Features { get; set; }

    [StringLength(350)]
    public string? CopyrightText { get; set; }

    [StringLength(150)]
    public string? ContactFirstNameLabel { get; set; }

    [StringLength(150)]
    public string? ContactLastNameLabel { get; set; }

    [StringLength(150)]
    public string? ContactEmailLabel { get; set; }

    [StringLength(150)]
    public string? ContactMessageLabel { get; set; }

    [StringLength(150)]
    public string? ContactSubmitButtonText { get; set; }

    [StringLength(512)]
    public string? ImageUrl2 { get; set; }

    [StringLength(150)]
    public string? ImageAlt2 { get; set; }

    [StringLength(512)]
    public string? ImageUrl3 { get; set; }

    [StringLength(150)]
    public string? ImageAlt3 { get; set; }

    [StringLength(512)]
    public string? ImageUrl4 { get; set; }

    [StringLength(150)]
    public string? ImageAlt4 { get; set; }

    [StringLength(500)]
    public string? MapEmbedUrl { get; set; }

    public Guid? Link1TargetId { get; set; }

    public Guid? Link1TypeId { get; set; }

    public bool? Link1OpenInNewTab { get; set; }

    public Guid? Link2TargetId { get; set; }

    public Guid? Link2TypeId { get; set; }

    public bool? Link2OpenInNewTab { get; set; }

    public Guid? Link3TargetId { get; set; }

    public Guid? Link3TypeId { get; set; }

    public bool? Link3OpenInNewTab { get; set; }

    public Guid? Link4TargetId { get; set; }

    public Guid? Link4TypeId { get; set; }

    public bool? Link4OpenInNewTab { get; set; }

    [StringLength(500)]
    public string? VideoUrl { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("Sections")]
    public virtual ComponentType ComponentType { get; set; } = null!;

    [ForeignKey("Link1TargetId")]
    [InverseProperty("SectionLink1Targets")]
    public virtual Page? Link1Target { get; set; }

    [ForeignKey("Link1TypeId")]
    [InverseProperty("SectionLink1Types")]
    public virtual LinkType? Link1Type { get; set; }

    [ForeignKey("Link2TargetId")]
    [InverseProperty("SectionLink2Targets")]
    public virtual Page? Link2Target { get; set; }

    [ForeignKey("Link2TypeId")]
    [InverseProperty("SectionLink2Types")]
    public virtual LinkType? Link2Type { get; set; }

    [ForeignKey("Link3TargetId")]
    [InverseProperty("SectionLink3Targets")]
    public virtual Page? Link3Target { get; set; }

    [ForeignKey("Link3TypeId")]
    [InverseProperty("SectionLink3Types")]
    public virtual LinkType? Link3Type { get; set; }

    [ForeignKey("Link4TargetId")]
    [InverseProperty("SectionLink4Targets")]
    public virtual Page? Link4Target { get; set; }

    [ForeignKey("Link4TypeId")]
    [InverseProperty("SectionLink4Types")]
    public virtual LinkType? Link4Type { get; set; }

    [ForeignKey("PageId")]
    [InverseProperty("SectionPages")]
    public virtual Page Page { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();
}
