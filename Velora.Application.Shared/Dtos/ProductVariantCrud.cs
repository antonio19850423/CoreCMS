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
    public class ProductVariantCrud : BulkInsert
    {
        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInSelectBox = true)]
        public string? ProductName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 150, IsRequired = true, ShowInSelectBox = true)]
        public string Name { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, IsRequired = true, ShowInSelectBox = true)]
        public decimal Price { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true, IsRequired = true)]
        public decimal? ComparePrice { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
        public string? Image { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true, MaxLength = 100, IsRequired = true)]
        public string? Sku { get; set; }
        public bool IsDefault { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = false)]
        public Guid ParentId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true, MaxLength = 100, IsRequired = true)]
        public string? Barcode { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true, IsRequired = true)]
        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true)]
        public bool IsActive { get; set; }

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
