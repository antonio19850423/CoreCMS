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
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? Link1Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Link1Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? Link1Color { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? Link2Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Link2Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? Link2Color { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? Link3Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Link3Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? Link3Color { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string? Link4Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Link4Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 17, GridOrder = 17, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? Link4Color { get; set; }
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

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 25, GridOrder = 25, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeName", SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid ComponentTypeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 26, GridOrder = 26, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeId", SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? ComponentTypeName { get; set; }

        // Status & sorting
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 27, GridOrder = 27, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 28, GridOrder = 28, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 29, GridOrder = 29, ShowInGrid = false, ShowInForm = true)]
        public Guid ParentId { get; set; }

        // Audit info
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 30, GridOrder = 30, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 31, GridOrder = 31, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 32, GridOrder = 32, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 33, GridOrder = 33, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
