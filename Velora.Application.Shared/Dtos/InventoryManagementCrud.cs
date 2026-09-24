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
    public class InventoryManagementCrud : BulkInsert
    {
        public Guid Id { get; set; }

        public Guid? VariantId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 200)]

        public string ProductName { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 150)]

        public string? VariantName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]

        public string? Sku { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 100)]

        public string? Barcode { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]

        public decimal Price { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true)]

        public int InventoryInQuantity { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public int ReservedQuantity { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = true)]
        public int PaidQuantity { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = true)]
        public int? CurrentQuantity { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = true)]
        public int? LowStockThreshold { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = true)]
        public int? LowStockQuantity { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = true)]
        public bool? IsOutOfStock { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 13, GridOrder = 13, ShowInGrid = true, ShowInForm = true)]
        public bool? IsLowStock { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 14, GridOrder = 14, ShowInGrid = true, ShowInForm = true)]
        public bool? IsInStock { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = true)]
        public int InventoryStatusPriority { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true)]
        public string InventoryStatusTitle { get; set; } = null!;

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = true)]
        public decimal? InventoryValue { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true)]
        public bool? ProductIsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 19, GridOrder = 19, ShowInGrid = true, ShowInForm = true)]
        public bool? VariantIsActive { get; set; }
    }
}
