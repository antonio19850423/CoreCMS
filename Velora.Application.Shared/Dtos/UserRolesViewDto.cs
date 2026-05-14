using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class UserRoleViewDto
    {
        public Guid UserId { get; set; }

        [StringLength(100)]
        public string UserName { get; set; } = null!;

        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public bool UserIsActive { get; set; }

        public Guid RoleId { get; set; }

        [StringLength(100)]
        public string RoleName { get; set; } = null!;

        [StringLength(50)]
        public string RoleCode { get; set; } = null!;

        [StringLength(500)]
        public string? RoleDescription { get; set; }
    }
}
