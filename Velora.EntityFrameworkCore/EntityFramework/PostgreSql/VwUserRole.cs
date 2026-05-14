using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VwUserRole
{
    public Guid? Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? UserIsActive { get; set; }

    public Guid? RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleCode { get; set; }

    public string? RoleDescription { get; set; }
}
