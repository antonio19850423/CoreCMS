using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ShippingMethods", Schema = "cms")]
public partial class ShippingMethod
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int EstimatedDays { get; set; }

    public bool IsNationwide { get; set; }

    public bool IsDefault { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [InverseProperty("ShippingMethod")]
    public virtual ICollection<ShippingMethodCity> ShippingMethodCities { get; set; } = new List<ShippingMethodCity>();
}
