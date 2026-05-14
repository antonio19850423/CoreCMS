using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }

        public Guid Userid { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Nationalcode { get; set; }

        public string? Address { get; set; }

        public Guid? Countryid { get; set; }

        public Guid? Stateid { get; set; }

        public Guid? Cityid { get; set; }
        public string? ProfileImage { get; set; }
        public int? Age { get; set; }
        public bool IsTest { get; set; } = false;

    }
}
