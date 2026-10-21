using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email."), EmailAddress(ErrorMessage = "Email chưa đúng định dạng."), StringLength(246, ErrorMessage = "Email không quá 246 ký tự.")]
    [Display(Name = "Email")] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu."), DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")] public string Password { get; set; } = "";
    [Display(Name = "Giữ đăng nhập")] public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}