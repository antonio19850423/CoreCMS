using System.ComponentModel.DataAnnotations;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos;
public class StateCrud : BulkInsert
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string StateTitle { get; set; } = null!;

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}