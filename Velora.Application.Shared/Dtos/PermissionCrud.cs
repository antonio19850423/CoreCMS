using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public class PermissionCrud : BulkInsert

    {
        public Guid? Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.Resource, ServiceName = "resourceView", LinkedFieldCode = "ResourceName", SelectDisplayFields = "[\"code\",\"displayName\"]")]
        public Guid? ResourceId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = false, EntityName = LookupEntities.Resource, ServiceName = "resourceView", LinkedFieldCode = "ResourceId", SelectDisplayFields = "[\"code\",\"displayName\"]")]
        public string? ResourceName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false)]
        public string? ResourceCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = false)]
        public string? ResourceTypeCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.ComboBox, IsRequired = true, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, EntityName = LookupEntities.Permission, Route = "/api/ComboBox/permissions")]
        public int? Actions { get; set; }

        [ResourceColumn(FieldType = FieldTypes.MultiSelectBox, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.Role, ServiceName = "roleView", LinkedFieldCode = "RoleNames", Route = "/api/ComboBox/roles", SelectDisplayFields = "[\"code\",\"name\"]")]
        public string RoleIds { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = false, EntityName = LookupEntities.Role, ServiceName = "roleView", LinkedFieldCode = "RoleIds", Route = "/api/ComboBox/roles", SelectDisplayFields = "[\"code\",\"name\"]")]
        public string? RoleNames { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true)]
        public bool? IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false)]
        public DateTime? CreatedAt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false)]
        public DateTime? UpdatedAt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
