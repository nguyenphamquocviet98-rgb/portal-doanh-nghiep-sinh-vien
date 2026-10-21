using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternshipPortal.ViewModels.Internships;

public class InternshipSearchViewModel
{
    [StringLength(100, ErrorMessage = "Từ khóa không quá 100 ký tự.")]
    [Display(Name = "Tên vị trí")] public string? Keyword { get; set; }
    [Display(Name = "Ngành")] public int? MajorId { get; set; }
    [StringLength(200, ErrorMessage = "Địa điểm không quá 200 ký tự.")]
    [Display(Name = "Địa điểm")] public string? Location { get; set; }
    [Display(Name = "Doanh nghiệp")] public int? CompanyId { get; set; }
    [EnumDataType(typeof(WorkMode), ErrorMessage = "Hình thức chưa hợp lệ.")]
    [Display(Name = "Hình thức")] public WorkMode? WorkMode { get; set; }
    public int Page { get; set; } = 1;
    [BindNever] public int TotalCount { get; set; }
    [BindNever] public int TotalPages { get; set; }
    [BindNever] public List<PositionCardViewModel> Positions { get; set; } = [];
    [BindNever] public List<SelectListItem> Majors { get; set; } = [];
    [BindNever] public List<SelectListItem> Companies { get; set; } = [];
    [BindNever] public List<SelectListItem> Locations { get; set; } = [];
}