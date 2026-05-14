using System;
using System.Collections.Generic;

namespace Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;

public partial class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CeatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? Createdby { get; set; }

    public Guid? Updatedby { get; set; }

    public bool IsTest { get; set; }

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
