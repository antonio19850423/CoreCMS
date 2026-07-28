using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSmsSettingForm
{
    public Guid Id { get; set; }

    [StringLength(500)]
    public string ApiKey { get; set; } = null!;

    public int? Provider { get; set; }

    [StringLength(9)]
    public string ProvidereName { get; set; } = null!;

    [StringLength(50)]
    public string? SenderNumber { get; set; }

    public bool IsActive { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
