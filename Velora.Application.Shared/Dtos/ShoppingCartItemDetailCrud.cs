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
    public class ShoppingCartItemDetailCrud : BulkInsert
    {
// ================================
// Display Fields
// ================================

[ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? OrderCode { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? ProductName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? ProductVariantName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? ProductCategoryName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? ProductBrandName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public string? ProductTypeName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public int? Quantity { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Lable, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public decimal? UnitPrice { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public decimal? ProductVariantPrice { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public decimal? DiscountAmount { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Currency, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 16)]
        public decimal? FinalUnitPrice { get; set; }


        // ================================
        // Internal / Related Fields
        // ================================

        public Guid? Id { get; set; }

        public Guid ParentId { get; set; }

        public Guid? ShoppingCartId { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? VariantId { get; set; }

        public Guid? DiscountId { get; set; }

        public Guid? DiscountItemId { get; set; }

        public int? DiscountType { get; set; }

        [StringLength(9)]
        public string DiscountTypeTitle { get; set; } = null!;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DiscountValue { get; set; }


        // ================================
        // Dates
        // ================================

        public DateTime? CreatedAt { get; set; }

        [StringLength(19)]
        public string? CreatedAtPersian { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(19)]
        public string? UpdatedAtPersian { get; set; }

}

}
