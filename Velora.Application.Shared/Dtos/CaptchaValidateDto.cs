using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CaptchaValidateDto
    {
        public Guid CaptchaId { get; set; }

        public string UserInput { get; set; } = null!;
    }
}
