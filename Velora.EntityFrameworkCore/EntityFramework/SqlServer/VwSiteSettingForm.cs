using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSiteSettingForm
{
    public Guid Id { get; set; }

    [StringLength(200)]
    public string SiteName { get; set; } = null!;

    [StringLength(300)]
    public string? DomainName { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }

    [StringLength(200)]
    public string? LogoAlt { get; set; }

    [StringLength(500)]
    public string? DarkLogoUrl { get; set; }

    [StringLength(200)]
    public string? DarkLogoAlt { get; set; }

    [StringLength(500)]
    public string? FaviconUrl { get; set; }

    [StringLength(100)]
    public string? PhoneTitle { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Phone2Title { get; set; }

    [StringLength(50)]
    public string? Phone2 { get; set; }

    [StringLength(100)]
    public string? MobileTitle { get; set; }

    [StringLength(50)]
    public string? Mobile { get; set; }

    [StringLength(100)]
    public string? FaxTitle { get; set; }

    [StringLength(50)]
    public string? Fax { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? AddressTitle { get; set; }

    [StringLength(1000)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? Address2Title { get; set; }

    [StringLength(1000)]
    public string? Address2 { get; set; }

    [StringLength(300)]
    public string? DefaultMetaTitle { get; set; }

    [StringLength(1000)]
    public string? DefaultMetaDescription { get; set; }

    [StringLength(1000)]
    public string? DefaultMetaKeywords { get; set; }

    public bool IsActive { get; set; }

    public bool? SmtpEnableSsl { get; set; }

    [StringLength(200)]
    public string? SmtpHost { get; set; }

    public int? SmtpPort { get; set; }

    [StringLength(300)]
    public string? SmtpUserName { get; set; }

    [StringLength(500)]
    public string? SmtpPassword { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
