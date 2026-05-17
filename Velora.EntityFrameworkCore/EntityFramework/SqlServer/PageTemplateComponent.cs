using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("PageTemplateComponents", Schema = "cms")]
public partial class PageTemplateComponent
{
    [Key]
    public Guid Id { get; set; }

    public Guid PageTemplateId { get; set; }

    public int SortOrder { get; set; }

    public Guid ComponentTypeId { get; set; }

    [StringLength(100)]
    public string ComponentVariant { get; set; } = null!;

    public bool IsEditable { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("PageTemplateComponents")]
    public virtual ComponentType ComponentType { get; set; } = null!;

    [ForeignKey("PageTemplateId")]
    [InverseProperty("PageTemplateComponents")]
    public virtual PageTemplate PageTemplate { get; set; } = null!;
}
