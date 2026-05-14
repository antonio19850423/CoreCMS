using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum LocalizationKeys
        {
        SaveSuccess,
        UpdateSuccess,
        DeleteSuccess,

        Unauthorized,    // 401
        NotFound,        // 404
        UserNotFound,    // کاربر یافت نشد
        ServerError,     // خطای سرور
        InvalidPassword, // رمز عبور اشتباه است
        ActionFailed,
        ValidationFailed,
        IdRequired,
        InvalidField,
        Required,
        MaxLength,
        Checkbox,
        Number,
        NoResources,
        LoadResources,
        ModelIsNull,
        LoginSuccess,
        CannotDeleteUsedRecord,
        ShouldInsert,
        ErrorFile
    }


    }
