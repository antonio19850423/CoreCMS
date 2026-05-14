using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VwUserForm
{
    public Guid? Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? IsActive { get; set; }

    public string? Password { get; set; }

    public string? ProfileImage { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? NationalCode { get; set; }

    public string? MobileNumber { get; set; }

    public string? Address { get; set; }

    public string? CountryName { get; set; }

    public Guid? CountryId { get; set; }

    public string? StateName { get; set; }

    public Guid? StateId { get; set; }

    public string? CityName { get; set; }

    public Guid? CityId { get; set; }

    public Guid? RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleNames { get; set; }

    public string? RoleIds { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Age { get; set; }

    public bool? IsDeleted { get; set; }
}
