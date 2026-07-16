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
    public class ProductInventoryTransactionCrud : BulkInsert
    {

        public Guid Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, IsRequired = true)]
        public int ChangeQuantity { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 500, IsRequired = true)] 
        public string? Note { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 3, GridOrder = 3, ShowInGrid = false, ShowInForm = true,
        ServiceName = "",
        LinkedFieldCode = "OperationTypeName",
        Route = "/api/ComboBox/OperationTypes")]
        public byte OperationType { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = false,
            ServiceName = "",
            LinkedFieldCode = "OperationType",
            Route = "/api/ComboBox/OperationTypes")]
        public string OperationTypeName { get; set; } = null!;
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = false, ShowInForm = true,
    EntityName = LookupEntities.ProductCategory,
    ServiceName = "inventoryTransactionReasonView",
    LinkedFieldCode = "ReasonName",
    Route = "/api/ComboBox/InventoryTransactionReasons",
    SelectDisplayFields = "[\"name\",\"code\"]")]
        public Guid ReasonId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = false,
           EntityName = LookupEntities.ProductCategory,
           ServiceName = "inventoryTransactionReasonView",
           LinkedFieldCode = "ReasonId",
           Route = "/api/ComboBox/InventoryTransactionReasons",
           SelectDisplayFields = "[\"name\",\"code\"]")]
        public string? ReasonName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 6, GridOrder = 6, ShowInGrid = false, ShowInForm = true,
            EntityName = LookupEntities.ProductCategory,
            ServiceName = "productView",
            LinkedFieldCode = "ProductName",
            Route = "/api/ComboBox/Products",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public Guid ProductId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = false,
            EntityName = LookupEntities.ProductCategory,
            ServiceName = "productView",
            LinkedFieldCode = "ProductId",
            Route = "/api/ComboBox/Products",
            SelectDisplayFields = "[\"name\",\"slug\"]")]
        public string? ProductName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = false, ShowInForm = true,
        EntityName = LookupEntities.ProductCategory,
        ServiceName = "productVariantView",
        LinkedFieldCode = "ProductVariantName",
        Route = "/api/ComboBox/ProductVariants",
        SelectDisplayFields = "[\"name\",\"price\"]")]
        public Guid? ProductVariantId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = false, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false,
           EntityName = LookupEntities.ProductCategory,
           ServiceName = "productVariantView",
           LinkedFieldCode = "ProductVariantId",
           Route = "/api/ComboBox/ProductVariants",
           SelectDisplayFields = "[\"name\",\"price\"]")]
        public string? ProductVariantName { get; set; }


        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = false, ShowInForm = false, MaxLength = 50)]
        public string? ReasonCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 9, GridOrder = 9, ShowInGrid = false, ShowInForm = false)]
        public Guid? ReferenceId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 10, GridOrder = 10, ShowInGrid = false, ShowInForm = false)]
        public Guid? ReferenceDetailId { get; set; }
        public Guid ParentId { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 11, GridOrder = 11, ShowInGrid = true, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 12, GridOrder = 12, ShowInGrid = true, ShowInForm = false)]
        public string? CreatedByName { get; set; }

    }
}
