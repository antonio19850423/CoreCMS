using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public class DiscountItemCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 1, GridOrder = 1, ShowInGrid = false, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.Discount, ServiceName = "discountView", LinkedFieldCode = "ParentName", Route = "/api/ComboBox/Discounts", SelectDisplayFields = "[\"label\",\"name\"]")]
        public Guid? ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.Discount, ServiceName = "discountView", LinkedFieldCode = "ParentId", Route = "/api/ComboBox/Discounts", SelectDisplayFields = "[\"label\",\"name\"]")]
        public string? ParentName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductBrand,
            ServiceName = "productBrandView",
            LinkedFieldCode = "ProductBrandName",
            Route = "/api/ComboBox/ProductBrands",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public Guid? ProductBrandId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.ProductBrand,
            ServiceName = "productBrandView",
            LinkedFieldCode = "ProductBrandId",
            Route = "/api/ComboBox/ProductBrands",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? ProductBrandName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductCategory,
            ServiceName = "productCategoryView",
            LinkedFieldCode = "ProductCategoryName",
            Route = "/api/ComboBox/ProductCategories",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public Guid? ProductCategoryId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.ProductCategory,
            ServiceName = "productCategoryView",
            LinkedFieldCode = "ProductCategoryId",
            Route = "/api/ComboBox/ProductCategories",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? ProductCategoryName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.Product,
            ServiceName = "productView",
            LinkedFieldCode = "ProductName",
            Route = "/api/ComboBox/Products",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public Guid ProductId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.Product,
            ServiceName = "productView",
            LinkedFieldCode = "ProductId",
            Route = "/api/ComboBox/Products",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? ProductName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = true,
          EntityName = LookupEntities.ProductVariant,
          ServiceName = "productVariantView",
          LinkedFieldCode = "ProductVariantName",
          Route = "/api/ComboBox/ProductVariants",
           SelectDisplayFields = "[\"productName\",\"name\",\"price\"]")]
        public Guid? ProductVariantId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = false,
           EntityName = LookupEntities.ProductVariant,
           ServiceName = "productVariantView",
           LinkedFieldCode = "ProductVariantId",
           Route = "/api/ComboBox/ProductVariants",
           SelectDisplayFields = "[\"productName\",\"name\",\"price\"]")]
        public string? ProductVariantName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
