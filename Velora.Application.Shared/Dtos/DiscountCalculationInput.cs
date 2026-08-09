using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class DiscountCalculationInput
    {
        public Guid ProductId { get; set; }

        public Guid? ProductVariantId { get; set; }

        public Guid? ProductBrandId { get; set; }

        public Guid? ProductCategoryId { get; set; }

        public decimal Price { get; set; }
    }
}
