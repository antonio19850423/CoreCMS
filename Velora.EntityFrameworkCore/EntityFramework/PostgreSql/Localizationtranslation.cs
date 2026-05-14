using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class LocalizationTranslation
{
    public long Id { get; set; }

    public string LocalizationKeyCode { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string Value { get; set; } = null!;

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual LocalizationKey LocalizationKeyCodeNavigation { get; set; } = null!;
}
