using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductFileSeedItem
    {
        public string ProductSlug { get; set; } = null!;

        public List<ProductFileSeedModel> Files { get; set; } = new();
    }
}
