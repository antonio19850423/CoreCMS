using Microsoft.EntityFrameworkCore;
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
    public  class ProductVariantDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [StringLength(100)]
        public string? Sku { get; set; }

        [StringLength(100)]
        public string? Barcode { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ComparePrice { get; set; }

        [StringLength(300)]
        public string? Image { get; set; }

        public int SortOrder { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        [StringLength(150)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }

    }
}
