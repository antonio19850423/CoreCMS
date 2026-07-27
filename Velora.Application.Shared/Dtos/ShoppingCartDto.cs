using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ShoppingCartDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        [StringLength(200)]
        public string CartToken { get; set; } = null!;

        public Guid? TenantId { get; set; }

        public int Status { get; set; }

        public DateTime? ExpireAt { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }
    }
}
