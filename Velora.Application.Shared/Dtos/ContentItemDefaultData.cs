using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ContentItemDefaultData
    {
        public ContentItemLanguageData Rtl { get; set; } = new ContentItemLanguageData();
        public ContentItemLanguageData Ltr { get; set; } = new ContentItemLanguageData();
    }
}
