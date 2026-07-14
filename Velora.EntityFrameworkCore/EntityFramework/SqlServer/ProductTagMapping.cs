using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductTagMapping", Schema = "cms")]
[Index("ProductId", Name = "IX_ProductTagMapping_Product")]
[Index("ProductTagId", Name = "IX_ProductTagMapping_Tag")]
[Index("ProductId", "ProductTagId", Name = "UQ_ProductTagMapping", IsUnique = true)]
public partial class ProductTagMapping
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid ProductTagId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductTagMappings")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("ProductTagId")]
    [InverseProperty("ProductTagMappings")]
    public virtual ProductTag ProductTag { get; set; } = null!;
}
