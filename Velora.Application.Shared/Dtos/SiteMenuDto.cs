using System.ComponentModel.DataAnnotations;

namespace Velora.Application.Shared.Dtos;

public class SiteMenuDto
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ParentId { get; set; }

    public int SortOrder { get; set; }

    [StringLength(200)]
    public string Link1Text { get; set; } = null!;

    [StringLength(500)]
    public string? Link1Url { get; set; }

    public Guid? Link1TargetId { get; set; }

    public Guid? Link1TypeId { get; set; }

    [StringLength(50)]
    public string? Link1Color { get; set; }

    public bool? Link1OpenInNewTab { get; set; }
    [StringLength(150)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? IconColor { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

}