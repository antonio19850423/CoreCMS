using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class LocalizationViewDto
    {
        public string? LocalizationKeyCode { get; set; }

        public string? LanguageCode { get; set; }

        public string? Value { get; set; }

        public string? Type { get; set; }

        public int? Order { get; set; }
    }
}
