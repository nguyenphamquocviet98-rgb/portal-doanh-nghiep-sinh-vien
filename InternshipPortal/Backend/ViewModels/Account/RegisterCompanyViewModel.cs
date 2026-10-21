using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.ViewModels.Account;

public class RegisterCompanyViewModel : RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên doanh nghiệp."), StringLength(200, ErrorMessage = "Tên doanh nghiệp không quá 200 ký tự.")]
    [Display(Name = "Tên doanh nghiệp")] public string CompanyName { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ."), StringLength(290, ErrorMessage = "Địa chỉ không quá 290 ký tự.")]
    [Display(Name = "Địa chỉ doanh nghiệp")] public string Address { get; set; } = "";
    [StringLength(30, ErrorMessage = "Mã số thuế không quá 30 ký tự.")]
    [Display(Name = "Mã số thuế")] public string? TaxCode { get; set; }
    [StringLength(290, ErrorMessage = "Website không quá 290 ký tự."), Url(ErrorMessage = "Website cần bắt đầu bằng http:// hoặc https://.")]
    [Display(Name = "Website")] public string? Website { get; set; }
}