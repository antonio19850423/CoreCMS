using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class SeedHistory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
