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
    public class ShoppingCartViewDto
    {
        public Guid Id { get; set; }

        public string? CartToken { get; set; }


        public List<ShoppingCartItemViewDto> Items { get; set; }
            = new();

        public int TotalQuantity =>
            Items.Sum(x => x.Quantity);

        /// <summary>
        /// مبلغ کالاها قبل از تخفیف
        /// </summary>
        public decimal Subtotal =>
            Items.Sum(x => x.UnitPrice * x.Quantity);

        /// <summary>
        /// مجموع تخفیف
        /// </summary>
        public decimal TotalDiscount =>
            Items.Sum(x => x.Discount * x.Quantity);

        /// <summary>
        /// مبلغ نهایی پس از تخفیف
        /// </summary>
        public decimal TotalAmount =>
            Items.Sum(x => x.TotalPrice);
        public bool IsAllDownloadable { get; set; }
        public Guid? CouponId { get; set; }

        public string? CouponCode { get; set; }

        public decimal CouponDiscountAmount { get; set; }
    }
}
