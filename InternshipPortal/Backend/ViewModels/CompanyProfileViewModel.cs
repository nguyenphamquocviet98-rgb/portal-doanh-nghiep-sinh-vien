using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.ViewModels;

public class CompanyProfileViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên doanh nghiệp."), StringLength(200), Display(Name = "Tên doanh nghiệp")]
    public string Name { get; set; } = "";
    [StringLength(30), Display(Name = "Mã số thuế")]
    public string? TaxCode { get; set; }
    [StringLength(290), Display(Name = "Website")]
    public string? Website { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ."), StringLength(290), Display(Name = "Địa chỉ")]
    public string Address { get; set; } = "";
    [StringLength(150), Display(Name = "Lĩnh vực hoạt động")]
    public string? BusinessField { get; set; }
    [StringLength(1990), Display(Name = "Giới thiệu doanh nghiệp")]
    public string? Description { get; set; }
    [BindNever] public string Email { get; set; } = "";
    [BindNever] public string ApprovalStatusName { get; set; } = "";
    [BindNever] public string? ApprovalNote { get; set; }
}
