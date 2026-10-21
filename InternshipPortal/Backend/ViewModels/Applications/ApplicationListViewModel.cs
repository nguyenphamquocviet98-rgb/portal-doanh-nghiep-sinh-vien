using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.ViewModels.Applications;

public class ApplicationListViewModel
{
    [EnumDataType(typeof(ApplicationStatus), ErrorMessage = "Trạng thái chưa hợp lệ.")]
    [Display(Name = "Trạng thái")] public ApplicationStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    [BindNever] public int TotalCount { get; set; }
    [BindNever] public int TotalPages { get; set; }
    [BindNever] public List<ApplicationDetailsViewModel> Applications { get; set; } = [];
}