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
    public class ProductFileDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        [StringLength(300)]
        public string FileUrl { get; set; } = null!;

        [StringLength(300)]
        public string? ThumbnailUrl { get; set; }

        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(200)]
        public string? Alt { get; set; }

        [StringLength(30)]
        public string MediaType { get; set; } = null!;

        public int SortOrder { get; set; }

        public bool IsMain { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
}
