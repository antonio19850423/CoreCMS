using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class SeedJsonModel
    {
        public List<RoleJsonModel> Roles { get; set; } = new();
        public List<UserJsonModel> Users { get; set; } = new();
        public List<ResourceJsonModel> Resources { get; set; } = new();
    }
}
