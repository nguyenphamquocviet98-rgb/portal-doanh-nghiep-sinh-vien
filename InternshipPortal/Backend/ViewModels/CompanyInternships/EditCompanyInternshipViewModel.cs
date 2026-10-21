using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.CompanyInternships;

public class EditCompanyInternshipViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    [Required(ErrorMessage = "Thiếu phiên bản hồ sơ. Hãy tải lại trang."), StringLength(30)]
    public string Version { get; set; } = "";
    [Required(ErrorMessage = "Chọn trạng thái thực tập."), EnumDataType(typeof(InternshipStatus), ErrorMessage = "Trạng thái không hợp lệ.")]
    [Display(Name = "Trạng thái thực tập")]
    public InternshipStatus? Status { get; set; }
    public InternshipStatus CurrentStatus { get; set; }
    [Required(ErrorMessage = "Nhập tên người hướng dẫn."), StringLength(150, ErrorMessage = "Tên tối đa 150 ký tự.")]
    [Display(Name = "Tên người hướng dẫn")]
    public string SupervisorName { get; set; } = "";
    [EmailAddress(ErrorMessage = "Email không hợp lệ."), StringLength(246, ErrorMessage = "Email tối đa 246 ký tự.")]
    [Display(Name = "Email người hướng dẫn")]
    public string? SupervisorEmail { get; set; }
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ."), StringLength(30, ErrorMessage = "Số điện thoại tối đa 30 ký tự.")]
    [Display(Name = "Điện thoại người hướng dẫn")]
    public string? SupervisorPhone { get; set; }
}

public record CompanyInternshipUpdateResult(bool Succeeded, string Message, bool NotFound = false);
