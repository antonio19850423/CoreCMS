using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductVariantViewDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public decimal? ComparePrice { get; set; }

        public string? Sku { get; set; }

        public string? Barcode { get; set; }

        public string? Image { get; set; }

        public bool IsDefault { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
        public int Stock { get; set; }
    }
}
