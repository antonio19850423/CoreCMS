using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
    {
    public class UploadImageRequestDto
        {
        public IFormFile File { get; set; } = null!;
        public string? Name { get; set; }
        }

    }
