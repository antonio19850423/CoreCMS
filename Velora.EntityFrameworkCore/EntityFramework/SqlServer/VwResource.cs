using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwResource
{
    public Guid ResourceId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ResourceCode { get; set; } = null!;

    [StringLength(250)]
    public string DefaultDisplayName { get; set; } = null!;

    [StringLength(10)]
    public string? LanguageCode { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public int Order { get; set; }

    public int? GridOrder { get; set; }

    public int? FormOrder { get; set; }

    public Guid? ParentId { get; set; }

    public int? PermissionActions { get; set; }

    public Guid? RoleId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public Guid ResourceTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ResourceTypeCode { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? FieldType { get; set; }

    public int? MaxLength { get; set; }

    public bool IsRequired { get; set; }

    public bool ShowInForm { get; set; }

    public bool ShowInGrid { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? InputMask { get; set; }

    [StringLength(255)]
    public string? Route { get; set; }

    public bool? IsDynamicForm { get; set; }

    [StringLength(200)]
    public string? LinkedFieldCode { get; set; }

    public bool? ShowInSelectBox { get; set; }

    public int? SelectBoxOrder { get; set; }

    [StringLength(100)]
    public string? ServiceName { get; set; }

    [StringLength(100)]
    public string? EntityName { get; set; }

    [StringLength(300)]
    public string? SelectDisplayFields { get; set; }

    [StringLength(200)]
    public string? GroupKey { get; set; }

    public bool? ShowInTreeView { get; set; }
}
