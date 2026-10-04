using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class DiscountItemSeedModel
    {
        public string DiscountName { get; set; } = null!;

        public string? ProductSlug { get; set; }

        public string? VariantSku { get; set; }

        public string? CategorySlug { get; set; }

        public string? BrandSlug { get; set; }

        public int SortOrder { get; set; }
    }
}
