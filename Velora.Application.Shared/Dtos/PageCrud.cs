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
    public partial class PageCrud
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? CanonicalUrl { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 500, IsRequired=true)]
        public string? MetaDescription { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 500, IsRequired = true)]
        public string? MetaKeywords { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 200, IsRequired = true)]
        public string? MetaTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 150, IsRequired = true)]
        public string Name { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? OgImageUrl { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.PageTemplate, ServiceName = "pageTemplateView", LinkedFieldCode = "PageTemplateName", SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid? PageTemplateId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.PageTemplate, ServiceName = "pageTemplateView", LinkedFieldCode = "PageTemplateId", SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? PageTemplateName { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder =9, ShowInGrid = true, ShowInForm = true, MaxLength = 200, IsRequired = true)]
        public string Slug { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true)]
        public bool IsHome { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true)]
        public bool? IsDynamic { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true)]
        public bool IsPublished { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
