using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public class PaymentStatusLogCrud : BulkInsert
    {
        public Guid Id { get; set; }

        public int NewStatus { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? NewStatusTitle { get; set; }

        public int? OldStatus { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]

        public string? OldStatusTitle { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 19)]
        public string? CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 201)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }
    }
}
