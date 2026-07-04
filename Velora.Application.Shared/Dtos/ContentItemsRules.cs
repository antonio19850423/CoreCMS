using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ContentItemsRules
    {
        public bool ContentType { get; set; } = true;
        public bool Title { get; set; } = true;
        public bool Summary { get; set; } = true;
        public bool Content { get; set; } = true;
        public bool ImageUrl { get; set; } = true;
        public bool ImageAlt { get; set; } = true;
        public bool AuthorName { get; set; } = true;
        public bool AuthorTitle { get; set; } = true;
        public bool AuthorAvatarUrl { get; set; } = true;
        public bool ExternalUrl { get; set; } = true;
        public bool PublishedAt { get; set; } = true;
        public bool IsPublished { get; set; } = true;
        public bool Tags { get; set; } = true;
        public bool SortOrder { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public bool Slug { get; set; } = true;
    }
}
