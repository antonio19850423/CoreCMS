using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwPageForm
{
    public Guid Id { get; set; }

    [StringLength(300)]
    public string? CanonicalUrl { get; set; }

    public bool IsActive { get; set; }

    public bool IsHome { get; set; }

    public bool IsPublished { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(500)]
    public string? MetaKeywords { get; set; }

    [StringLength(200)]
    public string? MetaTitle { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(300)]
    public string? OgImageUrl { get; set; }

    public Guid? PageTemplateId { get; set; }

    [StringLength(150)]
    public string? PageTemplateName { get; set; }

    [StringLength(200)]
    public string Slug { get; set; } = null!;

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
