using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class Resource
{
    public Guid Id { get; set; }

    public Guid ResourceTypeId { get; set; }

    public Guid? ParentId { get; set; }

    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool Istest { get; set; }

    public string? FieldType { get; set; }

    public int? MaxLength { get; set; }

    public bool IsRequired { get; set; }

    public bool ShowInForm { get; set; }

    public bool ShowInGrid { get; set; }

    public int? FormOrder { get; set; }

    public int? GridOrder { get; set; }

    public string? Route { get; set; }

    public string? InputMask { get; set; }

    public bool? IsDynamicForm { get; set; }

    public string? LinkedFieldCode { get; set; }

    public bool? ShowInSelectBox { get; set; }

    public int? SelectBoxOrder { get; set; }

    public string? EntityName { get; set; }

    public string? ServiceName { get; set; }

    public string? SelectDisplayFields { get; set; }

    public virtual ICollection<Resource> InverseParent { get; set; } = new List<Resource>();

    public virtual Resource? Parent { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<ResourceLanguage> ResourceLanguages { get; set; } = new List<ResourceLanguage>();

    public virtual ResourceType ResourceType { get; set; } = null!;
}
