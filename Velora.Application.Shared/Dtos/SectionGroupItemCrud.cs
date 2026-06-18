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
    public class SectionGroupItemCrud : BulkInsert
    {
        public Guid? Id { get; set; }

        // Basic info
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]
        public string? Code { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]
        public string? Name { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder =3, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Description { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]
        public string? Icon { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, MaxLength = 50)]
        public string? Color { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.SectionGroupItem, ServiceName = "sectionGroupItemView", LinkedFieldCode = "GroupName", Route = "/api/ComboBox/SectionGroupItems", SelectDisplayFields = "[\"name\"]")]
        public Guid GroupId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = false, EntityName = LookupEntities.SectionGroupItem, ServiceName = "sectionGroupItemView", LinkedFieldCode = "GroupId", Route = "/api/ComboBox/SectionGroupItems", ShowInSelectBox = true, SelectDisplayFields = "[\"name\"]")]
        public string? GroupName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 8, GridOrder =8, ShowInGrid = true, ShowInForm = true)]

        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }
    }
}
