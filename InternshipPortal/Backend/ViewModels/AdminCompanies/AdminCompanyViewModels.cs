using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
namespace InternshipPortal.ViewModels.AdminCompanies;
public class AdminCompanyListViewModel
{
    [Display(Name = "Tên hoặc email"), StringLength(200)] public string? Keyword { get; set; }
    [Display(Name = "Trạng thái"), EnumDataType(typeof(CompanyApprovalStatus))] public CompanyApprovalStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<AdminCompanyDetailsViewModel> Companies { get; set; } = [];
}
public class AdminCompanyDecisionViewModel
{
    [Required, StringLength(30)] public string Version { get; set; } = "";
    [Required, EnumDataType(typeof(CompanyApprovalStatus))] public CompanyApprovalStatus? TargetStatus { get; set; }
    [Display(Name = "Ghi chú / lý do từ chối"), StringLength(990, ErrorMessage = "Ghi chú tối đa 990 ký tự.")] public string? Note { get; set; }
}
public class AdminCompanyDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string? TaxCode { get; set; }
    public string? Website { get; set; }
    public string? BusinessField { get; set; }
    public string? Description { get; set; }
    public string? ApprovalNote { get; set; }
    public CompanyApprovalStatus Status { get; set; }
    public bool IsActive { get; set; }
    public AdminCompanyDecisionViewModel Decision { get; set; } = new();
    public bool CanDecide => IsActive && Status == CompanyApprovalStatus.Pending;
}
