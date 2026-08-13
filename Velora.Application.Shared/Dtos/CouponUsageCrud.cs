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
    public class CouponUsageCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true)]
        public string CouponTypeName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true)]
        public decimal? CouponValue { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true)]
        public string? Code { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = false)]
        public Guid OrderId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = false)]
        public DateTime UsedAt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = false)]
        public Guid UserId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public DateTime? CouponDiscountAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public decimal? FinalAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public string? UsedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public string? UsedByName { get; set; }

    }
}
