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
    public class ProductCategoryCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = true, MaxLength = 150,ShowInSelectBox =true,SelectBoxOrder =1,IsRequired =true)]
        public string Name { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, ShowInTreeView = true, MaxLength = 200, ShowInSelectBox = true, SelectBoxOrder = 2,IsRequired =true)]
        public string? Slug { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder =3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, ShowInTreeView = true, MaxLength = 200, ShowInSelectBox = true, SelectBoxOrder = 3, IsRequired = true)]
        public string? SeoTitle { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, ShowInTreeView = true, MaxLength = 500, ShowInSelectBox = true, SelectBoxOrder = 4, IsRequired = true)]
        public string? SeoDescription { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, MaxLength = 300)]
        public string? Description { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, EntityName = LookupEntities.ContentCategory, ServiceName = "contentCategoryView", LinkedFieldCode = "ParentName", Route = "/api/ComboBox/ProductCategories", SelectDisplayFields = "[\"label\",\"code\"]")]
        public Guid? ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.ContentCategory, ServiceName = "contentCategoryView", LinkedFieldCode = "ParentId", Route = "/api/ComboBox/ProductCategories", SelectDisplayFields = "[\"label\",\"code\"]")]
        public string? ParentName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder =8, GridOrder = 8, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, MaxLength = 150)]
        public string? Icon { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, MaxLength = 50)]
        public string? IconColor { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false,IsRequired =true)]
        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true, ShowInTreeView = true)]
        public bool IsActive { get; set; }
        // ===== Audit =====
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string? UpdatedByName { get; set; }

    }
}
