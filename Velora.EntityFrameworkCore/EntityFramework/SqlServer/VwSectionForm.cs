using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSectionForm
{
    public Guid Id { get; set; }

    [StringLength(50)]
    public string? Link1Color { get; set; }

    [StringLength(100)]
    public string? Link1Text { get; set; }

    [StringLength(300)]
    public string? Link1Url { get; set; }

    [StringLength(50)]
    public string? Link2Color { get; set; }

    [StringLength(100)]
    public string? Link2Text { get; set; }

    [StringLength(300)]
    public string? Link2Url { get; set; }

    [StringLength(50)]
    public string? Link3Color { get; set; }

    [StringLength(100)]
    public string? Link3Text { get; set; }

    [StringLength(300)]
    public string? Link3Url { get; set; }

    [StringLength(50)]
    public string? Link4Color { get; set; }

    [StringLength(100)]
    public string? Link4Text { get; set; }

    [StringLength(300)]
    public string? Link4Url { get; set; }

    [StringLength(50)]
    public string? BackgroundColor { get; set; }

    [StringLength(50)]
    public string? DescriptionColor { get; set; }

    [StringLength(50)]
    public string? HeaderColor { get; set; }

    [StringLength(150)]
    public string? IconAlt { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    [StringLength(512)]
    public string? ImageUrl { get; set; }

    [StringLength(150)]
    public string? ImageAlt { get; set; }

    [StringLength(50)]
    public string? SubtitleColor { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    public int? ColumnsCount { get; set; }

    public Guid ComponentTypeId { get; set; }

    [StringLength(100)]
    public string? ComponentTypeName { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public Guid PageId { get; set; }

    public int SortOrder { get; set; }

    public Guid ParentId { get; set; }

    [StringLength(500)]
    public string? Subtitle { get; set; }

    public string? Features { get; set; }

    [StringLength(350)]
    public string? CopyrightText { get; set; }

    [StringLength(150)]
    public string? ContactEmailLabel { get; set; }

    [StringLength(150)]
    public string? ContactFirstNameLabel { get; set; }

    [StringLength(150)]
    public string? ContactLastNameLabel { get; set; }

    [StringLength(150)]
    public string? ContactMessageLabel { get; set; }

    [StringLength(500)]
    public string? MapEmbedUrl { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string? Link1TargetId { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string? Link1TypeId { get; set; }

    public bool? Link1OpenInNewTab { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
