using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
