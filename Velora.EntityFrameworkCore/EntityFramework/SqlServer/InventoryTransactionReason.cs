using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("InventoryTransactionReason", Schema = "cms")]
public partial class InventoryTransactionReason
{
    [Key]
    public byte Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("Reason")]
    public virtual ICollection<ProductInventoryTransaction> ProductInventoryTransactions { get; set; } = new List<ProductInventoryTransaction>();
}
