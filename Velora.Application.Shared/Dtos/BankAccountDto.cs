using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public class BankAccountDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SiteSettingId { get; set; }

        [StringLength(100)]
        public string BankName { get; set; } = null!;

        [StringLength(200)]
        public string AccountOwnerName { get; set; } = null!;

        [StringLength(50)]
        public string? CardNumber { get; set; }

        [StringLength(100)]
        public string? AccountNumber { get; set; }

        [StringLength(50)]
        public string? ShebaNumber { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
