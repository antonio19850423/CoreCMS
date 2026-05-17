using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwPageTemplateComponentForm
{
    public Guid Id { get; set; }

    public Guid PageTemplateId { get; set; }

    public int SortOrder { get; set; }

    public Guid ComponentTypeId { get; set; }

    [StringLength(100)]
    public string ComponentVariant { get; set; } = null!;

    public bool IsEditable { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
