using System.ComponentModel.DataAnnotations;

namespace Velora.Application.Shared.Dtos;

public class ProductTagDto
{
    public Guid Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string? Slug { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    [StringLength(50)]
    public string? TextColor { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }

}