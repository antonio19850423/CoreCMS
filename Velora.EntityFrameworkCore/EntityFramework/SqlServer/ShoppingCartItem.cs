using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ShoppingCartItems", Schema = "cms")]
[Index("ProductId", Name = "IX_ShoppingCartItems_ProductId")]
[Index("ShoppingCartId", Name = "IX_ShoppingCartItems_ShoppingCartId")]
[Index("VariantId", Name = "IX_ShoppingCartItems_VariantId")]
public partial class ShoppingCartItem
{
    [Key]
    public Guid Id { get; set; }

    public Guid ShoppingCartId { get; set; }

    public Guid ProductId { get; set; }

    public Guid? VariantId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? DiscountId { get; set; }

    public Guid? DiscountItemId { get; set; }

    public int? DiscountType { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DiscountValue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal FinalUnitPrice { get; set; }

    public Guid? ProductTypeId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ShoppingCartItems")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ShoppingCartId")]
    [InverseProperty("ShoppingCartItems")]
    public virtual ShoppingCart ShoppingCart { get; set; } = null!;

    [ForeignKey("VariantId")]
    [InverseProperty("ShoppingCartItems")]
    public virtual ProductVariant? Variant { get; set; }
}
