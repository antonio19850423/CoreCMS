using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Velora.Application.Shared.Dtos;

public class ProductAttributeDto
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Code { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

}