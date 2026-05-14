using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VwPermissionForm
{
    public Guid? Id { get; set; }

    public Guid? ResourceId { get; set; }

    public string? ResourceCode { get; set; }

    public string? ResourceName { get; set; }

    public string? ResourceTypeCode { get; set; }

    public int? Actions { get; set; }

    public string? RoleIds { get; set; }

    public string? RoleNames { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedByName { get; set; }

    public string? UpdatedByName { get; set; }
}
