using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class LocalizationtranslationDto
    {
        public long Id { get; set; }

        public string LocalizationKeyCode { get; set; } = null!;

        public string LanguageCode { get; set; } = null!;

        public string Value { get; set; } = null!;
        public string Direction { get; set; } = "ltr";

        public bool IsTest { get; set; }
        public Dictionary<string,string> Translations { get; set; }

        }
}
