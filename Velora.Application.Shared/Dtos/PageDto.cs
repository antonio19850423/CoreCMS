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
    public  class PageDto
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string Slug { get; set; } = null!;

        public Guid? PageTemplateId { get; set; }

        public bool IsHome { get; set; }

        public bool IsPublished { get; set; }

        [StringLength(200)]
        public string? MetaTitle { get; set; }

        [StringLength(500)]
        public string? MetaDescription { get; set; }

        [StringLength(500)]
        public string? MetaKeywords { get; set; }

        [StringLength(300)]
        public string? CanonicalUrl { get; set; }

        [StringLength(300)]
        public string? OgImageUrl { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
        public bool? IsDynamic { get; set; }

    }
}
