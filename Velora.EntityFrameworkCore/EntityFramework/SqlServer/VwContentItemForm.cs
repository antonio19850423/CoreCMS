using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwContentItemForm
{
    public Guid Id { get; set; }

    [StringLength(250)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Summary { get; set; }

    public string? Content { get; set; }

    [StringLength(50)]
    public string ContentType { get; set; } = null!;

    [StringLength(300)]
    public string? AuthorAvatarUrl { get; set; }

    [StringLength(150)]
    public string? AuthorName { get; set; }

    [StringLength(150)]
    public string? AuthorTitle { get; set; }

    [StringLength(500)]
    public string? ExternalUrl { get; set; }

    [StringLength(512)]
    public string? ImageUrl { get; set; }

    [StringLength(250)]
    public string? ImageAlt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public Guid? CategoryId { get; set; }

    [StringLength(150)]
    public string? CategoryName { get; set; }

    [StringLength(200)]
    public string? CategorySlug { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public Guid? PageId { get; set; }

    public Guid? ParentId { get; set; }

    [StringLength(200)]
    public string? Slug { get; set; }

    [StringLength(150)]
    public string? SourceTitle { get; set; }

    [StringLength(500)]
    public string? SourceUrl { get; set; }

    [StringLength(512)]
    public string? ImageDetailUrl { get; set; }

    [StringLength(250)]
    public string? ImageDetailAlt { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }

    [StringLength(4000)]
    public string? TagIds { get; set; }

    [StringLength(4000)]
    public string? TagNames { get; set; }
}
