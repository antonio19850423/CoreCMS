using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductListViewDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string? Summary { get; set; }

        public decimal Price { get; set; }

        public string? MainImage { get; set; }

        public string? Thumbnail { get; set; }


        public string CategoryName { get; set; } = null!;

        public string CategorySlug { get; set; } = null!;


        public string? BrandName { get; set; }

        public string? BrandSlug { get; set; }


        public int Inventory { get; set; }


        public List<ProductMediaViewDto> Gallery { get; set; }
            = new();


        public DateTime CreatedAt { get; set; }
        public bool HasVariant { get; set; }

        public Guid? DefaultVariantId { get; set; }
        public bool HasDiscount { get; set; }

        public Guid? DiscountId { get; set; }

        public Guid? DiscountItemId { get; set; }

        public byte? DiscountType { get; set; }

        public decimal? DiscountValue { get; set; }

        public decimal? DiscountAmount { get; set; }

        public decimal? FinalPrice { get; set; }
    }
}
