using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ComponentRules
    {
        public bool Title { get; set; } = true;
        public bool Subtitle { get; set; } = true;
        public bool Description { get; set; } = true;
        public bool Image { get; set; } = true;
        public bool LinkText { get; set; } = true;
        public bool LinkUrl { get; set; } = true;
    }
}
