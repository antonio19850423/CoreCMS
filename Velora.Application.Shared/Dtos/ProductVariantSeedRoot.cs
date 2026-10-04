using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductVariantSeedRoot
    {
        public List<ProductVariantSeedItem> ProductVariants { get; set; } = new();
    }
}
