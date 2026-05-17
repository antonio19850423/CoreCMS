using System.ComponentModel.DataAnnotations;

namespace Velora.Application.Shared.Dtos;

public class ComponentTypeDto
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Code { get; set; } = null!;

    [StringLength(30)]
    public string Type { get; set; } = null!;

    [StringLength(300)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

}