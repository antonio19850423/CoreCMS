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
    public class SiteMenuCrud : BulkInsert
    {
        public Guid Id { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true,ShowInTreeView =true, MaxLength = 200, GroupKey = "Link1", Route = "/api/ComboBox/Pages")]
        public string? Link1Text { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, MaxLength = 500, GroupKey = "Link1")]
        public string? Link1Url { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, MaxLength = 50, GroupKey = "Link1")]
        public string? Link1Color { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, GroupKey = "Link1")]
        public Guid? Link1TargetId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, GroupKey = "Link1")]
        public Guid? Link1TypeId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Link, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, GroupKey = "Link1")]
        public bool? Link1OpenInNewTab { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false)]
        public int SortOrder { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Image, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 150, ShowInTreeView = false)]
        public string? Icon { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = false, MaxLength = 50, ShowInTreeView = false)]
        public string? IconColor { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, ShowInTreeView = true)]
        public bool IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 5, GridOrder = 4, ShowInGrid = true, ShowInForm = true, ShowInTreeView = false, EntityName = LookupEntities.SectionGroupItem, ServiceName = "siteMenuView", LinkedFieldCode = "ParentName", Route = "/api/ComboBox/SiteMenus", SelectDisplayFields = "[\"label\",\"code\"]")]
        public Guid? ParentId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 5, GridOrder = 4, ShowInGrid = true, ShowInForm = false, ShowInTreeView = false, EntityName = LookupEntities.SectionGroupItem, ServiceName = "siteMenuView", LinkedFieldCode = "ParentId", Route = "/api/ComboBox/SiteMenus", SelectDisplayFields = "[\"label\",\"code\"]")]
        public string? ParentName { get; set; }

        // ===== Audit =====
        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 5, GridOrder = 6, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string CreatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 6, GridOrder = 7, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string UpdatedAtPersian { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 7, GridOrder = 8, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string? CreatedByName { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 8, GridOrder = 9, ShowInGrid = true, ShowInTreeView = false, ShowInForm = false)]
        public string? UpdatedByName { get; set; }
    }
}
