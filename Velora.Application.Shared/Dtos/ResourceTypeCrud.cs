using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public class ResourceTypeCrud: BulkInsert
    {
        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 50, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInSelectBox = true, SelectBoxOrder = 1)]
        public string? Code { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 50, IsRequired = true, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, ShowInSelectBox = true, SelectBoxOrder = 2)]
        public string? Name { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 200, IsRequired = true, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, ShowInSelectBox = true, SelectBoxOrder = 3)]
        public string? DisplayName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 300, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, ShowInSelectBox = true, SelectBoxOrder = 4)]
        public string? Description { get; set; }
        public bool IsTest { get; set; } = false;
    }

}
