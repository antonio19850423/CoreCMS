using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ContentItemLanguageData
    {
        public string ContentType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
        public string? Content { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageAlt { get; set; }

        public string? AuthorName { get; set; }
        public string? AuthorTitle { get; set; }
        public string? AuthorAvatarUrl { get; set; }

        public DateTime? PublishedAt { get; set; }

        public List<TagDto> Tags { get; set; }
        public string? Slug { get; set; }

        public string CategoryName { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? PageId { get; set; }
        public string? ExternalUrl { get; set; }
        public int SortOrder { get; set; }
        public string? SourceTitle { get; set; }
        public string? SourceUrl { get; set; }
        public string? ImageDetailUrl { get; set; }
        public string? ImageDetailAlt { get; set; }
    }
}
