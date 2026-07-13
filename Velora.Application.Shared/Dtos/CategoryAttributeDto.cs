using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos;

public class CategoryAttributeDto
{
    [Key]
    public Guid Id { get; set; }

    public Guid CategoryId { get; set; }

    public Guid AttributeId { get; set; }

    public int SortOrder { get; set; }

    public bool IsTest { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

}