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
    public  class ShoppingCartItemDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ShoppingCartId { get; set; }

        public Guid ProductId { get; set; }

        public Guid? VariantId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? DiscountId { get; set; }

        public Guid? DiscountItemId { get; set; }

        public int? DiscountType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountValue { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal FinalUnitPrice { get; set; }

        public Guid? ProductTypeId { get; set; }
    }
}
