using System.ComponentModel.DataAnnotations;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos;
public class CityCrud : BulkInsert
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string CityTitle { get; set; } = null!;

    public Guid StateId { get; set; }

    [StringLength(100)]
    public string? StateTitle { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}