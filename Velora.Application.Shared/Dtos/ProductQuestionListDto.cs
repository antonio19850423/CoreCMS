using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductQuestionListDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Question { get; set; } = null!;

        public string? Answer { get; set; }

        public bool IsAnswered { get; set; }

        public string UserName { get; set; } = null!;

        public string PersianDate { get; set; } = null!;
    }
}
