using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("PaymentStatusLogs", Schema = "cms")]
public partial class PaymentStatusLog
{
    [Key]
    public Guid Id { get; set; }

    public Guid PaymentId { get; set; }

    public int? OldStatus { get; set; }

    public int NewStatus { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("PaymentId")]
    [InverseProperty("PaymentStatusLogs")]
    public virtual Payment Payment { get; set; } = null!;
}
