using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CmsConfigurationSeedSection
    {
        public string? DefaultTheme { get; set; }

        public bool? EnableShop { get; set; }
        public bool? EnableBlog { get; set; }
        public bool? EnableNews { get; set; }

        public bool? EnableMultiLanguage { get; set; }
        public bool? EnableComments { get; set; }
        public bool? EnableSeo { get; set; }
        public bool? EnableCache { get; set; }

        public bool? EnableFaq { get; set; }
        public bool? EnablePrivacy { get; set; }
        public bool? EnableDynamicPages { get; set; }

        public bool? EnableProductCategoriesMenu { get; set; }
        public bool? EnableProductBrandsMenu { get; set; }
        public bool? EnableProductSearchMenu { get; set; }

        public bool? EnableUserLogin { get; set; }
        public bool? EnableUserRegistration { get; set; }

        public string? SiteType { get; set; }

        public bool? IsActive { get; set; }
    }
}
