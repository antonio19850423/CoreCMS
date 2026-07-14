using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductAttributeValue", Schema = "cms")]
[Index("ProductId", Name = "IX_ProductAttributeValue_Product")]
[Index("ProductAttributeId", Name = "IX_ProductAttributeValue_ProductAttribute")]
[Index("ProductId", "ProductAttributeId", Name = "UQ_ProductAttributeValue", IsUnique = true)]
public partial class ProductAttributeValue
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid ProductAttributeId { get; set; }

    [StringLength(1000)]
    public string Value { get; set; } = null!;

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductAttributeValues")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ProductAttributeId")]
    [InverseProperty("ProductAttributeValues")]
    public virtual ProductAttribute ProductAttribute { get; set; } = null!;
}
