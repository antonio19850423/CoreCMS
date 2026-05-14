using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class SeedHistoryDto
    {
        public int Id { get; set; }

        [StringLength(300)]
        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
