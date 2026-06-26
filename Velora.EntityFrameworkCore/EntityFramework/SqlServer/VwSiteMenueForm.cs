using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSiteMenueForm
{
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    [StringLength(200)]
    public string? ParentName { get; set; }

    public int SortOrder { get; set; }

    [StringLength(200)]
    public string Link1Text { get; set; } = null!;

    [StringLength(500)]
    public string? Link1Url { get; set; }

    public Guid? Link1TargetId { get; set; }

    public Guid? Link1TypeId { get; set; }

    [StringLength(50)]
    public string? Link1Color { get; set; }

    public bool? Link1OpenInNewTab { get; set; }

    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
