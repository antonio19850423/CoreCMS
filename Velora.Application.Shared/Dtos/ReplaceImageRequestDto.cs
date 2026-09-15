using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ReplaceImageRequestDto
    {
        public IFormFile File { get; set; }

        public string Name { get; set; }

        public string? OldUrl { get; set; }
    }
}
