using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
namespace InternshipPortal.ViewModels;
public class PeriodFormViewModel : IValidatableObject
{
    public int Id { get; set; }
    [Required, StringLength(30), Display(Name = "Mã đợt")] public string Code { get; set; } = "";
    [Required, StringLength(150), Display(Name = "Tên đợt")] public string Name { get; set; } = "";
    [Required, DataType(DataType.Date), Display(Name = "Ngày bắt đầu")] public DateTime? StartDate { get; set; }
    [Required, DataType(DataType.Date), Display(Name = "Ngày kết thúc")] public DateTime? EndDate { get; set; }
    [Required, EnumDataType(typeof(InternshipPeriodStatus)), Display(Name = "Trạng thái")] public InternshipPeriodStatus? Status { get; set; }
    public string? Version { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (StartDate.HasValue && EndDate.HasValue && EndDate.Value.Date < StartDate.Value.Date)
            yield return new("Ngày kết thúc phải từ ngày bắt đầu trở đi.", [nameof(EndDate)]);
        if (StartDate.HasValue && StartDate.Value.Year < 1900 || EndDate.HasValue && EndDate.Value.Year < 1900)
            yield return new("Ngày phải từ năm 1900 trở đi.");
    }
}
