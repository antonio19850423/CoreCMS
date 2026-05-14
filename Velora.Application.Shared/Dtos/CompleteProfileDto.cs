using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CompleteProfileDto
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Nationalcode { get; set; }
        public string? Address { get; set; }
        public Guid? Countryid { get; set; }
        public Guid? Stateid { get; set; }
        public Guid? Cityid { get; set; }
    }
}
