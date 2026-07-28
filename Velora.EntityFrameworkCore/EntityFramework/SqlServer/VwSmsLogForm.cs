using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSmsLogForm
{
    public Guid Id { get; set; }

    public int? Provider { get; set; }

    [StringLength(9)]
    public string ProvidereName { get; set; } = null!;

    [StringLength(1000)]
    public string Message { get; set; } = null!;

    [StringLength(50)]
    public string Mobile { get; set; } = null!;

    [StringLength(200)]
    public string? ProviderMessageId { get; set; }

    [StringLength(50)]
    public string SmsType { get; set; } = null!;

    public bool IsSuccess { get; set; }

    [StringLength(1000)]
    public string? ErrorMessage { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? SentAtPersian { get; set; }
}
