using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("LocalizationTranslation", Schema = "gen")]
[Index("LanguageCode", Name = "IX_LocalizationTranslation_LanguageCode")]
[Index("LocalizationKeyCode", "LanguageCode", Name = "UQ_LocalizationTranslation_Key_Language", IsUnique = true)]
public partial class LocalizationTranslation
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string LocalizationKeyCode { get; set; } = null!;

    [StringLength(10)]
    public string LanguageCode { get; set; } = null!;

    [StringLength(500)]
    public string Value { get; set; } = null!;

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("LocalizationKeyCode")]
    [InverseProperty("LocalizationTranslations")]
    public virtual LocalizationKey LocalizationKeyCodeNavigation { get; set; } = null!;
}
