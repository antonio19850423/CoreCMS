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
    public  class PageTemplateComponentCrud : BulkInsert
    {
        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.PageTemplate, ServiceName = "pageTemplateView", LinkedFieldCode = "PageTemplateName", SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid PageTemplateId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.PageTemplate, ServiceName = "pageTemplateView", LinkedFieldCode = "PageTemplateId", SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? PageTemplateName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeName", SelectDisplayFields = "[\"name\",\"code\"]")]

        public Guid ComponentTypeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeId", SelectDisplayFields = "[\"name\",\"code\"]")]

        public string? ComponentTypeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]
        public string ComponentVariant { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public bool IsEditable { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
