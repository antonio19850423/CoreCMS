using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ResourceLanguage", Schema = "auth")]
[Index("ResourceId", "LanguageCode", Name = "UQ_ResourceLanguage", IsUnique = true)]
public partial class ResourceLanguage
{
    [Key]
    public Guid Id { get; set; }

    public Guid ResourceId { get; set; }

    [StringLength(10)]
    public string LanguageCode { get; set; } = null!;

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ResourceId")]
    [InverseProperty("ResourceLanguages")]
    public virtual Resource Resource { get; set; } = null!;
}
