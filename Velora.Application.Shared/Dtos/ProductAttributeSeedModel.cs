using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductAttributeSeedModel
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;


        public int SortOrder { get; set; }
    }
}
