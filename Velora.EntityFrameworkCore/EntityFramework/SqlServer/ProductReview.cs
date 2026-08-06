using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductReviews", Schema = "cms")]
public partial class ProductReview
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? UserId { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    public string Comment { get; set; } = null!;

    public int Rate { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
