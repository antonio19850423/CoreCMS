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
    public class SmsLogCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = true,
           ServiceName = "",
           LinkedFieldCode = "ProviderName",
           Route = "/api/ComboBox/SmsProviders")]
        public int? Provider { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = false,
                ServiceName = "",
                LinkedFieldCode = "Provider",
                Route = "/api/ComboBox/SmsProviders")]
        public string ProviderName { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false, MaxLength = 50)]
        public string SmsType { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = false, MaxLength = 1000)]
        public string Message { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 4, GridOrder =4, ShowInGrid = true, ShowInForm = false)]
        public bool IsSuccess { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = false, MaxLength = 50)]
        public string Mobile { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false, MaxLength = 200)]
        public string? ProviderMessageId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder =7, GridOrder = 7, ShowInGrid = true, ShowInForm = false, MaxLength = 1000)]
        public string? ErrorMessage { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = false)]
        public string SentAtPersian { get; set; }

    }
}
