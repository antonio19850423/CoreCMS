using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public partial class CmsConfigurationDto
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string SiteType { get; set; } = null!;

        public bool EnableShop { get; set; }

        public bool EnableBlog { get; set; }

        public bool EnableNews { get; set; }

        public bool EnableMultiLanguage { get; set; }

        [StringLength(100)]
        public string? DefaultTheme { get; set; }

        public bool EnableComments { get; set; }

        public bool EnableSeo { get; set; }

        public bool EnableCache { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
