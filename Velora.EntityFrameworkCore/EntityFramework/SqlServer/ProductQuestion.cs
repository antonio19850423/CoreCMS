using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ProductQuestions", Schema = "cms")]
public partial class ProductQuestion
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid? UserId { get; set; }

    [StringLength(1000)]
    public string Question { get; set; } = null!;

    public string? Answer { get; set; }

    public Guid? AnsweredBy { get; set; }

    public bool IsAnswered { get; set; }

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
