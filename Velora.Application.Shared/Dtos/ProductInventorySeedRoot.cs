using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductInventorySeedRoot
    {
        public List<ProductInventorySeedModel> ProductInventory { get; set; } = new();
    }
}
