using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class LoginResultDto
    {
        public UserDto User { get; set; }          // اطلاعات کاربر
        public string Token { get; set; }          // توکن JWT
        public DateTime ExpireDate { get; set; }   // زمان انقضای توکن
        public string RefreshToken { get; set; }   // (اختیاری) رفرش توکن اگر داری
    }
}
