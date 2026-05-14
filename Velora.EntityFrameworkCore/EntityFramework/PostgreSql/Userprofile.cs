using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class UserProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? NationalCode { get; set; }

    public string? Address { get; set; }

    public Guid? CountryId { get; set; }

    public Guid? StateId { get; set; }

    public Guid? CityId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool Istest { get; set; }

    public string? ProfileImage { get; set; }

    public int? Age { get; set; }

    public virtual User User { get; set; } = null!;
}
