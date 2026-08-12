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
    public class ShoppingCartItemViewDto
    {
        public Guid Id { get; set; }

        public Guid ShoppingCartId { get; set; }

        public Guid ProductId { get; set; }
        public Guid? ProductTypeId { get; set; }
        public Guid? VariantId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? VariantName { get; set; }

        public string? ImageUrl { get; set; }

        /// <summary>
        /// قیمت فعلی هر واحد قبل از تخفیف
        /// </summary>
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// تخفیف اعمال شده برای هر واحد
        /// </summary>
        public decimal Discount { get; set; }
        public byte? DiscountType { get; set; }

        public decimal? DiscountValue { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal? FinalPrice { get; set; }

        /// <summary>
        /// شناسه تخفیق اعمال شده
        /// </summary>
        public Guid? DiscountId { get; set; }

        /// <summary>
        /// قیمت نهایی هر واحد
        /// </summary>
        public decimal FinalUnitPrice =>
            Math.Max(0, UnitPrice - Discount);

        /// <summary>
        /// مبلغ نهایی آیتم
        /// </summary>
        public decimal TotalPrice =>
            FinalUnitPrice * Quantity;
    }
}
