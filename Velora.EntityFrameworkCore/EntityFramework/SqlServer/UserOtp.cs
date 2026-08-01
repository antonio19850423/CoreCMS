using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("UserOtp", Schema = "auth")]
public partial class UserOtp
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(20)]
    public string Mobile { get; set; } = null!;

    [StringLength(500)]
    public string CodeHash { get; set; } = null!;

    public int Purpose { get; set; }

    public DateTime ExpiresAt { get; set; }

    public int AttemptCount { get; set; }

    public int MaxAttempts { get; set; }

    public bool IsUsed { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsVerified { get; set; }

    public DateTime? VerifiedAt { get; set; }
}
