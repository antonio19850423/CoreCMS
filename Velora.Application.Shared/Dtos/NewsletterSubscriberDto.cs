using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public class NewsletterSubscriberDto
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
}
