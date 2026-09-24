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
    public class OrderManagementCrud : BulkInsert
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]

        public string? OrderCode { get; set; }

        public Guid? UserId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder =2, GridOrder =2, ShowInGrid = true, ShowInForm = true)]
        public DateTime? OrderedAt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 19)]
        public string? OrderedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]

        public int? OrderStatus { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 13)]

        public string OrderStatusTitle { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder =6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 201)]
        public string? BuyerName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder =7, GridOrder = 7, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? ReceiverFirstName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]

        public string? ReceiverLastName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 201)]

        public string ReceiverFullName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true, MaxLength = 20)]

        public string? ReceiverNationalCode { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, MaxLength = 20)]

        public string? ReceiverPhone { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]

        public string? AddressText { get; set; }

        public Guid? ShippingMethodId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]

        public string? ShippingMethodName { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true)]
        public decimal ShippingPrice { get; set; }

        public Guid? CouponId { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? CouponCode { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true)]
        public decimal? CouponDiscountAmount { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = true)]
        public decimal? TaxAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true)]
        public decimal? DutyAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 19, GridOrder = 19, ShowInGrid = true, ShowInForm = true)]
        public decimal FinalAmount { get; set; }

        public Guid? PaymentId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 20, GridOrder = 20, ShowInGrid = true, ShowInForm = true)]
        public bool? HasPayment { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 21, GridOrder = 21, ShowInGrid = true, ShowInForm = true)]
        public int? PaymentMethod { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 22, GridOrder = 22, ShowInGrid = true, ShowInForm = true, MaxLength = 13)]

        public string PaymentMethodTitle { get; set; } = null!;

        public int? PaymentStatus { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 23, GridOrder = 23, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]

        public string PaymentStatusTitle { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 24, GridOrder =24, ShowInGrid = true, ShowInForm = true)]

        public decimal? PaymentAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 25, GridOrder = 25, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]

        public string? GatewayTrackingCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 26, GridOrder = 26, ShowInGrid = true, ShowInForm = true)]

        public DateTime? PaidAt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 27, GridOrder =27, ShowInGrid = true, ShowInForm = true, MaxLength = 19)]

        public string? PaidAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 28, GridOrder = 28, ShowInGrid = true, ShowInForm = true)]

        public int? LastPaymentStatusOldStatus { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 29, GridOrder = 29, ShowInGrid = true, ShowInForm = true)]
        public int? LastPaymentStatusNewStatus { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 30, GridOrder = 30, ShowInGrid = true, ShowInForm = true)]
        public DateTime? LastPaymentStatusChangeAt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 31, GridOrder = 31, ShowInGrid = true, ShowInForm = true,MaxLength =19)]
        public string? LastPaymentStatusChangeAtPersian { get; set; }

        public Guid? LastPaymentStatusChangedById { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder =32, GridOrder =32, ShowInGrid = true, ShowInForm = true, MaxLength = 201)]

        public string? LastPaymentStatusChangedBy { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 33, GridOrder =33, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]

        public string? LastPaymentStatusDescription { get; set; }


    }
}
