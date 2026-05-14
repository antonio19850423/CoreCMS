using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwPermissionForm
{
    public Guid Id { get; set; }

    public Guid ResourceId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ResourceCode { get; set; } = null!;

    [StringLength(250)]
    public string ResourceName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ResourceTypeCode { get; set; } = null!;

    public int Actions { get; set; }

    [StringLength(4000)]
    public string? RoleIds { get; set; }

    [StringLength(4000)]
    public string? RoleNames { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
