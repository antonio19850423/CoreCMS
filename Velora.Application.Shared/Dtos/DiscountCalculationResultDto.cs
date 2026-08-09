using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class DiscountCalculationResultDto
    {
        public bool HasDiscount { get; set; }

        public Guid? DiscountId { get; set; }

        public Guid? DiscountItemId { get; set; }

        public byte? DiscountType { get; set; }

        public decimal? DiscountValue { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal FinalPrice { get; set; }
    }
}
