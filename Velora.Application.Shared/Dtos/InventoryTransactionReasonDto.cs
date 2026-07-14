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
    public class InventoryTransactionReasonDto
    {
        [Key]
        public byte Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string Code { get; set; } = null!;

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
