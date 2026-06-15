using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSectionGroupItemForm
{
    public Guid Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    public Guid? GroupId { get; set; }

    [StringLength(50)]
    public string? GroupName { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
