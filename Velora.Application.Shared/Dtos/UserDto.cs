using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.GraphQL;
namespace Velora.Application.Shared.Dtos
{
    public  class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NationalCode { get; set; }

        public string? MobileNumber { get; set; }
        public bool IsActive { get; set; }
        [Shared.GraphQL.GraphQLIgnore]
        public List<RoleDto> Roles { get; set; }
        public bool IsTest { get; set; } = false;
    }
}
