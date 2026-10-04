using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class InventoryVariantSeedModel
    {
        public string Sku { get; set; } = null!;

        public int InitialStock { get; set; }
    }
}
