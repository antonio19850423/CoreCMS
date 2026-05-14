using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("LocalizationKey", Schema = "gen")]
public partial class LocalizationKey
{
    [Key]
    [StringLength(200)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string Type { get; set; } = null!;

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public int? Order { get; set; }

    [InverseProperty("LocalizationKeyCodeNavigation")]
    public virtual ICollection<LocalizationTranslation> LocalizationTranslations { get; set; } = new List<LocalizationTranslation>();
}
