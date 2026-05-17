using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("SectionItems", Schema = "cms")]
public partial class SectionItem
{
    [Key]
    public Guid Id { get; set; }

    public Guid SectionId { get; set; }

    public Guid ComponentTypeId { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(300)]
    public string? Subtitle { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    [StringLength(300)]
    public string? AvatarUrl { get; set; }

    [StringLength(150)]
    public string? Role { get; set; }

    [StringLength(100)]
    public string? LinkText { get; set; }

    [StringLength(300)]
    public string? LinkUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("SectionItems")]
    public virtual ComponentType ComponentType { get; set; } = null!;

    [ForeignKey("SectionId")]
    [InverseProperty("SectionItems")]
    public virtual Section Section { get; set; } = null!;
}
