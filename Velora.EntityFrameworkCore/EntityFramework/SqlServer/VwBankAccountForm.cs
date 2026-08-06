using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwBankAccountForm
{
    public Guid Id { get; set; }

    [StringLength(200)]
    public string AccountOwnerName { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string BankName { get; set; } = null!;

    [StringLength(50)]
    public string? CardNumber { get; set; }

    [StringLength(100)]
    public string? AccountNumber { get; set; }

    [StringLength(50)]
    public string? ShebaNumber { get; set; }

    public Guid ParentId { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
