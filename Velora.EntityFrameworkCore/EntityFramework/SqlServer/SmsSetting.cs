using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("SmsSettings", Schema = "cms")]
public partial class SmsSetting
{
    [Key]
    public Guid Id { get; set; }

    public int? Provider { get; set; }

    [StringLength(500)]
    public string ApiKey { get; set; } = null!;

    [StringLength(50)]
    public string? SenderNumber { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }
}
