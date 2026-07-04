using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ContentItemRuleModel
    {
        public ContentItemsRules Rules { get; set; } = new ContentItemsRules();

        public ContentItemDefaultData DefaultData { get; set; } = new ContentItemDefaultData();
    }
}
