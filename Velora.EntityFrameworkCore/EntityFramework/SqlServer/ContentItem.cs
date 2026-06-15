using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ContentItem", Schema = "cms")]
public partial class ContentItem
{
    [Key]
    public Guid Id { get; set; }

    public Guid? PageId { get; set; }

    public Guid? CategoryId { get; set; }

    [StringLength(50)]
    public string ContentType { get; set; } = null!;

    [StringLength(250)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Summary { get; set; }

    public string? Content { get; set; }

    [StringLength(512)]
    public string? ImageUrl { get; set; }

    [StringLength(250)]
    public string? ImageAlt { get; set; }

    [StringLength(150)]
    public string? AuthorName { get; set; }

    [StringLength(150)]
    public string? AuthorTitle { get; set; }

    [StringLength(300)]
    public string? AuthorAvatarUrl { get; set; }

    [StringLength(500)]
    public string? ExternalUrl { get; set; }

    public DateTime? PublishedAt { get; set; }

    public bool IsPublished { get; set; }

    [StringLength(500)]
    public string? Tags { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("ContentItems")]
    public virtual Category? Category { get; set; }

    [ForeignKey("PageId")]
    [InverseProperty("ContentItems")]
    public virtual Page? Page { get; set; }
}
