using System.ComponentModel.DataAnnotations;
using InternshipPortal.ViewModels.Account;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternshipPortal.ViewModels;

public class EditStudentProfileViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên."), StringLength(150, ErrorMessage = "Họ tên không quá 150 ký tự.")]
    [Display(Name = "Họ và tên")] public string FullName { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập mã sinh viên."), StringLength(30, ErrorMessage = "Mã sinh viên không quá 30 ký tự.")]
    [Display(Name = "Mã sinh viên")] public string StudentCode { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập lớp."), StringLength(50, ErrorMessage = "Lớp không quá 50 ký tự.")]
    [Display(Name = "Lớp")] public string ClassName { get; set; } = "";
    [StringLength(30, ErrorMessage = "Số điện thoại không quá 30 ký tự."), Phone(ErrorMessage = "Số điện thoại chưa hợp lệ.")]
    [Display(Name = "Điện thoại")] public string? PhoneNumber { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn khoa.")]
    [Display(Name = "Khoa")] public int FacultyId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ngành.")]
    [Display(Name = "Ngành")] public int MajorId { get; set; }
    [StringLength(990, ErrorMessage = "Kỹ năng không quá 990 ký tự.")]
    [Display(Name = "Kỹ năng")] public string? Skills { get; set; }
    [StringLength(1990, ErrorMessage = "Giới thiệu không quá 1990 ký tự.")]
    [Display(Name = "Giới thiệu bản thân")] public string? Bio { get; set; }
    [BindNever] public string Email { get; set; } = "";
    [BindNever] public List<SelectListItem> Faculties { get; set; } = [];
    [BindNever] public List<MajorOptionViewModel> Majors { get; set; } = [];
}