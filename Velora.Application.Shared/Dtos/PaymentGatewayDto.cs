using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PaymentGatewayDto
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(50)]
        public string GatewayCode { get; set; } = null!;

        public int ProviderType { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(300)]
        public string? LogoUrl { get; set; }

        public string? SettingsJson { get; set; }

        [StringLength(500)]
        public string? CallbackUrl { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
