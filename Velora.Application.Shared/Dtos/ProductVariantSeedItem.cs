using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductVariantSeedItem
    {
        public string ProductSlug { get; set; } = null!;

        public List<ProductVariantSeedModel> Variants { get; set; } = new();
    }
}
