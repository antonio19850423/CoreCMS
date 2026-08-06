using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductQuestionForm
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    [StringLength(200)]
    public string? ProductTitle { get; set; }

    [StringLength(250)]
    public string? ProductSlug { get; set; }

    [StringLength(1000)]
    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    public Guid? AnsweredBy { get; set; }

    [StringLength(201)]
    public string AnsweredName { get; set; } = null!;

    public bool IsAnswered { get; set; }

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
