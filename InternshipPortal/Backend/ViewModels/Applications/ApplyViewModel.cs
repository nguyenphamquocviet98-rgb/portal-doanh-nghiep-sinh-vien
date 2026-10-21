using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.ViewModels.Applications;

public class ApplyViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Vị trí thực tập chưa hợp lệ.")]
    public int PositionId { get; set; }
    [StringLength(1990, ErrorMessage = "Lời giới thiệu không quá 1990 ký tự.")]
    [Display(Name = "Lời giới thiệu")] public string? CoverLetter { get; set; }
    [BindNever] public string Title { get; set; } = "";
    [BindNever] public string CompanyName { get; set; } = "";
    [BindNever] public string Location { get; set; } = "";
    [BindNever] public WorkMode WorkMode { get; set; }
    [BindNever] public DateTime ApplicationDeadline { get; set; }
    [BindNever] public bool HasCv { get; set; }
    [BindNever] public string? BlockReason { get; set; }
    [BindNever] public int? ExistingApplicationId { get; set; }
    public bool CanSubmit => BlockReason is null;
}