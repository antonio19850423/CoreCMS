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
    public class CustomerManagementCrud : BulkInsert
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]

        public string? CustomerName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true)]
        public int? OrderCount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true)]
        public int? SoldQuantity { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public decimal? GrossSalesAmount { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]
        public decimal? ProductDiscountAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true)]
        public decimal? CouponDiscountAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public decimal? TotalDiscountAmount { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 8, GridOrder =8, ShowInGrid = true, ShowInForm = true)]
        public decimal? NetSalesAmount { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true)]
        public decimal? SalesAfterProductDiscount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true)]
        public DateTime? LastOrderAt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true,MaxLength =19)]
        public string? LastOrderAtPersian { get; set; }

    }
}
