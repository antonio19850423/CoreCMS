using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductAttributeForm
{
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Code { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

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
