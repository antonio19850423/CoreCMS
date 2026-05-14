using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Keyless]
public partial class VwLocalization
{
    [StringLength(200)]
    public string LocalizationKeyCode { get; set; } = null!;

    [StringLength(10)]
    public string LanguageCode { get; set; } = null!;

    [StringLength(500)]
    public string Value { get; set; } = null!;

    [StringLength(50)]
    public string Type { get; set; } = null!;

    public int? Order { get; set; }
}
