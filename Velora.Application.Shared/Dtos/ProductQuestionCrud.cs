using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
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
    public class ProductQuestionCrud : BulkInsert
    {

        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.HiddenText, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true)]
        public Guid ProductId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = true)]
        public string? ProductTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true)]
        public string? ProductSlug { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]
        public string Question { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]
        public string? Answer { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 6, GridOrder =6, ShowInGrid = true, ShowInForm = true)]
        public bool IsAnswered { get; set; }
        [ResourceColumn(FieldType = FieldTypes.HiddenText, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public Guid? AnsweredBy { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true)]
        public bool IsApproved { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true)]
        public string AnsweredName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.HiddenText, IsRequired = false, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true)]
        public Guid? UserId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true)]
        public string UserName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }

    }
}
