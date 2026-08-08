using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
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
    public class CouponCrud : BulkInsert
    {
        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = true,
                ServiceName = "",
                LinkedFieldCode = "CouponTypeName",
                Route = "/api/ComboBox/CouponTypes")]
        public byte CouponType { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = false,
                ServiceName = "",
                LinkedFieldCode = "CouponType",
                Route = "/api/ComboBox/CouponTypes")]
        public string CouponTypeName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 2, GridOrder = 2, IsRequired =true,ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public decimal CouponValue { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public decimal? MinimumOrderAmount { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public decimal? MaximumDiscountAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 5, GridOrder = 5, IsRequired = true, ShowInGrid = false, ShowInForm = true)]
        public DateTime StartDate { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false, MaxLength = 19)]
        public string? StartDatePersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 7, GridOrder = 7, IsRequired = true, ShowInGrid = false, ShowInForm = true)]
        public DateTime EndDate { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false, MaxLength = 19)]
        public string? EndDatePersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, IsRequired = true, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string Code { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public bool? CanCombineWithDiscount { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public bool IsSingleUsePerUser { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public int? UsageLimit { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public int UsedCount { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }

    }
}
