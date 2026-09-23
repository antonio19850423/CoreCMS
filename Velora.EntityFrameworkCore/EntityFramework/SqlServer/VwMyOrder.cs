using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwMyOrder
{
    public Guid Id { get; set; }

    [StringLength(50)]
    public string? OrderCode { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? OrderedAt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalAmount { get; set; }

    public int? OrderStatus { get; set; }

    public int? PaymentMethod { get; set; }

    public DateTime? PaidAt { get; set; }

    [StringLength(16)]
    public string PaymentStatusTitle { get; set; } = null!;

    [StringLength(13)]
    public string OrderStatusTitle { get; set; } = null!;

    [StringLength(19)]
    public string? OrderedAtPersian { get; set; }

    public int? ItemCount { get; set; }
}
