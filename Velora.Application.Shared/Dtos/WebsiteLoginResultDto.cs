using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class WebsiteLoginResultDto
    {
        public WebsiteUserDto User { get; set; } = null!;

        public string Token { get; set; } = null!;

        public DateTime ExpireDate { get; set; }

        public string RefreshToken { get; set; } = null!;
    }
}
