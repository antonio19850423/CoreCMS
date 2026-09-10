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
    public  class PaymentStatusLogDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PaymentId { get; set; }

        public int? OldStatus { get; set; }

        public int NewStatus { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
