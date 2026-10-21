using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.ViewModels.Internships;

public class DiaryFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Hồ sơ thực tập chưa hợp lệ.")]
    public int InternshipId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Tuần phải lớn hơn 0.")]
    [Display(Name = "Tuần thực tập")] public int WeekNumber { get; set; } = 1;
    [Required(ErrorMessage = "Vui lòng nhập công việc đã làm."), StringLength(2990, ErrorMessage = "Nội dung công việc không quá 2990 ký tự.")]
    [Display(Name = "Nội dung công việc")] public string WorkContent { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập kết quả đạt được."), StringLength(2990, ErrorMessage = "Kết quả không quá 2990 ký tự.")]
    [Display(Name = "Kết quả đạt được")] public string Results { get; set; } = "";
    [StringLength(2990, ErrorMessage = "Khó khăn không quá 2990 ký tự.")]
    [Display(Name = "Khó khăn")] public string? Difficulties { get; set; }
    [StringLength(1990, ErrorMessage = "Ghi chú không quá 1990 ký tự.")]
    [Display(Name = "Ghi chú")] public string? Note { get; set; }
    [BindNever] public string PositionTitle { get; set; } = "";
    [BindNever] public string CompanyName { get; set; } = "";
    [BindNever] public DateTime StartDate { get; set; }
    [BindNever] public DateTime EndDate { get; set; }
    [BindNever] public int TotalWeeks { get; set; }
    [BindNever] public DateTime InternshipStartDate { get; set; }
    [BindNever] public DateTime InternshipEndDate { get; set; }
    [BindNever] public int? DiaryId { get; set; }
    [BindNever] public bool CanWrite { get; set; }
}
