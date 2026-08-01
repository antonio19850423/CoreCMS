using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class WebsiteUserDto
    {
        public Guid Id { get; set; }

        public string Mobile { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? NationalCode { get; set; }

        public string? Email { get; set; }

        public string? Avatar { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
