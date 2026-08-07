using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductDetailViewDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;


        public string? Summary { get; set; }

        public string? Description { get; set; }


        public string? MainImage { get; set; }

        public string? Thumbnail { get; set; }



        public CategoryViewDto? Category { get; set; }


        public BrandViewDto? Brand { get; set; }



        public decimal Price { get; set; }


        public decimal? OldPrice { get; set; }



        public List<ProductMediaViewDto> Gallery { get; set; }
            = new();



        public List<ProductAttributeViewDto> Attributes { get; set; }
            = new();



        public List<ProductVariantViewDto> Variants { get; set; }
            = new();

        public List<ProductTagViewDto> Tags { get; set; }
            = new();

    }
}
