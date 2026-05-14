using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("GeneralSettings", Schema = "gen")]
[Index("Key", Name = "UQ__GeneralS__C41E0289F449EC30", IsUnique = true)]
public partial class GeneralSetting
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Key { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string Value { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
