using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public class PaymentCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public Guid? BankAccountId { get; set; }

        public Guid? GatewayId { get; set; }

        [StringLength(100)]
        public string? PaymentGatewayName { get; set; }

        [StringLength(200)]
        public string? GatewayTrackingCode { get; set; }

        [StringLength(200)]
        public string? GatewayTransactionId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public int PaymentMethod { get; set; }

        public int PaymentStatus { get; set; }

        [StringLength(13)]
        public string PaymentMethodName { get; set; } = null!;

        [StringLength(16)]
        public string PaymentStatusName { get; set; } = null!;

        [StringLength(500)]
        public string? ReceiptFile { get; set; }

        public Guid? ShoppingCartId { get; set; }

        [StringLength(19)]
        public string? CreatedAtPersian { get; set; }

        [StringLength(19)]
        public string? UpdatedAtPersian { get; set; }

        [StringLength(201)]
        public string? CreatedByName { get; set; }

        [StringLength(201)]
        public string? UpdatedByName { get; set; }

        [StringLength(50)]
        public string? OrderCode { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? FinalAmount { get; set; }

        public DateTime? OrderedAt { get; set; }

        [StringLength(1000)]
        public string? AddressText { get; set; }

        [StringLength(200)]
        public string? ShippingMethodName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ShippingPrice { get; set; }

        [StringLength(200)]
        public string? CartToken { get; set; }

        [StringLength(100)]
        public string? CouponCode { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CouponDiscountAmount { get; set; }

        public Guid? CouponId { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? ReceiverFirstName { get; set; }

        [StringLength(20)]
        public string? ReceiverNationalCode { get; set; }

        [StringLength(20)]
        public string? ReceiverPhone { get; set; }

        public Guid? ShippingMethodId { get; set; }

        public int? Status { get; set; }
    }
}
