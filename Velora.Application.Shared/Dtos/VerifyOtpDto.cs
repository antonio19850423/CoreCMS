using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class VerifyOtpDto
    {
        public string Mobile { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
    }
}
