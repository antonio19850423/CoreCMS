using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public  class ContentItemDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? PageId { get; set; }

        public Guid? CategoryId { get; set; }

        [StringLength(50)]
        public string ContentType { get; set; } = null!;

        [StringLength(250)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? Summary { get; set; }

        public string? Content { get; set; }

        [StringLength(512)]
        public string? ImageUrl { get; set; }

        [StringLength(250)]
        public string? ImageAlt { get; set; }

        [StringLength(150)]
        public string? AuthorName { get; set; }

        [StringLength(150)]
        public string? AuthorTitle { get; set; }

        [StringLength(300)]
        public string? AuthorAvatarUrl { get; set; }

        [StringLength(500)]
        public string? ExternalUrl { get; set; }

        public DateTime? PublishedAt { get; set; }

        public bool IsPublished { get; set; }


        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
        [StringLength(200)]
        public string? Slug { get; set; }
        [StringLength(150)]
        public string? SourceTitle { get; set; }

        [StringLength(500)]
        public string? SourceUrl { get; set; }
        [StringLength(512)]
        public string? ImageDetailUrl { get; set; }

        [StringLength(250)]
        public string? ImageDetailAlt { get; set; }

    }
}
