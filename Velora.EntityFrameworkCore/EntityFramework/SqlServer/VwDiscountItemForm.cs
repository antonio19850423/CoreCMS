using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwDiscountItemForm
{
    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    [StringLength(200)]
    public string? ParentName { get; set; }

    public Guid? ProductBrandId { get; set; }

    [StringLength(150)]
    public string? ProductBrandName { get; set; }

    public Guid? ProductCategoryId { get; set; }

    [StringLength(150)]
    public string? ProductCategoryName { get; set; }

    public Guid? ProductId { get; set; }

    [StringLength(200)]
    public string? ProductName { get; set; }

    public Guid? ProductVariantId { get; set; }

    [StringLength(150)]
    public string? ProductVariantName { get; set; }

    public int SortOrder { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
