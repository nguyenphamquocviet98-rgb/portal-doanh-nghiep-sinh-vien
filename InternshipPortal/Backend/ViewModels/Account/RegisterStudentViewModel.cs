using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternshipPortal.ViewModels.Account;

public class RegisterStudentViewModel : RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập mã sinh viên."), StringLength(30, ErrorMessage = "Mã sinh viên không quá 30 ký tự.")]
    [Display(Name = "Mã sinh viên")] public string StudentCode { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập lớp."), StringLength(50, ErrorMessage = "Tên lớp không quá 50 ký tự.")]
    [Display(Name = "Lớp")] public string ClassName { get; set; } = "";
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn khoa.")]
    [Display(Name = "Khoa")] public int FacultyId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ngành.")]
    [Display(Name = "Ngành")] public int MajorId { get; set; }
    [BindNever] public List<SelectListItem> Faculties { get; set; } = [];
    [BindNever] public List<MajorOptionViewModel> Majors { get; set; } = [];
}

public class MajorOptionViewModel
{
    public int Id { get; set; }
    public int FacultyId { get; set; }
    public string Name { get; set; } = "";
}