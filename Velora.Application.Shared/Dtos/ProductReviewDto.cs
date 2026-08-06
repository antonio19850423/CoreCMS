using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductReviewDto
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
}
