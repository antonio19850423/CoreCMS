using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ShoppingCartRequestDto
    {

        /// <summary>
        /// محصول انتخاب شده
        /// </summary>
        public Guid ProductId { get; set; }



        /// <summary>
        /// واریانت انتخاب شده
        /// ممکن است محصول واریانت نداشته باشد
        /// </summary>
        public Guid? VariantId { get; set; }



        /// <summary>
        /// تعداد
        /// </summary>
        public int Quantity { get; set; } = 1;

    }
}
