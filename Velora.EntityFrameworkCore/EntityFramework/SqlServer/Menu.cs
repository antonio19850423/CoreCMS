using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("Menus", Schema = "cms")]
public partial class Menu
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public int SortOrder { get; set; }

    [StringLength(200)]
    public string Link1Text { get; set; } = null!;

    [StringLength(500)]
    public string? Link1Url { get; set; }

    public Guid? Link1TargetId { get; set; }

    public Guid? Link1TypeId { get; set; }

    [StringLength(50)]
    public string? Link1Color { get; set; }

    public bool? Link1OpenInNewTab { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<Menu> InverseParent { get; set; } = new List<Menu>();

    [ForeignKey("Link1TargetId")]
    [InverseProperty("Menus")]
    public virtual Page? Link1Target { get; set; }

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Menu? Parent { get; set; }
}
