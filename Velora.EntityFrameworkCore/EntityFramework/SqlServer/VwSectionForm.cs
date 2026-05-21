using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwSectionForm
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? ButtonText { get; set; }

    [StringLength(300)]
    public string? ButtonUrl { get; set; }

    public int? ColumnsCount { get; set; }

    public Guid ComponentTypeId { get; set; }

    [StringLength(100)]
    public string? ComponentTypeName { get; set; }

    public string? Description { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public Guid PageId { get; set; }

    public int SortOrder { get; set; }

    public Guid ParentId { get; set; }

    [StringLength(500)]
    public string? Subtitle { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
