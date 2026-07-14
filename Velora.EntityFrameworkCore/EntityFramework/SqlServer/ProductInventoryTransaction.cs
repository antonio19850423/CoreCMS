using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductInventoryTransaction", Schema = "cms")]
[Index("TransactionDate", Name = "IX_ProductInventoryTransaction_Date")]
[Index("ProductId", Name = "IX_ProductInventoryTransaction_ProductId")]
[Index("ProductVariantId", Name = "IX_ProductInventoryTransaction_ProductVariantId")]
[Index("ReasonId", Name = "IX_ProductInventoryTransaction_ReasonId")]
[Index("ReferenceType", "ReferenceId", Name = "IX_ProductInventoryTransaction_Reference")]
public partial class ProductInventoryTransaction
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? ProductVariantId { get; set; }

    public byte OperationType { get; set; }

    public int ChangeQuantity { get; set; }

    public byte ReasonId { get; set; }

    public byte? ReferenceType { get; set; }

    public Guid? ReferenceId { get; set; }

    public Guid? ReferenceDetailId { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public DateTime TransactionDate { get; set; }

    public Guid? CreatedBy { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductInventoryTransactions")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("ProductInventoryTransactions")]
    public virtual ProductVariant? ProductVariant { get; set; }

    [ForeignKey("ReasonId")]
    [InverseProperty("ProductInventoryTransactions")]
    public virtual InventoryTransactionReason Reason { get; set; } = null!;
}
