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
    public class BankAccountCrud : BulkInsert
    {
        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true,MaxLength =200)]
        public string AccountOwnerName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]
        public string? Description { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string BankName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]
        public string? CardNumber { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? AccountNumber { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]
        public string? ShebaNumber { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true)]
        public bool IsDefault { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true)]
        public bool IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 10, GridOrder = 10, ShowInGrid = false, ShowInForm = true)]
        public int DisplayOrder { get; set; }

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
