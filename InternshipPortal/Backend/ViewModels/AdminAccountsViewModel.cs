using System.ComponentModel.DataAnnotations;
namespace InternshipPortal.ViewModels;
public class AdminAccountsViewModel
{
    [StringLength(200), Display(Name = "Tên, email hoặc mã sinh viên")] public string? Keyword { get; set; }
    [Display(Name = "Nhóm tài khoản")] public string? Role { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<AdminAccountDetailsViewModel> Accounts { get; set; } = [];
}
public class AdminAccountDetailsViewModel
{
    public string Id { get; set; } = "";
    public string FullName { get; set; } = "";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public string? Version { get; set; }
    public List<string> Roles { get; set; } = [];
    public string? StudentCode { get; set; }
    public string? ClassName { get; set; }
    public string? MajorName { get; set; }
    public string? FacultyName { get; set; }
    public string? CompanyName { get; set; }
    public bool CanToggle => !Roles.Contains("Admin");
}
public class ToggleAccountViewModel
{
    [Required, StringLength(100)] public string Version { get; set; } = "";
    [Required] public bool? IsActive { get; set; }
}
