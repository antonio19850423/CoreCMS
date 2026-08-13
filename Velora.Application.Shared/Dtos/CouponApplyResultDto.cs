using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CouponApplyResultDto
    {
        public Guid CouponId { get; set; }
        public string CouponCode { get; set; } = string.Empty;

        public decimal OrderAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }

        public byte CouponType { get; set; }
        public decimal CouponValue { get; set; }
    }
}
