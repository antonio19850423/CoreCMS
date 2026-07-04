using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ContentItemDetailDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? Summary { get; set; }

        public string? Content { get; set; }

        public string ImageUrl { get; set; }

        public string ImageAlt { get; set; }

        public string CategoryName { get; set; }

        public string AuthorName { get; set; }

        public string AuthorTitle { get; set; }

        public string AuthorAvatarUrl { get; set; }

        public DateTime? PublishedAt { get; set; }

        public List<string> Tags { get; set; }
    }
}
