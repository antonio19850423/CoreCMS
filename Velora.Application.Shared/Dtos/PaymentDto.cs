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
    public class PaymentDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? ShoppingCartId { get; set; }

        public int PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public int PaymentStatus { get; set; }

        public Guid? GatewayId { get; set; }

        [StringLength(200)]
        public string? GatewayTransactionId { get; set; }

        [StringLength(200)]
        public string? GatewayTrackingCode { get; set; }

        public Guid? BankAccountId { get; set; }

        [StringLength(500)]
        public string? ReceiptFile { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
