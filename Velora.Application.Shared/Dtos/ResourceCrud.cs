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
    public  class ResourceCrud: BulkInsert
    {
        public Guid? Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 100, IsRequired = true, FormOrder = 0, GridOrder = 0, ShowInGrid = true, ShowInForm = true,ShowInSelectBox =true)]
        public string? Code { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 250, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInSelectBox = true)]
        public string? DisplayName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ResourceType, ServiceName = "resourceTypeView", LinkedFieldCode = "ResourceTypeTitle", Route = "/api/ComboBox/resourceTypes", SelectDisplayFields = "[\"code\",\"name\"]")]
        public Guid? ResourceTypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.ResourceType, ServiceName = "resourceTypeView", LinkedFieldCode = "ResourceTypeId", Route = "/api/ComboBox/resourceTypes", ShowInSelectBox = true,SelectDisplayFields = "[\"code\",\"name\"]")]
        public string? ResourceTypeTitle { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, MaxLength = 300, IsRequired = true, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true)]
        public string? Description { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public bool? IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]
        public int? Order { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 50, IsRequired = true, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true)]
        public string? FieldType { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = true, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public int? MaxLength { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true)]
        public bool? IsRequired { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true)]
        public bool? ShowInForm { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true)]
        public bool? ShowInGrid { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true)]
        public bool? ShowInSelectBox { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = true, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true)]
        public int? FormOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = true, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true)]
        public int? GridOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = true, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true)]
        public int? SelectBoxOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 255, IsRequired = false, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = true)]
        public string? Route { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 20, IsRequired = false, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true)]

        public string? InputMask { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = true)]
        public bool? IsDynamicForm { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 200, IsRequired = false, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true)]
        public string? LinkedFieldCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 100, IsRequired = false, FormOrder = 19, GridOrder = 19, ShowInGrid = true, ShowInForm = true)]
        public string? EntityName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 100, IsRequired = false, FormOrder = 20, GridOrder = 20, ShowInGrid = true, ShowInForm = true)]
        public string? ServiceName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 21, GridOrder = 21, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.Resource, ServiceName = "resourceView", LinkedFieldCode = "ParentDisplayName", SelectDisplayFields = "[\"code\",\"displayName\"]")]
        public Guid? ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 21, GridOrder = 21, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.Resource, ServiceName = "resourceView", LinkedFieldCode = "ParentId",SelectDisplayFields = "[\"code\",\"displayName\"]")]
        public string? ParentDisplayName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false,MaxLength =300, FormOrder = 22, GridOrder = 22, ShowInGrid = false, ShowInForm = true)]
        public string? SelectDisplayFields { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, MaxLength = 200, FormOrder = 23, GridOrder = 23, ShowInGrid = false, ShowInForm = true)]
        public string? GroupKey { get; set; }
    }
}
