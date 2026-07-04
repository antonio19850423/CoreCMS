using System.ComponentModel.DataAnnotations;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;

namespace Velora.Application.Shared.Dtos;
public class TagCrud : BulkInsert
{
    public Guid? Id { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 1, GridOrder = 1, ShowInGrid = true, ShowInForm = true, MaxLength = 150, ShowInSelectBox = true)]
    public string Name { get; set; } = null!;
    [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 2, GridOrder = 2, ShowInGrid = true, ShowInForm = true, MaxLength = 150, ShowInSelectBox = true)]
    public string Slug { get; set; } = null!;
    [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 3, GridOrder = 3, ShowInGrid = true, ShowInForm = true, MaxLength = 50, ShowInSelectBox = true)]
    public string Color { get; set; } = null!;
    [ResourceColumn(FieldType = FieldTypes.Textarea, FormOrder = 4, GridOrder = 4, ShowInGrid = true, ShowInForm = true, MaxLength = 300)]
    public string? Description { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Number, FormOrder = 5, GridOrder = 5, ShowInGrid = true, ShowInForm = true)]
    public int? SortOrder { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Checkbox, FormOrder = 6, GridOrder = 6, ShowInGrid = true, ShowInForm = true)]
    public bool? IsActive { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 7, GridOrder = 7, ShowInGrid = true, ShowInForm = false)]
    public string CreatedAtPersian { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Text, FormOrder = 8, GridOrder = 8, ShowInGrid = true, ShowInForm = false)]
    public string UpdatedAtPersian { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 9, GridOrder = 9, ShowInGrid = true, ShowInForm = false)]
    public string? CreatedByName { get; set; }
    [ResourceColumn(FieldType = FieldTypes.Date, FormOrder = 10, GridOrder = 10, ShowInGrid = true, ShowInForm = false)]
    public string? UpdatedByName { get; set; }
}