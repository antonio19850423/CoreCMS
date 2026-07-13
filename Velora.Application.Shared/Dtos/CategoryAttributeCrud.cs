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
    public class CategoryAttributeCrud : BulkInsert {

        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, EntityName = LookupEntities.ProductCategory, ServiceName = "productAttributeView", LinkedFieldCode = "AttributeName", Route = "/api/ComboBox/ProductAttributes", SelectDisplayFields = "[\"label\",\"code\"]")]

        public Guid AttributeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.ProductCategory, ServiceName = "productAttributeView", LinkedFieldCode = "AttributeId", Route = "/api/ComboBox/ProductAttributes", SelectDisplayFields = "[\"label\",\"code\"]")]

        public string? AttributeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = false)]

        public string? AttributeCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, EntityName = LookupEntities.ProductAttribute, ServiceName = "productCategoryView", LinkedFieldCode = "CategoryName", Route = "/api/ComboBox/ProductCategories", SelectDisplayFields = "[\"label\",\"code\"]")]

        public Guid CategoryId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.ProductAttribute, ServiceName = "productCategoryView", LinkedFieldCode = "CategoryId", Route = "/api/ComboBox/ProductCategories", SelectDisplayFields = "[\"label\",\"code\"]")]

        public string? CategoryName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = false, ShowInForm = false)]
        public string? CategorySlug { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder =3, GridOrder = 3, ShowInGrid = false, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
