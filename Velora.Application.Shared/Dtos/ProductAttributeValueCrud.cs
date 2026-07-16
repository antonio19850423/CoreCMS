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
    public class ProductAttributeValueCrud : BulkInsert
    {
        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductAttribute,
            ServiceName = "productAttributeView",
            LinkedFieldCode = "ProductAttributeName",
            Route = "/api/ComboBox/ProductAttributes",
            SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid ProductAttributeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false,
           EntityName = LookupEntities.ProductAttribute,
           ServiceName = "productAttributeView",
           LinkedFieldCode = "ProductAttributeId",
           Route = "/api/ComboBox/ProductAttributes",
           SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? ProductAttributeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 1000, IsRequired = true)]
        public string Value { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = false)]
        public string? ProductAttributeCode { get; set; }

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
