using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên."), StringLength(150, ErrorMessage = "Họ tên không quá 150 ký tự.")]
    [Display(Name = "Họ và tên")] public string FullName { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập email."), EmailAddress(ErrorMessage = "Email chưa đúng định dạng."), StringLength(246, ErrorMessage = "Email không quá 246 ký tự.")]
    [Display(Name = "Email")] public string Email { get; set; } = "";
    [StringLength(30, ErrorMessage = "Số điện thoại không quá 30 ký tự."), Phone(ErrorMessage = "Số điện thoại chưa hợp lệ.")]
    [Display(Name = "Số điện thoại")] public string? PhoneNumber { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu."), StringLength(100, MinimumLength = 10, ErrorMessage = "Mật khẩu cần từ 10 đến 100 ký tự."), DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")] public string Password { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu."), DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Hai mật khẩu chưa khớp.")]
    [Display(Name = "Nhập lại mật khẩu")] public string ConfirmPassword { get; set; } = "";
}