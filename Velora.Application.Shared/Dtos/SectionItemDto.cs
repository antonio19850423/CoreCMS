using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class SectionItemDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        [StringLength(250)]
        public string? Title { get; set; }

        [StringLength(300)]
        public string? Subtitle { get; set; }

        public string? Description { get; set; }

        [StringLength(150)]
        public string? Price { get; set; }

        [StringLength(300)]
        public string? ImageUrl { get; set; }

        [StringLength(300)]
        public string? AvatarUrl { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }

        public bool IsTest { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        [StringLength(50)]
        public string? BackgroundColor { get; set; }

        [StringLength(50)]
        public string? SubtitleColor { get; set; }

        [StringLength(50)]
        public string? DescriptionColor { get; set; }

        [StringLength(100)]
        public string? Link1Text { get; set; }

        [StringLength(300)]
        public string? Link1Url { get; set; }

        [StringLength(50)]
        public string? Link1Color { get; set; }

        [StringLength(100)]
        public string? Link2Text { get; set; }

        [StringLength(300)]
        public string? Link2Url { get; set; }

        [StringLength(50)]
        public string? Link2Color { get; set; }

        [StringLength(100)]
        public string? Link3Text { get; set; }

        [StringLength(300)]
        public string? Link3Url { get; set; }

        [StringLength(50)]
        public string? Link3Color { get; set; }

        [StringLength(100)]
        public string? Link4Text { get; set; }

        [StringLength(300)]
        public string? Link4Url { get; set; }

        [StringLength(50)]
        public string? Link4Color { get; set; }

        [StringLength(150)]
        public string? Icon { get; set; }

        [StringLength(50)]
        public string? IconColor { get; set; }

        [StringLength(150)]
        public string? IconAlt { get; set; }

        [StringLength(250)]
        public string? ImageAlt { get; set; }

        [StringLength(50)]
        public string? TitleColor { get; set; }

        [StringLength(150)]
        public string? AvatarAlt { get; set; }
        public string? Features { get; set; }
    }
}
