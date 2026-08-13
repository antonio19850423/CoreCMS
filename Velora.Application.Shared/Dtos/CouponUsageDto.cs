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
    public  class CouponUsageDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CouponId { get; set; }

        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public DateTime UsedAt { get; set; }
    }
}
