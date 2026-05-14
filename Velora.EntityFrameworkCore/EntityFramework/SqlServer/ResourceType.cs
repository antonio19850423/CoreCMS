using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ResourceTypes", Schema = "auth")]
[Index("Code", Name = "UQ__Resource__A25C5AA7C5256CAE", IsUnique = true)]
public partial class ResourceType
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string DisplayName { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsTest { get; set; }

    [InverseProperty("ResourceType")]
    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
