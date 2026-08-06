using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("PaymentGateways", Schema = "cms")]
public partial class PaymentGateway
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string GatewayCode { get; set; } = null!;

    public int ProviderType { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(300)]
    public string? LogoUrl { get; set; }

    public string? SettingsJson { get; set; }

    [StringLength(500)]
    public string? CallbackUrl { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
