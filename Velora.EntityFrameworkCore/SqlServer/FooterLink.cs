using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.SqlServer;

[Table("FooterLinks", Schema = "site")]
public partial class FooterLink
{
    [Key]
    public int Id { get; set; }

    public int FooterColumnId { get; set; }

    [StringLength(150)]
    public string Title { get; set; } = null!;

    [StringLength(300)]
    public string Url { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("FooterColumnId")]
    [InverseProperty("FooterLinks")]
    public virtual FooterColumn FooterColumn { get; set; } = null!;
}
