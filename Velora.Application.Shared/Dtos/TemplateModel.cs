using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class TemplateModel
    {
        public string TemplateName { get; set; }
        public List<PageSeedModel> Pages { get; set; }
    }
}
