using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VwResource
{
    public Guid? ResourceId { get; set; }

    public string? ResourceCode { get; set; }

    public string? DefaultDisplayName { get; set; }

    public string? InputMask { get; set; }

    public string? LanguageCode { get; set; }

    public string? Name { get; set; }

    public int? Order { get; set; }

    public int? GridOrder { get; set; }

    public int? FormOrder { get; set; }

    public Guid? ParentId { get; set; }

    public int? PermissionActions { get; set; }

    public Guid? RoleId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? ResourceTypeId { get; set; }

    public string? ResourceTypeCode { get; set; }

    public string? FieldType { get; set; }

    public int? MaxLength { get; set; }

    public bool? IsRequired { get; set; }

    public bool? ShowInForm { get; set; }

    public bool? ShowInGrid { get; set; }

    public string? Route { get; set; }

    public bool? IsDynamicForm { get; set; }

    public string? LinkedFieldCode { get; set; }

    public bool? ShowInSelectBox { get; set; }

    public int? SelectBoxOrder { get; set; }

    public string? EntityName { get; set; }

    public string? ServiceName { get; set; }

    public string? SelectDisplayFields { get; set; }
}
