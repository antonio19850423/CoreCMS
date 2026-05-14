using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("ComponentFieldMaps", Schema = "cms")]
public partial class ComponentFieldMap
{
    [Key]
    public int Id { get; set; }

    public int ComponentTypeId { get; set; }

    [StringLength(100)]
    public string FieldName { get; set; } = null!;

    public bool IsVisible { get; set; }

    public int SortOrder { get; set; }

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("ComponentFieldMaps")]
    public virtual ComponentType ComponentType { get; set; } = null!;
}
