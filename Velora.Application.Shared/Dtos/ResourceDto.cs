using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public Guid ResourceTypeId { get; set; }
        public Guid? ParentId { get; set; }
        public string LanguageCode { get; set; } = null!;
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public bool IsTest { get; set; } = false;
        public string? FieldType { get; set; }

        public int? MaxLength { get; set; }

        public bool IsRequired { get; set; }

        public bool ShowInForm { get; set; }

        public bool ShowInGrid { get; set; }

        public int? FormOrder { get; set; }

        public int? GridOrder { get; set; }
        public string? Route { get; set; }
        public string? InputMask { get; set; }
        public bool? IsDynamicForm { get; set; }
        /// <summary>
        /// مشخص‌کنندهٔ کد فیلدی است که با این فیلد ارتباط دارد و مقدار نمایشی آن را تأمین می‌کند.
        /// معمولاً برای فیلدهای از نوع ComboBox یا AutoComplete استفاده می‌شود.
        /// به‌عنوان مثال، اگر این فیلد "RoleId" باشد و مقدار LinkedFieldCode برابر "RoleName" تنظیم شود،
        /// در رابط کاربری مقدار "RoleName" به‌عنوان متن نمایشی نمایش داده می‌شود، 
        /// اما مقدار واقعی که در زمان ذخیره ارسال می‌گردد همان "RoleId" خواهد بود.
        /// </summary>
        public string LinkedFieldCode { get; set; }
        public bool ShowInSelectBox { get; set; } = false;
        public int? SelectBoxOrder { get; set; }
        public string? EntityName { get; set; }

        public string? ServiceName { get; set; }
        public string? SelectDisplayFields { get; set; }

    }
}
