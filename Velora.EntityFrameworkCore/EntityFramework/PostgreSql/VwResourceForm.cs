using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VwResourceForm
{
    public Guid? Id { get; set; }

    public string? Code { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public string? EntityName { get; set; }

    public string? FieldType { get; set; }

    public int? FormOrder { get; set; }

    public int? GridOrder { get; set; }

    public string? InputMask { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDynamicForm { get; set; }

    public bool? IsRequired { get; set; }

    public string? LinkedFieldCode { get; set; }

    public int? MaxLength { get; set; }

    public int? Order { get; set; }

    public Guid? ParentId { get; set; }

    public string? ParentDisplayName { get; set; }

    public Guid? ResourceTypeId { get; set; }

    public string? ResourceTypeTitle { get; set; }

    public string? Route { get; set; }

    public int? SelectBoxOrder { get; set; }

    public string? ServiceName { get; set; }

    public bool? ShowInForm { get; set; }

    public bool? ShowInGrid { get; set; }

    public bool? ShowInSelectBox { get; set; }
}
