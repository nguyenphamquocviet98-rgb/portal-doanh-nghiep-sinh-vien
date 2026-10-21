using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.ViewModels.CompanyApplications;

public class CompanyApplicationListViewModel
{
    [StringLength(200, ErrorMessage = "Từ khóa tối đa 200 ký tự."), Display(Name = "Tên hoặc mã sinh viên")]
    public string? Keyword { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Vị trí không hợp lệ."), Display(Name = "Vị trí")]
    public int? PositionId { get; set; }
    [EnumDataType(typeof(ApplicationStatus), ErrorMessage = "Trạng thái không hợp lệ."), Display(Name = "Trạng thái")]
    public ApplicationStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    [BindNever] public int TotalPages { get; set; }
    [BindNever] public int TotalCount { get; set; }
    [BindNever] public List<SelectListItem> Positions { get; set; } = [];
    [BindNever] public List<CompanyApplicationDetailsViewModel> Applications { get; set; } = [];
}

public class CompanyApplicationDetailsViewModel
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public string PositionTitle { get; set; } = "";
    public string FullName { get; set; } = "";
    public string StudentCode { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string ClassName { get; set; } = "";
    public string MajorName { get; set; } = "";
    public string FacultyName { get; set; } = "";
    public decimal? Gpa { get; set; }
    public string? Skills { get; set; }
    public string? Bio { get; set; }
    public string? CoverLetter { get; set; }
    public string? Note { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasCv { get; set; }
    public UpdateApplicationViewModel Decision { get; set; } = new();
}
