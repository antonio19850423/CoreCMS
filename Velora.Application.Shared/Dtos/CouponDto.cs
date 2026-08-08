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
    public  class CouponDto
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Code { get; set; } = null!;

        public int? UsageLimit { get; set; }

        public int UsedCount { get; set; }

        public bool IsSingleUsePerUser { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool? CanCombineWithDiscount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public byte CouponType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CouponValue { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MinimumOrderAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MaximumDiscountAmount { get; set; }

    }
}
