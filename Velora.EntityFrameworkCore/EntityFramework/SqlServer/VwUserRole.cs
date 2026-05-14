using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwUserRole
{
    public Guid UserId { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public bool UserIsActive { get; set; }

    public Guid RoleId { get; set; }

    [StringLength(100)]
    public string RoleName { get; set; } = null!;

    [StringLength(50)]
    public string RoleCode { get; set; } = null!;

    [StringLength(500)]
    public string? RoleDescription { get; set; }
}
