using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("SeedHistory", Schema = "auth")]
[Index("Name", Name = "UQ_SeedHistory_Name", IsUnique = true)]
public partial class SeedHistory
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
