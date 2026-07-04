using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[Table("ContentItemTags", Schema = "cms")]
public partial class ContentItemTag
{
    public Guid ContentItemId { get; set; }

    public Guid TagId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public bool IsTest { get; set; }

    [Key]
    public Guid Id { get; set; }

    [ForeignKey("ContentItemId")]
    [InverseProperty("ContentItemTags")]
    public virtual ContentItem ContentItem { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("ContentItemTags")]
    public virtual Tag Tag { get; set; } = null!;
}
