using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwCategoryAttributeForm
{
    public Guid Id { get; set; }

    public Guid AttributeId { get; set; }

    [StringLength(150)]
    public string? AttributeName { get; set; }

    [StringLength(100)]
    public string? AttributeCode { get; set; }

    public Guid CategoryId { get; set; }

    [StringLength(150)]
    public string? CategoryName { get; set; }

    [StringLength(200)]
    public string? CategorySlug { get; set; }

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
