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


        /// <summary>
        /// شناسه سبد مهمان
        /// برای کاربر بدون لاگین
        /// </summary>
        public string? CartToken { get; set; }



        public List<ShoppingCartItemViewDto> Items { get; set; }
            = new();



        /// <summary>
        /// تعداد کل محصولات
        /// </summary>
        public int TotalQuantity =>
            Items.Sum(x => x.Quantity);



        /// <summary>
        /// مبلغ کل سبد
        /// </summary>
        public decimal TotalAmount =>
            Items.Sum(x => x.TotalPrice);
    }
}
