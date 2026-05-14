using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        public int Actions { get; set; }
        public Guid ResourceId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsTest { get; set; } = false;
    }
}
