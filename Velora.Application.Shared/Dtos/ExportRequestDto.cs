using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ExportRequestDto
    {
        public bool ExportCurrentPage { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
