using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Internships;

namespace InternshipPortal.ViewModels.CompanyInternships;

public class CompanyInternshipListViewModel
{
    [Display(Name = "Tên hoặc mã sinh viên"), StringLength(200)] public string? Keyword { get; set; }
    [Display(Name = "Trạng thái"), EnumDataType(typeof(InternshipStatus), ErrorMessage = "Trạng thái không hợp lệ.")] public InternshipStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<CompanyInternshipViewModel> Internships { get; set; } = [];
}

public class CompanyInternshipViewModel
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string FullName { get; set; } = "";
    public string StudentCode { get; set; } = "";
    public string PositionTitle { get; set; } = "";
    public string PeriodName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public InternshipStatus Status { get; set; }
    public string? SupervisorName { get; set; }
    public string? SupervisorEmail { get; set; }
    public string? SupervisorPhone { get; set; }
    public List<DiaryDetailsViewModel> Diaries { get; set; } = [];
}
