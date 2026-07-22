using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductSeedRoot
    {
        public List<ProductFullSeedModel> Products { get; set; } = new();
    }
}
