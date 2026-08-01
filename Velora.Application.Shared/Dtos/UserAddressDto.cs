using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public  class UserAddressDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [StringLength(100)]
        public string Title { get; set; } = null!;

        public Guid ProvinceId { get; set; }

        public Guid CityId { get; set; }

        [StringLength(1000)]
        public string Address { get; set; } = null!;

        [StringLength(10)]
        public string PostalCode { get; set; } = null!;

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public bool IsTest { get; set; }
    }
}
