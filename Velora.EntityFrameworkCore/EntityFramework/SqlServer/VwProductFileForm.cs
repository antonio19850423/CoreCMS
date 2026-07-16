using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductFileForm
{
    public Guid Id { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    [StringLength(200)]
    public string? Alt { get; set; }

    [StringLength(300)]
    public string FileUrl { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsMain { get; set; }

    [StringLength(30)]
    public string MediaType { get; set; } = null!;

    public Guid ParentId { get; set; }

    public int SortOrder { get; set; }

    [StringLength(300)]
    public string? ThumbnailUrl { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
