using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductReviewForm
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    [StringLength(200)]
    public string? ProductTitle { get; set; }

    [StringLength(250)]
    public string? ProductSlug { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    public int Rate { get; set; }

    public string Comment { get; set; } = null!;

    public Guid? UserId { get; set; }

    [StringLength(201)]
    public string UserName { get; set; } = null!;

    public bool IsApproved { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
