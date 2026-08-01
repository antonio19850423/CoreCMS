using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class UserLoginDto
    {
        public UserDto User { get; set; } = null!;

        public UserProfileDto? Profile { get; set; }
    }
}
