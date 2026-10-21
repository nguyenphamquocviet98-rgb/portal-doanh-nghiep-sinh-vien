using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.CompanyApplications;

public class UpdateApplicationViewModel
{
    [Required(ErrorMessage = "Thiếu trạng thái hiện tại. Vui lòng tải lại hồ sơ."), EnumDataType(typeof(ApplicationStatus), ErrorMessage = "Trạng thái hiện tại không hợp lệ.")]
    public ApplicationStatus? ExpectedStatus { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn trạng thái mới."), EnumDataType(typeof(ApplicationStatus), ErrorMessage = "Trạng thái mới không hợp lệ.")]
    public ApplicationStatus? TargetStatus { get; set; }
    [StringLength(1990, ErrorMessage = "Ghi chú tối đa 1990 ký tự."), Display(Name = "Lý do từ chối")]
    public string? Note { get; set; }
}
