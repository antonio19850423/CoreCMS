using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ContentCategory", Schema = "cms")]
public partial class ContentCategory
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    public string? Slug { get; set; }

    [StringLength(300)]
    public string? Description { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    public Guid? ParentId { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
}
