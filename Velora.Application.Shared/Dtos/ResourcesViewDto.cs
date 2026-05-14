using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ResourcesViewDto
    {
        public ResourcesViewDto()
        {
            Children = new List<ResourcesViewDto>();
        }
        public Guid? ResourceId { get; set; }

        public string? ResourceCode { get; set; }

        public string? DefaultDisplayName { get; set; }

        public string? LanguageCode { get; set; }

        public string Name { get; set; }
        public int? Order { get; set; }
        public Guid? ParentId { get; set; }
        public int? Actions { get; set; }
        public Guid? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? ResourceTypeId { get; set; }
        // مشخصات فیلد (برای ResourceType = FIELD)
        public string? FieldType { get; set; }  // Text, Number, Date, Checkbox, Select ...
        public int? MaxLength { get; set; }
        public bool IsRequired { get; set; }
        public bool ShowInForm { get; set; }
        public bool ShowInGrid { get; set; }
        public int? FormOrder { get; set; }     // ترتیب در فرم
        public int? GridOrder { get; set; }     // ترتیب در دیتاگرید
        public string ResourceTypeCode { get; set; }
        public string? Route { get; set; }
        public bool? IsDynamicForm { get; set; }
        public string? InputMask { get; set; }
        public string LinkedFieldCode { get; set; }

        public bool ShowInSelectBox { get; set; } = false;
        public int? SelectBoxOrder { get; set; }
        public string? EntityName { get; set; }

        public string? ServiceName { get; set; }
        public string? SelectDisplayFields { get; set; }
        // فقط برای ResourceType = MENU
        public List<ResourcesViewDto> Children { get; set; } = new List<ResourcesViewDto>();
    }
}
