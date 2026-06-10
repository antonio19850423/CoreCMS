using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class SiteGlobalSettingView
{
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

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? Phone2 { get; set; }

    [StringLength(50)]
    public string? Mobile { get; set; }

    [StringLength(50)]
    public string? Fax { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(1000)]
    public string? Address { get; set; }

    [StringLength(1000)]
    public string? Address2 { get; set; }

    [StringLength(300)]
    public string? DefaultMetaTitle { get; set; }

    [StringLength(1000)]
    public string? DefaultMetaDescription { get; set; }

    [StringLength(1000)]
    public string? DefaultMetaKeywords { get; set; }

    public bool EnableBlog { get; set; }

    public bool EnableShop { get; set; }

    public bool EnableSeo { get; set; }

    public bool EnableCache { get; set; }

    public bool EnableComments { get; set; }

    public bool EnableMultiLanguage { get; set; }

    public bool EnableNews { get; set; }

    [StringLength(50)]
    public string SiteType { get; set; } = null!;

    [StringLength(100)]
    public string? DefaultTheme { get; set; }
}
