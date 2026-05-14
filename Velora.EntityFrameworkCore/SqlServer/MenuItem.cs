using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("MenuItems", Schema = "site")]
public partial class MenuItem
{
    [Key]
    public int Id { get; set; }

    public int MenuId { get; set; }

    public int? ParentId { get; set; }

    [StringLength(150)]
    public string Title { get; set; } = null!;

    [StringLength(300)]
    public string Url { get; set; } = null!;

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    [StringLength(30)]
    public string? Target { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    [InverseProperty("Parent")]
    public virtual ICollection<MenuItem> InverseParent { get; set; } = new List<MenuItem>();

    [ForeignKey("MenuId")]
    [InverseProperty("MenuItems")]
    public virtual Menu Menu { get; set; } = null!;

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual MenuItem? Parent { get; set; }
}
