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
    public class ContentCategoryDto
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(200)]
        public string? Slug { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        [StringLength(150)]
        public string? Icon { get; set; }

        [StringLength(50)]
        public string? IconColor { get; set; }

        public Guid? ParentId { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
