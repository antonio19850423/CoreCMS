using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("FooterColumns", Schema = "site")]
public partial class FooterColumn
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Title { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("FooterColumn")]
    public virtual ICollection<FooterLink> FooterLinks { get; set; } = new List<FooterLink>();
}
