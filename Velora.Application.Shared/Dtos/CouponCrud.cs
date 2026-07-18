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


        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, EntityName = LookupEntities.Discount, ServiceName = "discountView", LinkedFieldCode = "ParentName", Route = "/api/ComboBox/Discounts", SelectDisplayFields = "[\"label\",\"name\"]")]
        public Guid? ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.Discount, ServiceName = "discountView", LinkedFieldCode = "ParentId", Route = "/api/ComboBox/Discounts", SelectDisplayFields = "[\"label\",\"name\"]")]
        public string? ParentName { get; set; }

        [StringLength(100)]
        public string Code { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true)]

        public DateTime StartDate { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = false, MaxLength = 19)]
        public string? StartDatePersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = true)]
        public DateTime EndDate { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false, MaxLength = 19)]
        public string? EndDatePersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public bool IsSingleUsePerUser { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true)]
        public int? UsageLimit { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true)]
        public int UsedCount { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 10, GridOrder = 10 , ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }

    }
}
