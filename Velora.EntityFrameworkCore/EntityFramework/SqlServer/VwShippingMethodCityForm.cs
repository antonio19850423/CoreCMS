using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwShippingMethodCityForm
{
    public Guid Id { get; set; }

    public Guid CityId { get; set; }

    [StringLength(100)]
    public string? CityTitle { get; set; }

    public Guid ParentId { get; set; }

    [StringLength(200)]
    public string? ShippingMethodTitle { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
