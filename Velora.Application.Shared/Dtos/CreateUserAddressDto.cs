using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CreateUserAddressDto
    {
        public string Title { get; set; } = null!;

        public Guid ProvinceId { get; set; }

        public Guid CityId { get; set; }

        public string Address { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public bool IsDefault { get; set; }
    }
}
