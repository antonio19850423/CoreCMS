using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("States", Schema = "gen")]
public partial class State
{
    [Key]
    public Guid Id { get; set; }

    public Guid CountryId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsTest { get; set; }

    [InverseProperty("State")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("CountryId")]
    [InverseProperty("States")]
    public virtual Country Country { get; set; } = null!;
}
