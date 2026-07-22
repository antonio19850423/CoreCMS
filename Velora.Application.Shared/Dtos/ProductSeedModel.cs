using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public class ProductSeedModel
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;
        public Guid CategoryId { get; set; }

        public Guid? BrandId { get; set; }

        public Guid ProductTypeId { get; set; }


        public string? Summary { get; set; }

        public string? Description { get; set; }


        public decimal Price { get; set; }


        public string? Barcode { get; set; }

        public string? Sku { get; set; }


        public decimal? Weight { get; set; }


        public string? MainImage { get; set; }

        public string? Thumbnail { get; set; }


        public string? SeoTitle { get; set; }

        public string? SeoDescription { get; set; }


        public int SortOrder { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsPublished { get; set; }
        public int InitialStock { get; set; }

        public bool IsActive { get; set; }

        public List<ProductFileSeedModel> Files { get; set; } = new();

        public List<ProductVariantSeedModel> Variants { get; set; } = new();

        public List<ProductAttributeSeedModel> Attributes { get; set; } = new();
    }
}
