using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwPaymentGatewayForm
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public int ProviderType { get; set; }

    [StringLength(17)]
    public string ProviderTypeTitle { get; set; } = null!;

    [StringLength(50)]
    public string GatewayCode { get; set; } = null!;

    public string? SettingsJson { get; set; }

    [StringLength(300)]
    public string? LogoUrl { get; set; }

    [StringLength(500)]
    public string? CallbackUrl { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsDefault { get; set; }

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
