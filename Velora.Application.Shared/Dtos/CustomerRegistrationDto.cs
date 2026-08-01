using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CustomerRegistrationDto
    {
        public string Mobile { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string NationalCode { get; set; } = null!;
    }
}
