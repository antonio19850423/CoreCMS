using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwPaymentStatusLogForm
{
    public Guid Id { get; set; }

    public int NewStatus { get; set; }

    public int? OldStatus { get; set; }

    public Guid ParentId { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }
}
