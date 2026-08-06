using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ShippingMethodCities", Schema = "cms")]
[Index("ShippingMethodId", "CityId", Name = "UQ_ShippingMethodCities", IsUnique = true)]
public partial class ShippingMethodCity
{
    [Key]
    public Guid Id { get; set; }

    public Guid ShippingMethodId { get; set; }

    public Guid CityId { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    [ForeignKey("ShippingMethodId")]
    [InverseProperty("ShippingMethodCities")]
    public virtual ShippingMethod ShippingMethod { get; set; } = null!;
}
