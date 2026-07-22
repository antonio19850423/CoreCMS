using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductFullSeedModel
    {

        public ProductCategorySeedModel Category { get; set; } = new();

        public ProductBrandSeedModel Brand { get; set; } = new();
        public ProductTypeSeedModel ProductType { get; set; } = new();

        public ProductSeedModel Product { get; set; } = new();


        public List<ProductFileSeedModel> Files { get; set; } = new();

        public List<ProductVariantSeedModel> Variants { get; set; } = new();

        public List<ProductAttributeSeedModel> Attributes { get; set; } = new();


        public List<ProductTagSeedModel> Tags { get; set; } = new();

    }
}
