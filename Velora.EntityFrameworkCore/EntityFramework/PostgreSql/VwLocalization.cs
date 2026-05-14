using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class VwLocalization
{
    public string? LocalizationKeyCode { get; set; }

    public string? LanguageCode { get; set; }

    public string? Value { get; set; }

    public string? Type { get; set; }

    public int? Order { get; set; }
}
