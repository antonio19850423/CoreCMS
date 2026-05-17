using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("PageTemplates", Schema = "cms")]
[Index("Code", Name = "UQ__PageTemp__A25C5AA7F176466D", IsUnique = true)]
public partial class PageTemplate
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Code { get; set; } = null!;

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("PageTemplate")]
    public virtual ICollection<PageTemplateComponent> PageTemplateComponents { get; set; } = new List<PageTemplateComponent>();

    [InverseProperty("PageTemplate")]
    public virtual ICollection<Page> Pages { get; set; } = new List<Page>();
}
