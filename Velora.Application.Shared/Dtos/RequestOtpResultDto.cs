using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class RequestOtpResultDto
    {
        public int ExpirationMinutes { get; set; }
        public bool IsExistingUser { get; set; }
    }
}
