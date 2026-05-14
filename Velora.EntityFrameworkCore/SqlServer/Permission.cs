using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("Permissions", Schema = "auth")]
public partial class Permission
{
    [Key]
    public Guid Id { get; set; }

    public Guid ResourceId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public int Actions { get; set; }

    public bool IsTest { get; set; }

    [ForeignKey("ResourceId")]
    [InverseProperty("Permissions")]
    public virtual Resource Resource { get; set; } = null!;

    [InverseProperty("Permission")]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
