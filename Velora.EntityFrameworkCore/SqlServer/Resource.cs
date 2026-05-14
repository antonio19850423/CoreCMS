using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("Resources", Schema = "auth")]
[Index("Code", Name = "UQ__Resource__A25C5AA778CFA20B", IsUnique = true)]
public partial class Resource
{
    [Key]
    public Guid Id { get; set; }

    public Guid ResourceTypeId { get; set; }

    public Guid? ParentId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(250)]
    public string DisplayName { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Description { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsTest { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FieldType { get; set; }

    public int? MaxLength { get; set; }

    public bool IsRequired { get; set; }

    public bool ShowInForm { get; set; }

    public bool ShowInGrid { get; set; }

    public int? FormOrder { get; set; }

    public int? GridOrder { get; set; }

    [StringLength(255)]
    public string? Route { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? InputMask { get; set; }

    public bool? IsDynamicForm { get; set; }

    [StringLength(200)]
    public string? LinkedFieldCode { get; set; }

    public bool? ShowInSelectBox { get; set; }

    public int? SelectBoxOrder { get; set; }

    [StringLength(100)]
    public string? EntityName { get; set; }

    [StringLength(100)]
    public string? ServiceName { get; set; }

    [StringLength(300)]
    public string? SelectDisplayFields { get; set; }

    [InverseProperty("Resource")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    [InverseProperty("Resource")]
    public virtual ICollection<ResourceLanguage> ResourceLanguages { get; set; } = new List<ResourceLanguage>();

    [ForeignKey("ResourceTypeId")]
    [InverseProperty("Resources")]
    public virtual ResourceType ResourceType { get; set; } = null!;
}
