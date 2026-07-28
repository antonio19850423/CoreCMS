using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("SmsLogs", Schema = "cms")]
public partial class SmsLog
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Mobile { get; set; } = null!;

    [StringLength(1000)]
    public string Message { get; set; } = null!;

    public int? Provider { get; set; }

    [StringLength(200)]
    public string? ProviderMessageId { get; set; }

    [StringLength(50)]
    public string SmsType { get; set; } = null!;

    public bool IsSuccess { get; set; }

    [StringLength(1000)]
    public string? ErrorMessage { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime CreatedAt { get; set; }
}
