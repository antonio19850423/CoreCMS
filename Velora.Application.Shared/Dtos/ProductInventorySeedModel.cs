using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductInventorySeedModel
    {
        public string ProductSlug { get; set; } = null!;

        public int InitialStock { get; set; }
    }
}
