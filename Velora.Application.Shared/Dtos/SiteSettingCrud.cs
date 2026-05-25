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
    public class SiteSettingCrud : BulkInsert {

        [Key]
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]
        public string SiteName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]
        public string? DomainName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? LogoUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]
        public string? LogoAlt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? DarkLogoUrl { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]
        public string? DarkLogoAlt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? FaviconUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? PhoneTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]
        public string? Phone { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? Phone2Title { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]
        public string? Phone2 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? MobileTitle { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]
        public string? Mobile { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? FaxTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]

        public string? Fax { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]
        public string? Email { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? AddressTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]
        public string? Address { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 19, GridOrder = 19, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? Address2Title { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 20, GridOrder = 20, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]
        public string? Address2 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 21, GridOrder = 21, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]

        public string? DefaultMetaTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 22, GridOrder = 22, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]
        public string? DefaultMetaDescription { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 23, GridOrder = 23, ShowInGrid = true, ShowInForm = true, MaxLength = 1000)]

        public string? DefaultMetaKeywords { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 24, GridOrder = 24, ShowInGrid = true, ShowInForm = true)]

        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 25, GridOrder = 25, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 26, GridOrder = 26, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 27, GridOrder = 27, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 28, GridOrder = 28, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }

    }
}
