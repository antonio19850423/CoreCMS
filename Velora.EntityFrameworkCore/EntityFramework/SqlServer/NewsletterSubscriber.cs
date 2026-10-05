using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("NewsletterSubscriber", Schema = "cms")]
public partial class NewsletterSubscriber
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(320)]
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsConfirmed { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
