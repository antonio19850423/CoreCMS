using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos
{
    public  class UserCrud: BulkInsert
    {
        public Guid? Id { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 100, IsRequired = true, FormOrder = 0, GridOrder = 0,ShowInGrid =true,ShowInForm =true)]
        public string? UserName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text,InputMask =InputMasks.Password,MaxLength = 255,IsRequired = false,FormOrder = 1,GridOrder = 1,ShowInGrid = false,ShowInForm = true)]
        public string? Password { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Text, InputMask=InputMasks.Email, MaxLength = 200, IsRequired = false, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true)]
        public string? Email { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, InputMask = InputMasks.Mobile, MaxLength = 20, IsRequired = true, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true)]
        public string? MobileNumber { get; set; }

        [ResourceColumn(FieldType = FieldTypes.Checkbox, IsRequired = true, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true)]
        public bool? IsActive { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text,MaxLength =100, IsRequired = true, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]
        public string? FirstName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 100, IsRequired = true, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true)]
        public string? LastName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 20, InputMask = InputMasks.NationalCode, IsRequired = false, FormOrder =7, GridOrder = 7, ShowInGrid = true, ShowInForm = true)]
        public string? NationalCode { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Image,MaxLength = 512,IsRequired = false,FormOrder =8,GridOrder = 8,ShowInGrid = false,ShowInForm = true)]
        public string? ProfileImage { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Textarea, MaxLength = 500, IsRequired = false, FormOrder = 9,GridOrder =9, ShowInGrid = false, ShowInForm = true)]
        public string? Address { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, MaxLength = 100, IsRequired = false, FormOrder = 10, GridOrder =10, ShowInGrid = false, ShowInForm = false)]
        public string? CountryName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 10, GridOrder = 10, ShowInGrid = false, ShowInForm = false)]
        public Guid? CountryId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, MaxLength = 100, IsRequired = false, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = false)]
        public string? StateName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 11, GridOrder = 11, ShowInGrid = false, ShowInForm = false)]
        public Guid? StateId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 100, IsRequired = false, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = false)]
        public string? CityName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Autocomplete, IsRequired = false, FormOrder = 12, GridOrder = 12, ShowInGrid = false, ShowInForm = false)]
        public Guid? CityId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, MaxLength = 20, InputMask = InputMasks.Phone, IsRequired = false, FormOrder = 13, GridOrder = 13)]
        public string? PhoneNumber { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, IsRequired = true, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = false,EntityName =LookupEntities.Role,ServiceName = "roleView", LinkedFieldCode = "RoleName",Route = "/api/ComboBox/roles", SelectDisplayFields = "[\"code\",\"name\"]")]
        public Guid? RoleId { get; set; }
        [ResourceColumn(FieldType = FieldTypes.SelectBox, MaxLength = 100, IsRequired = true, FormOrder = 14, GridOrder = 14, ShowInGrid = false,EntityName = LookupEntities.Role, ServiceName = "roleView", ShowInForm = false,LinkedFieldCode= "RoleId")]
        public string? RoleName { get; set; }
        [ResourceColumn(FieldType = FieldTypes.MultiSelectBox, IsRequired = true, FormOrder = 14, GridOrder = 14, ShowInGrid = false, ShowInForm = true, EntityName = LookupEntities.Role, ServiceName = "roleView", LinkedFieldCode = "RoleNames", Route = "/api/ComboBox/roles", SelectDisplayFields = "[\"code\",\"name\"]")]
        public string RoleIds { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Text, IsRequired = true, FormOrder = 15, GridOrder = 15, ShowInGrid = true, ShowInForm = false, EntityName = LookupEntities.Role, ServiceName = "roleView", LinkedFieldCode = "RoleIds", Route = "/api/ComboBox/roles", SelectDisplayFields = "[\"code\",\"name\"]")]
        public string? RoleNames { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, IsRequired = true, FormOrder = 16, GridOrder = 16, ShowInGrid = true, ShowInForm = true)]
        public DateTime? CreatedAt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Date, IsRequired = true, FormOrder = 17, GridOrder = 17, ShowInGrid = true, ShowInForm = true)]
        public DateTime? UpdatedAt { get; set; }
        [ResourceColumn(FieldType = FieldTypes.Number, IsRequired = false, FormOrder = 18, GridOrder = 18, ShowInGrid = true, ShowInForm = true)]
        public int? Age { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
