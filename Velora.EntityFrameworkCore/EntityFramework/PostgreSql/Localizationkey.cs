using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class LocalizationKey
{
    public string Code { get; set; } = null!;

    public string Type { get; set; } = null!;

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public int? Order { get; set; }

    public virtual ICollection<LocalizationTranslation> LocalizationTranslations { get; set; } = new List<LocalizationTranslation>();
}
