using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CaptchaGenerateDto
    {
        public Guid CaptchaId { get; set; }

        public string Image { get; set; } = null!;

        public int ExpirationSeconds { get; set; }
    }
}
