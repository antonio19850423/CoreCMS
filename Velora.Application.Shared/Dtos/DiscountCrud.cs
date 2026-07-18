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
    public class DiscountCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = false, MaxLength = 200)]
        public string Name { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = true,
            ServiceName = "",
            LinkedFieldCode = "DiscountTypeName",
            Route = "/api/ComboBox/DiscountTypes")]
        public byte DiscountType { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false,
            ServiceName = "",
            LinkedFieldCode = "DiscountType",
            Route = "/api/ComboBox/DiscountTypes")]
        public string DiscountTypeName { get; set; } = null!;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountValue { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true)]
        public DateTime StartDate { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = true)]
        public DateTime EndDate { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = false,MaxLength =19)]
        public string? StartDatePersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false, MaxLength = 19)]
        public string? EndDatePersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
