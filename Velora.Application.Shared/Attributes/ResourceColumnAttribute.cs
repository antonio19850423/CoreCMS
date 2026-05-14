using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ResourceColumnAttribute : Attribute
    {
        /// <summary>
        /// نوع کنترل / داده (Text, Number, Date, Checkbox, Select, Autocomplete...)
        /// </summary>
        public string FieldType { get; set; } = FieldTypes.Text;

        /// <summary>
        /// حداکثر طول برای رشته‌ها
        /// </summary>
        public int MaxLength { get; set; } = 0;

        /// <summary>
        /// آیا فیلد الزامی است
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// ترتیب نمایش در فرم
        /// </summary>
        public int FormOrder { get; set; } = 0;

        /// <summary>
        /// ترتیب نمایش در دیتاگرید
        /// </summary>
        public int GridOrder { get; set; } = 0;

        /// <summary>
        /// آیا فیلد در فرم نمایش داده شود
        /// </summary>
        public bool ShowInForm { get; set; } = true;

        /// <summary>
        /// آیا فیلد در دیتاگرید نمایش داده شود
        /// </summary>
        public bool ShowInGrid { get; set; } = true;

        /// <summary>
        /// توضیح اضافی / Tooltip
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// مشخص‌کنندهٔ کد فیلدی است که با این فیلد ارتباط دارد و مقدار نمایشی آن را تأمین می‌کند.
        /// معمولاً برای فیلدهای از نوع ComboBox یا AutoComplete استفاده می‌شود.
        /// به‌عنوان مثال، اگر این فیلد "RoleId" باشد و مقدار LinkedFieldCode برابر "RoleName" تنظیم شود،
        /// در رابط کاربری مقدار "RoleName" به‌عنوان متن نمایشی نمایش داده می‌شود، 
        /// اما مقدار واقعی که در زمان ذخیره ارسال می‌گردد همان "RoleId" خواهد بود.
        /// </summary>
        public string LinkedFieldCode { get; set; }
        public bool ShowInSelectBox { get; set; } = false;
        public int SelectBoxOrder { get; set; } = 0;

        public string? Route { get; set; }
        public string? InputMask { get; set; } = InputMasks.None; // "Email", "Number", "Currency", "NationalCode", "Phone"
        public string? EntityName { get; set; }

        public string? ServiceName { get; set; }
        public string? SelectDisplayFields { get; set; }

        

    }
}
