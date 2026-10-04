using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CategorySeedRoot
    {
        public List<ProductCategorySeedModel> Categories { get; set; }
            = new();
    }
}
