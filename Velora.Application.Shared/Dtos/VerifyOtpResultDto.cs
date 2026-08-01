using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class VerifyOtpResultDto
    {
        public bool IsExistingUser { get; set; }

        /*
         * چون ثبت‌نام داخل VerifyOtp انجام می‌شود،
         * دیگر مرحله جداگانه تکمیل پروفایل نداریم.
         */
        public bool RequiresProfileCompletion { get; set; }

        public UserDto User { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpireDate { get; set; }

        public string RefreshToken { get; set; } = null!;
    }

}
