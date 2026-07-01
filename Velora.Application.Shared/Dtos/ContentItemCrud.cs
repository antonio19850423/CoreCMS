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
    public class ContentItemCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 250)]
        public string Title { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? Summary { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true, MaxLength = 900)]
        public string? Content { get; set; }
        public string ContentType { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = true, MaxLength = 300)]
        public string? AuthorAvatarUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? AuthorName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, MaxLength = 150)]
        public string? AuthorTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, MaxLength = 500)]
        public string? ExternalUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true, MaxLength = 512)]
        public string? ImageUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true, MaxLength = 250)]
        public string? ImageAlt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 10, GridOrder = 10, ShowInGrid = false, ShowInForm = true)]

        public DateTime? PublishedAt { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true, MaxLength = 500)]
        public string? Tags { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ContentCategory,Route = "/api/ComboBox/ContentCategories", ServiceName = "contentCategoryView", LinkedFieldCode = "CategoryName")]
        public Guid? CategoryId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 13, GridOrder = 13, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.ContentCategory, Route = "/api/ComboBox/ContentCategories", ServiceName = "contentCategoryView", LinkedFieldCode = "CategoryId")]

        public string? CategoryName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, MaxLength = 200)]
        public string? CategorySlug { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 17, GridOrder = 17, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 19, GridOrder = 19, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 20, GridOrder = 20, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 21, GridOrder = 21, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
