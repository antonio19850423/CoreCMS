using Microsoft.AspNetCore.Components.Forms;
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
    public class ProductCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 200, IsRequired = true)]
        public string Name { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 500, IsRequired = true)]
        public string? Summary { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true, MaxLength = 700, IsRequired = true)]
        public string? Description { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 4, GridOrder = 4, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductCategory,
            ServiceName = "productCategoryView",
            LinkedFieldCode = "CategoryName",
            Route = "/api/ComboBox/ProductCategories",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public Guid CategoryId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.ProductCategory,
            ServiceName = "productCategoryView",
            LinkedFieldCode = "CategoryId",
            Route = "/api/ComboBox/ProductCategories",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? CategoryName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductBrand,
            ServiceName = "productBrandView",
            LinkedFieldCode = "BrandName",
            Route = "/api/ComboBox/ProductBrands",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public Guid? BrandId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.ProductBrand,
            ServiceName = "productBrandView",
            LinkedFieldCode = "BrandId",
            Route = "/api/ComboBox/ProductBrands",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? BrandName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductType,
            ServiceName = "productTypeView",
            LinkedFieldCode = "ProductTypeName",
            Route = "/api/ComboBox/ProductTypes",
            SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid ProductTypeId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = true, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.ProductType,
            ServiceName = "productTypeView",
            LinkedFieldCode = "ProductTypeId",
            Route = "/api/ComboBox/ProductTypes",
            SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? ProductTypeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 10, GridOrder = 10, ShowInGrid = false, ShowInForm = true)]
        public decimal Price { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, MaxLength = 100, IsRequired = true)]
        public string? Barcode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true, MaxLength = 100, IsRequired = true)]
        public string? Sku { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true, MaxLength = 250, IsRequired = true)]
        public string Slug { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.MultiSelectBox, IsRequired = false, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.ProductTag, ServiceName = "productTagView", LinkedFieldCode = "ProductTagNames", Route = "", SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? ProductTagIds { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = false, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = false, EntityName = LookupEntities.ProductTag, ServiceName = "productTagView", LinkedFieldCode = "ProductTagIds", Route = "", SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? ProductTagNames { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true)]
        public decimal? Weight { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? MainImage { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]
        public string? Thumbnail { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 19, GridOrder = 19, ShowInGrid = true, ShowInForm = true)]
        public int SortOrder { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 20, GridOrder = 20, ShowInGrid = true, ShowInForm = true)]
        public bool? IsFeatured { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 21, GridOrder = 21, ShowInGrid = true, ShowInForm = true)]
        public bool? IsPublished { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 22, GridOrder = 22, ShowInGrid = true, ShowInForm = true)]
        public bool? IsActive { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 23, GridOrder = 23, ShowInGrid = true, ShowInForm = true, MaxLength = 200, IsRequired = true)]
        public string? SeoTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 24, GridOrder = 24, ShowInGrid = true, ShowInForm = true, MaxLength = 500, IsRequired = true)]
        public string? SeoDescription { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 25, GridOrder = 25, ShowInGrid = true, ShowInForm = false)]
        public int SaleCount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 26, GridOrder = 26, ShowInGrid = true, ShowInForm = false)]
        public int ViewCount { get; set; }
    }
}
