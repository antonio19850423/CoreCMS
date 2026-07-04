using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwCmsConfigurationForm
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? DefaultTheme { get; set; }

    public bool EnableBlog { get; set; }

    public bool EnableCache { get; set; }

    public bool EnableComments { get; set; }

    public bool EnableMultiLanguage { get; set; }

    public bool EnableNews { get; set; }

    public bool EnableSeo { get; set; }

    public bool EnableShop { get; set; }

    public bool IsActive { get; set; }

    [StringLength(50)]
    public string SiteType { get; set; } = null!;

    public bool? EnablePrivacy { get; set; }

    public bool? EnableFaq { get; set; }

    public bool? EnableDynamicPages { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
