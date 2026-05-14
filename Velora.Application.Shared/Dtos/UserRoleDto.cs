using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class UserRoleDto
    {
        public Guid Id { get; set; }
        public Guid Userid { get; set; }

        public Guid Roleid { get; set; }
        public bool IsTest { get; set; } = false;
    }
}
