using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwCustomerManagement
{
    public Guid? Id { get; set; }

    [StringLength(201)]
    public string? CustomerName { get; set; }

    public int? OrderCount { get; set; }

    public int? SoldQuantity { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? GrossSalesAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? ProductDiscountAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? CouponDiscountAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? TotalDiscountAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? NetSalesAmount { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? SalesAfterProductDiscount { get; set; }

    public DateTime? LastOrderAt { get; set; }

    [StringLength(19)]
    public string? LastOrderAtPersian { get; set; }
}
