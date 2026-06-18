using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwResourceForm
{
    public Guid Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(250)]
    public string DisplayName { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? EntityName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FieldType { get; set; }

    public int? FormOrder { get; set; }

    public int? GridOrder { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? InputMask { get; set; }

    public bool IsActive { get; set; }

    public bool? IsDynamicForm { get; set; }

    public bool IsRequired { get; set; }

    [StringLength(200)]
    public string? LinkedFieldCode { get; set; }

    public int? MaxLength { get; set; }

    public int Order { get; set; }

    public Guid? ParentId { get; set; }

    [StringLength(250)]
    public string? ParentDisplayName { get; set; }

    public Guid ResourceTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ResourceTypeTitle { get; set; }

    [StringLength(255)]
    public string? Route { get; set; }

    public int? SelectBoxOrder { get; set; }

    [StringLength(100)]
    public string? ServiceName { get; set; }

    public bool ShowInForm { get; set; }

    public bool ShowInGrid { get; set; }

    public bool? ShowInSelectBox { get; set; }

    [StringLength(200)]
    public string? GroupKey { get; set; }
}
