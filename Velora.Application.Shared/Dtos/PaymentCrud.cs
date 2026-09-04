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

        // =========================
        // Order Information
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 1,
            GridOrder = 1,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 50)]
        public string? OrderCode { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 2,
            GridOrder = 2,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 19)]
        public string? CreatedAtPersian { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 3,
            GridOrder = 3,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 201)]
        public string? UserFullName { get; set; }

        // =========================
        // Receiver Information
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 4,
            GridOrder = 4,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 100)]
        public string? ReceiverFirstName { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 5,
            GridOrder = 5,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 100)]
        public string? ReceiverLastName { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 6,
            GridOrder = 6,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 20)]
        public string? ReceiverNationalCode { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 7,
            GridOrder = 7,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 20)]
        public string? ReceiverPhone { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 8,
            GridOrder = 8,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 1000)]
        public string? AddressText { get; set; }

        // =========================
        // Order Amount
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Currency,
            FormOrder = 9,
            GridOrder = 9,
            ShowInGrid = true,
            ShowInForm = true)]
        public decimal FinalAmount { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 10,
            GridOrder = 10,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 100)]
        public string? CouponCode { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Currency,
            FormOrder = 11,
            GridOrder = 11,
            ShowInGrid = true,
            ShowInForm = true)]
        public decimal? CouponDiscountAmount { get; set; }

        // =========================
        // Shipping
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 12,
            GridOrder = 12,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 200)]
        public string? ShippingMethodName { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Currency,
            FormOrder = 13,
            GridOrder = 13,
            ShowInGrid = true,
            ShowInForm = true)]
        public decimal ShippingPrice { get; set; }

        // =========================
        // Payment Information
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Currency,
            FormOrder = 14,
            GridOrder = 14,
            ShowInGrid = true,
            ShowInForm = true)]
        public decimal? PaymentAmount { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 15,
            GridOrder = 15,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 13)]
        public string PaymentMethodTitle { get; set; } = null!;

        [ResourceColumn(
            FieldType = FieldTypes.Autocomplete,
            IsRequired = true,
            FormOrder = 16,
            GridOrder = 16,
            ShowInGrid = false,
            ShowInForm = true,
            EntityName = LookupEntities.PaymentStatus,
            Route = "/api/ComboBox/PaymentStatuses",
            LinkedFieldCode = "PaymentStatusTitle")]
        public int? PaymentStatus { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Autocomplete,
            IsRequired = false,
            FormOrder = 17,
            GridOrder = 17,
            ShowInGrid = true,
            ShowInForm = false,
            EntityName = LookupEntities.PaymentStatus,
            Route = "/api/ComboBox/PaymentStatuses",
            LinkedFieldCode = "PaymentStatus")]
        public string PaymentStatusTitle { get; set; } = null!;

        // =========================
        // Payment Receipt / Gateway
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Image,
            FormOrder = 18,
            GridOrder = 18,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 16)]
        public string? ReceiptFile { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 19,
            GridOrder = 19,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 200)]
        public string? GatewayTrackingCode { get; set; }

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 20,
            GridOrder = 20,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 200)]
        public string? GatewayTransactionId { get; set; }

        // =========================
        // Description
        // =========================

        [ResourceColumn(
            FieldType = FieldTypes.Lable,
            FormOrder = 21,
            GridOrder = 21,
            ShowInGrid = true,
            ShowInForm = true,
            MaxLength = 1000)]
        public string? Description { get; set; }

        // =========================
        // Properties without ResourceColumn
        // =========================

        public DateTime? OrderedAt { get; set; }

        public Guid? CouponId { get; set; }

        public int Status { get; set; }

        public string ShoppingCartStatusTitle { get; set; } = null!;

        public Guid? UserId { get; set; }

        public string? CreatedByName { get; set; }

        public Guid? BankAccountId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? GatewayId { get; set; }

        public DateTime? PaidAt { get; set; }

        public Guid? ShoppingCartId { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public int? PaymentMethod { get; set; }

        public string? PaidAtPersian { get; set; }
    }
}
