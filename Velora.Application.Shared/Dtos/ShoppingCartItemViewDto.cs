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


        public Guid? VariantId { get; set; }



        public string ProductName { get; set; } = null!;


        public string? VariantName { get; set; }



        public string? ImageUrl { get; set; }



        public decimal UnitPrice { get; set; }



        public int Quantity { get; set; }



        public decimal TotalPrice =>
            UnitPrice * Quantity;

    }
}
