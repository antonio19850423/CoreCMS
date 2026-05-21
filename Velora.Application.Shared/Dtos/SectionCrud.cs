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
    public class SectionCrud : BulkInsert {
        public Guid? Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? Subtitle { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 250)]
        public string? Title { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true,MaxLength =100)]
        public string? ButtonText { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? ButtonUrl { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]
        public int? ColumnsCount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeName", SelectDisplayFields = "[\"name\",\"code\"]")]

        public Guid ComponentTypeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.ComponentType, ServiceName = "componentTypeView", LinkedFieldCode = "ComponentTypeId", SelectDisplayFields = "[\"name\",\"code\"]")]

        public string? ComponentTypeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? ImageUrl { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 500)]
        public string? Description { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 11, GridOrder =11, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = true)]
        public Guid ParentId { get; set; }


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
