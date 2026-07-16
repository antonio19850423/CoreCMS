using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductInventoryTransactionForm
{
    public Guid Id { get; set; }

    public int ChangeQuantity { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public byte OperationType { get; set; }

    [StringLength(6)]
    public string OperationTypeName { get; set; } = null!;

    public Guid ParentId { get; set; }

    [StringLength(200)]
    public string? ProductName { get; set; }

    public Guid? ProductVariantId { get; set; }

    [StringLength(150)]
    public string? ProductVariantName { get; set; }

    public Guid ReasonId { get; set; }

    [StringLength(100)]
    public string? ReasonName { get; set; }

    [StringLength(50)]
    public string? ReasonCode { get; set; }

    public Guid? ReferenceId { get; set; }

    public Guid? ReferenceDetailId { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }
}
