using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwProductAttributeValueForm
{
    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    public int SortOrder { get; set; }

    [StringLength(1000)]
    public string Value { get; set; } = null!;

    public Guid ProductAttributeId { get; set; }

    [StringLength(150)]
    public string? ProductAttributeName { get; set; }

    [StringLength(100)]
    public string? ProductAttributeCode { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}
