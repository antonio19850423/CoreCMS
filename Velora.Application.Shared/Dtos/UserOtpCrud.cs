using System.ComponentModel.DataAnnotations;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos;
public class UserOtpCrud : BulkInsert
{
    public Guid Id { get; set; }

    public int AttemptCount { get; set; }

    [StringLength(500)]
    public string CodeHash { get; set; } = null!;

    public bool IsUsed { get; set; }

    public int MaxAttempts { get; set; }

    [StringLength(20)]
    public string Mobile { get; set; } = null!;

    public int Purpose { get; set; }

    [StringLength(19)]
    public string? UsedAtPersian { get; set; }

    [StringLength(19)]
    public string? ExpiresAtPersian { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
    public DateTime? UsedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
    public bool IsVerified { get; set; }

    public DateTime? VerifiedAt { get; set; }
    [StringLength(19)]
    public string? VerifiedAtPersian { get; set; }
    public DateTime CreatedAt { get; set; }
}