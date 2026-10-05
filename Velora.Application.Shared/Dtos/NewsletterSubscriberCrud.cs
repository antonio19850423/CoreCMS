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
    public class NewsletterSubscriberCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = true, MaxLength = 320)]
        public string Email { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true)]
        public bool IsConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }
    }
}
