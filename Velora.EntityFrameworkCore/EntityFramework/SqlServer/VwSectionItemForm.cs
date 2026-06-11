using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSectionItemForm
{
    public Guid Id { get; set; }

    [StringLength(300)]
    public string? AvatarUrl { get; set; }

    [StringLength(50)]
    public string? BackgroundColor { get; set; }

    public string? Description { get; set; }

    [StringLength(50)]
    public string? DescriptionColor { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(150)]
    public string? IconAlt { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    [StringLength(250)]
    public string? ImageAlt { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

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

    [StringLength(150)]
    public string? Price { get; set; }

    public Guid ParentId { get; set; }

    public int SortOrder { get; set; }

    [StringLength(300)]
    public string? Subtitle { get; set; }

    [StringLength(50)]
    public string? SubtitleColor { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(50)]
    public string? TitleColor { get; set; }

    [StringLength(150)]
    public string? AvatarAlt { get; set; }

    public string? Features { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }

    [StringLength(100)]
    public string? ComponentTypeName { get; set; }
}
