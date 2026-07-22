using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public class ProductDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public Guid? BrandId { get; set; }

        public Guid ProductTypeId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(250)]
        public string Slug { get; set; } = null!;

        [StringLength(100)]
        public string? Sku { get; set; }

        [StringLength(100)]
        public string? Barcode { get; set; }

        [StringLength(500)]
        public string? Summary { get; set; }

        public string? Description { get; set; }

        [StringLength(300)]
        public string? Thumbnail { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Weight { get; set; }

        [StringLength(200)]
        public string? SeoTitle { get; set; }

        [StringLength(500)]
        public string? SeoDescription { get; set; }

        public int? ViewCount { get; set; }

        public int? SaleCount { get; set; }

        public int SortOrder { get; set; }

        public bool? IsPublished { get; set; }

        public bool? IsFeatured { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsTest { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        [StringLength(300)]
        public string? MainImage { get; set; }
    }
}
