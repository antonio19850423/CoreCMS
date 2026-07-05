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
    public class SectionCrud : BulkInsert
    {
        public Guid? Id { get; set; }

        // Basic info
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? Subtitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 250)]
        public string? Title { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? Description { get; set; }

        // Image
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 512)]
        public string? ImageUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]
        public string? ImageAlt { get; set; }

        // Links
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 100,GroupKey = "Link1",Route = "/api/ComboBox/Pages")]
        public string? Link1Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link1")]
        public string? Link1Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link1")]
        public string? Link1Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, GroupKey = "Link1")]
        public Guid? Link1TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, GroupKey = "Link1")]
        public Guid? Link1TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, GroupKey = "Link1")]
        public bool? Link1OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link2", Route = "/api/ComboBox/Pages")]
        public string? Link2Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link2")]
        public string? Link2Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link2")]
        public string? Link2Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, GroupKey = "Link2")]
        public Guid? Link2TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, GroupKey = "Link2")]
        public Guid? Link2TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, GroupKey = "Link2")]
        public bool? Link2OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link3", Route = "/api/ComboBox/Pages")]
        public string? Link3Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link3")]
        public string? Link3Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link3")]
        public string? Link3Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true, GroupKey = "Link3")]
        public Guid? Link3TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true, GroupKey = "Link3")]
        public Guid? Link3TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true, GroupKey = "Link3")]
        public bool? Link3OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 100, GroupKey = "Link4", Route = "/api/ComboBox/Pages")]
        public string? Link4Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 300, GroupKey = "Link4")]
        public string? Link4Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true, MaxLength = 50, GroupKey = "Link4")]
        public string? Link4Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true, GroupKey = "Link4")]
        public Guid? Link4TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true, GroupKey = "Link4")]
        public Guid? Link4TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true, GroupKey = "Link4")]
        public bool? Link4OpenInNewTab { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]
        public string? Icon { get; set; }
        // Icon
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]
        public string? IconAlt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 19, GridOrder = 19, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? IconColor { get; set; }

        // Colors (hidden)
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 20, GridOrder = 20, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? BackgroundColor { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 21, GridOrder = 21, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? HeaderColor { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 22, GridOrder = 22, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? SubtitleColor { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 23, GridOrder = 23, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? DescriptionColor { get; set; }

        // Columns & component type
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 24, GridOrder = 24, ShowInGrid = true, ShowInForm = true)]
        public int? ColumnsCount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 25, GridOrder = 25, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeName", SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid ComponentTypeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 26, GridOrder = 26, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeId", SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? ComponentTypeName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 27, GridOrder = 27, ShowInGrid = false, ShowInForm = true, MaxLength = 500)]

        public string? VideoUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 28, GridOrder = 28, ShowInGrid = false, ShowInForm = true, MaxLength = 500)]
        public string? ThumbnailUrl { get; set; }

        // Status & sorting
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 29, GridOrder = 29, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 30, GridOrder = 30, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 31, GridOrder = 31, ShowInGrid = false, ShowInForm = true)]
        public Guid ParentId { get; set; }

        // Audit info
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 32, GridOrder = 32, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 33, GridOrder = 33, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 34, GridOrder = 34, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 35, GridOrder = 35, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 36, GridOrder = 36, ShowInGrid = false, ShowInForm = true, MaxLength = 2000)]
        public string? Features { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 37, GridOrder = 37, ShowInGrid = false, ShowInForm = true, MaxLength = 350)]
        public string? CopyrightText { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 38, GridOrder = 38, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ContactFirstNameLabel { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 39, GridOrder = 39, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ContactLastNameLabel { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 40, GridOrder = 40, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ContactEmailLabel { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 41, GridOrder = 41, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ContactMessageLabel { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 42, GridOrder = 42, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ContactSubmitButtonText { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 43, GridOrder = 43, ShowInGrid = false, ShowInForm = true, MaxLength = 512)]
        public string? ImageUrl2 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 44, GridOrder = 44, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ImageAlt2 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 45, GridOrder = 45, ShowInGrid = false, ShowInForm = true, MaxLength = 512)]
        public string? ImageUrl3 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 46, GridOrder = 46, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ImageAlt3 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 47, GridOrder = 47, ShowInGrid = false, ShowInForm = true, MaxLength = 512)]
        public string? ImageUrl4 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 48, GridOrder = 48, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? ImageAlt4 { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 49, GridOrder = 49, ShowInGrid = false, ShowInForm = true, MaxLength = 500)]
        public string? MapEmbedUrl { get; set; }

    }
}
