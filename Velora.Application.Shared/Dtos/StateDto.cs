using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public  class StateDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CountryId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool IsTest { get; set; }

    }
}
