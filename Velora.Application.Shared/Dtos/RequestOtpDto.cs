using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class RequestOtpDto
    {
        public string Mobile { get; set; } = null!;
        public Guid CaptchaId { get; set; }

        public string CaptchaCode { get; set; }
            = null!;
    }
}
