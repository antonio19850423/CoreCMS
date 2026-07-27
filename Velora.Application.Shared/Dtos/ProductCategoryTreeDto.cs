using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductCategoryTreeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;


        public Guid? ParentId { get; set; }


        public List<ProductCategoryTreeDto> Children { get; set; }
            = new();
    }
}
