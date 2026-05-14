using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ResourceJsonModel
    {
        public string Type { get; set; } = "MENU"; // MENU / PAGE / ACTION / FIELD
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        // DisplayName per language
        public Dictionary<string, string> DisplayName { get; set; } = new();

        public int Order { get; set; } = 1;

        // Child resources
        public List<ResourceJsonModel> Children { get; set; } = new();

        // Roles that have permission on this resource
        public List<string> Roles { get; set; } = new();
    }
}
