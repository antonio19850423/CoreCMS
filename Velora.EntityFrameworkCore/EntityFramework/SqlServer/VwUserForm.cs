using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Keyless]
public partial class VwUserForm
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    [StringLength(256)]
    public string? Password { get; set; }

    [StringLength(512)]
    public string? ProfileImage { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(10)]
    public string? NationalCode { get; set; }

    [StringLength(20)]
    public string? MobileNumber { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? CountryName { get; set; }

    public Guid? CountryId { get; set; }

    [StringLength(100)]
    public string? StateName { get; set; }

    public Guid? StateId { get; set; }

    [StringLength(100)]
    public string? CityName { get; set; }

    public Guid? CityId { get; set; }

    public Guid? RoleId { get; set; }

    [StringLength(100)]
    public string? RoleName { get; set; }

    [StringLength(4000)]
    public string? RoleNames { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? RoleIds { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Age { get; set; }

    public bool IsDeleted { get; set; }
}
