using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwShoppingCartItemForm
{
    public Guid? Id { get; set; }

    public Guid ParentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DiscountAmount { get; set; }

    public Guid? DiscountId { get; set; }

    public Guid? DiscountItemId { get; set; }

    public int? DiscountType { get; set; }

    [StringLength(9)]
    public string DiscountTypeTitle { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DiscountValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? FinalUnitPrice { get; set; }

    public Guid? ProductId { get; set; }

    [StringLength(200)]
    public string? ProductName { get; set; }

    [StringLength(150)]
    public string? ProductCategoryName { get; set; }

    [StringLength(150)]
    public string? ProductBrandName { get; set; }

    [StringLength(100)]
    public string? ProductTypeName { get; set; }

    public Guid? ProductTypeId { get; set; }

    public int? Quantity { get; set; }

    public Guid? ShoppingCartId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? VariantId { get; set; }

    [StringLength(150)]
    public string? ProductVariantName { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ProductVariantPrice { get; set; }

    [StringLength(50)]
    public string? OrderCode { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }
}
