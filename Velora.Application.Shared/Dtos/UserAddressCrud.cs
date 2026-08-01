using System.ComponentModel.DataAnnotations;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos;
public class UserAddressCrud : BulkInsert
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(10)]
    public string PostalCode { get; set; } = null!;

    [StringLength(1000)]
    public string Address { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(100)]
    public string? CityTitle { get; set; }

    [StringLength(100)]
    public string? StateTitle { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public bool IsActive { get; set; }

    public bool IsDefault { get; set; }

    public Guid UserId { get; set; }

    public Guid CityId { get; set; }

    public Guid ProvinceId { get; set; }

    [StringLength(19)]
    public string? CreatedAtPersian { get; set; }

    [StringLength(19)]
    public string? UpdatedAtPersian { get; set; }

    [StringLength(201)]
    public string? CreatedByName { get; set; }

    [StringLength(201)]
    public string? UpdatedByName { get; set; }
}