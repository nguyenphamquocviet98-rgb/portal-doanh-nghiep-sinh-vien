using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace InternshipPortal.ViewModels;
public class AcademicUnitViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Nhập mã."), StringLength(20)] [Display(Name = "Mã")] public string Code { get; set; } = "";
    [Required(ErrorMessage = "Nhập tên."), StringLength(150)] [Display(Name = "Tên")] public string Name { get; set; } = "";
    [Display(Name = "Đang hoạt động")] public bool IsActive { get; set; } = true;
    [Display(Name = "Khoa")] public int? FacultyId { get; set; }
    public string? FacultyName { get; set; }
    public string? Version { get; set; }
    public bool IsMajor { get; set; }
    public List<SelectListItem> Faculties { get; set; } = [];
}
