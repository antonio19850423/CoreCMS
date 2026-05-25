using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("SiteSettings", Schema = "site")]
public partial class SiteSetting1
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string? SiteName { get; set; }

    [StringLength(250)]
    public string? Tagline { get; set; }

    [StringLength(300)]
    public string? LogoUrl { get; set; }

    [StringLength(300)]
    public string? FaviconUrl { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(150)]
    public string? Email { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    [StringLength(300)]
    public string? FacebookUrl { get; set; }

    [StringLength(300)]
    public string? InstagramUrl { get; set; }

    [StringLength(300)]
    public string? LinkedinUrl { get; set; }

    [StringLength(300)]
    public string? TwitterUrl { get; set; }

    [StringLength(300)]
    public string? YoutubeUrl { get; set; }

    [StringLength(300)]
    public string? TelegramUrl { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }
}
