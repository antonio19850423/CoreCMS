using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Enums;

namespace Velora.Application.Shared.Constants
{
    public static class LocalizationKeysExtensions
    {
        /// <summary>
        /// تبدیل enum به کلید localization با prefix دلخواه
        /// </summary>
        public static string ToKey(this LocalizationKeys key, string prefix)
        {
            return $"{prefix}{key}";
        }

        /// <summary>
        /// تبدیل enum به کلید localization با پیش‌فرض "Message."
        /// </summary>
        public static string ToMessageKey(this LocalizationKeys key)
        {
            return key.ToKey("Message.");
        }
    }
    public static class FieldTypes
    {
        public const string Text = "Text";
        public const string Number = "Number";
        public const string Date = "Date";
        public const string Checkbox = "Checkbox";
        public const string Select = "Select";
        public const string Autocomplete = "Autocomplete";
        public const string Textarea = "Textarea";
        public const string SelectBox = "SelectBox";
        public const string ComboBox = "ComboBox";
        public const string Image = "Image";
        public const string MultiSelectBox = "MultiSelectBox";



        // هر نوع کنترل دیگری که نیاز دارید
    }
    public static class InputMasks
    {
        public const string None = "None";
        public const string Email = "Email";
        public const string Phone = "Phone";
        public const string Mobile = "Mobile";
        public const string NationalCode = "NationalCode";
        public const string Currency = "Currency";
        public const string Number = "Number";
        public const string Password = "Password";
        
        }
    public static class LookupEntities
    {
        public const string ResourceType = "ResourceType";
        public const string Resource = "Resource";
        public const string User = "User";
        public const string Role = "Role";
        public const string Permission = "Permission";
        public const string ComponentType = "ComponentType";
        public const string Category = "Category";
        public const string PageTemplate = "PageTemplate";
        public const string PageTemplateComponent = "PageTemplateComponent";
        public const string Page = "Page";
        public const string Section = "Section";
        public const string CmsConfiguration = "CmsConfiguration";
        public const string SiteSetting = "SiteSetting";
        public const string SectionItem = "SectionItem";
        public const string SectionGroupItem = "SectionGroupItem";
        public const string ContentItem = "ContentItem";
        



        // سایر entityها...
    }
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Developer = "Developer";
    }


    public static class SeederNames
    {
        public const string Core = "Seed_Core_Data";
        public const string Localization = "Seed_Localization";
        public const string Resources = "Seed_Resources";
        public const string Permissions = "Seed_Permissions";
        public const string Settings = "Seed_Settings";
    }
    public static class SiteTypes
    {
        public const string COMPANY = "COMPANY";
    }



}
