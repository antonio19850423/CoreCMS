using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public  class MyOrderDetailQuery
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string? OrderCode { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? OrderedAt { get; set; }

        [StringLength(19)]
        public string? OrderedAtPersian { get; set; }

        public int? OrderStatus { get; set; }

        [StringLength(13)]
        public string OrderStatusTitle { get; set; } = null!;

        [StringLength(201)]
        public string? BuyerName { get; set; }

        [StringLength(100)]
        public string? ReceiverFirstName { get; set; }

        [StringLength(100)]
        public string? ReceiverLastName { get; set; }

        [StringLength(201)]
        public string ReceiverFullName { get; set; } = null!;

        [StringLength(20)]
        public string? ReceiverNationalCode { get; set; }

        [StringLength(20)]
        public string? ReceiverPhone { get; set; }

        public Guid? AddressId { get; set; }

        [StringLength(1000)]
        public string? AddressText { get; set; }

        public Guid? ShippingMethodId { get; set; }

        [StringLength(200)]
        public string? ShippingMethodName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ShippingPrice { get; set; }

        public Guid? CouponId { get; set; }

        [StringLength(100)]
        public string? CouponCode { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CouponDiscountAmount { get; set; }

        public bool? CouponUsed { get; set; }

        public Guid? CouponUsageId { get; set; }

        public DateTime? CouponUsedAt { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TaxAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DutyAmount { get; set; }

        public Guid PaymentId { get; set; }

        public int PaymentMethod { get; set; }

        [StringLength(13)]
        public string PaymentMethodTitle { get; set; } = null!;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PaymentAmount { get; set; }

        public int PaymentStatus { get; set; }

        [StringLength(16)]
        public string PaymentStatusTitle { get; set; } = null!;

        public Guid? GatewayId { get; set; }

        [StringLength(200)]
        public string? GatewayTransactionId { get; set; }

        [StringLength(200)]
        public string? GatewayTrackingCode { get; set; }

        public Guid? BankAccountId { get; set; }

        [StringLength(500)]
        public string? ReceiptFile { get; set; }

        public DateTime? PaidAt { get; set; }

        [StringLength(19)]
        public string? PaidAtPersian { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal FinalAmount { get; set; }

        public DateTime? OrderPaidAt { get; set; }

        [StringLength(19)]
        public string? OrderPaidAtPersian { get; set; }

        public Guid ShoppingCartItemId { get; set; }

        public Guid ProductId { get; set; }

        public Guid? VariantId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

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

        [Column(TypeName = "decimal(29, 2)")]
        public decimal? ItemFinalAmount { get; set; }

        [StringLength(200)]
        public string? ProductName { get; set; }

        [StringLength(300)]
        public string? ProductImage { get; set; }

        [StringLength(150)]
        public string? VariantName { get; set; }
    }
}
