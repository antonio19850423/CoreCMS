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
    public class SectionItemCrud : BulkInsert
    {
        public Guid Id { get; set; }

        // ===== Main Content =====
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 250)]
        public string? Title { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder =2, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Subtitle { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 600)]
        public string? Description { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string Price { get; set; }

        // ===== Media =====
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder =5, GridOrder = 5, ShowInGrid = false, ShowInForm = true, MaxLength = 512)]
        public string? ImageUrl { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ImageAlt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, MaxLength = 300)]
        public string? AvatarUrl { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? AvatarAlt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]

        public string? Icon { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 10, GridOrder = 10, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]

        public string? IconAlt { get; set; }

        // ===== Links =====
        // Links
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link1", Route = "/api/ComboBox/Pages")]
        public string? Link1Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link1")]
        public string? Link1Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link1")]
        public string? Link1Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true, GroupKey = "Link1")]
        public Guid? Link1TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true, GroupKey = "Link1")]
        public Guid? Link1TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true, GroupKey = "Link1")]
        public bool? Link1OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link2", Route = "/api/ComboBox/Pages")]
        public string? Link2Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link2")]
        public string? Link2Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link2")]
        public string? Link2Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = true, GroupKey = "Link2")]
        public Guid? Link2TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = true, GroupKey = "Link2")]
        public Guid? Link2TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = true, GroupKey = "Link2")]
        public bool? Link2OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link3", Route = "/api/ComboBox/Pages")]
        public string? Link3Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link3")]
        public string? Link3Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 13, GridOrder = 13, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link3")]
        public string? Link3Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 13, GridOrder = 13, ShowInGrid = false, ShowInForm = true, GroupKey = "Link3")]
        public Guid? Link3TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 13, GridOrder = 13, ShowInGrid = false, ShowInForm = true, GroupKey = "Link3")]
        public Guid? Link3TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 13, GridOrder = 13, ShowInGrid = false, ShowInForm = true, GroupKey = "Link3")]
        public bool? Link3OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link4", Route = "/api/ComboBox/Pages")]
        public string? Link4Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link4")]
        public string? Link4Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link4")]
        public string? Link4Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, GroupKey = "Link4")]
        public Guid? Link4TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, GroupKey = "Link4")]
        public Guid? Link4TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, GroupKey = "Link4")]
        public bool? Link4OpenInNewTab { get; set; }

        // ===== Display Settings =====
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 19, GridOrder = 19, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]


        public string? BackgroundColor { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 20, GridOrder = 20, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]

        public string? TitleColor { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 21, GridOrder = 21, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]

        public string? SubtitleColor { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 22, GridOrder = 22, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]

        public string? DescriptionColor { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 23, GridOrder = 23, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]

        public string? IconColor { get; set; }


        // ===== Settings =====
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 28, GridOrder = 28, ShowInGrid = false, ShowInForm = true,IsRequired =true)]

        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 29, GridOrder = 29, ShowInGrid = false, ShowInForm = true)]

        public bool IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 30, GridOrder = 30, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }

        // ===== Audit =====
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 30, GridOrder = 30, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 31, GridOrder = 31, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 32, GridOrder = 32, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 33, GridOrder = 33, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 34, GridOrder = 34, ShowInGrid = false, ShowInForm = true,MaxLength =2000)]
        public string? Features { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 35, GridOrder = 35, ShowInGrid = true, ShowInForm = false, MaxLength = 100)]
        public string? ComponentTypeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 36, GridOrder = 36, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.SectionGroupItem, ServiceName = "sectionGroupItemView", LinkedFieldCode = "SectionGroupItemName", Route = "/api/ComboBox/SectionGroupItems", SelectDisplayFields = "[\"label\",\"code\"]")]
        public Guid? SectionGroupItemId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 37, GridOrder = 37, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.SectionGroupItem, ServiceName = "sectionGroupItemView", LinkedFieldCode = "SectionGroupItemId", Route = "/api/ComboBox/SectionGroupItems", SelectDisplayFields = "[\"label\",\"code\"]")]
        public string? SectionGroupItemName { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 38, GridOrder = 38, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? Name { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 39, GridOrder = 39, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? Role { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 40, GridOrder = 40, ShowInGrid = false, ShowInForm = true, MaxLength = 500)]
        public string? Question { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 41, GridOrder = 41, ShowInGrid = false, ShowInForm = true, MaxLength = 600)]
        public string? Answer { get; set; }
    }
}
