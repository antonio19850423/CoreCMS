using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class RolePermissionMapDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = default!;
        public List<string> ResourceCodes { get; set; } = new();
    }

}
