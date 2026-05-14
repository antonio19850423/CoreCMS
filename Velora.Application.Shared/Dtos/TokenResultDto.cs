using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class TokenResultDto
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }

}
