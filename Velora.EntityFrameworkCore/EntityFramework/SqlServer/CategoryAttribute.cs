using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("CategoryAttribute", Schema = "cms")]
[Index("CategoryId", "AttributeId", Name = "UQ_CategoryAttribute", IsUnique = true)]
public partial class CategoryAttribute
{
    [Key]
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Guid AttributeId { get; set; }

    public int SortOrder { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("AttributeId")]
    [InverseProperty("CategoryAttributes")]
    public virtual ProductAttribute Attribute { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("CategoryAttributes")]
    public virtual ProductCategory Category { get; set; } = null!;
}
