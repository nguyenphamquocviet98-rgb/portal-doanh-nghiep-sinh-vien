using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using InternshipPortal.Helpers;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Internships;
namespace InternshipPortal.ViewModels.CompanyInternships;
public class CompanyEvaluationViewModel : IValidatableObject
{
    public int InternshipId { get; set; }
    public string FullName { get; set; } = "";
    public InternshipStatus Status { get; set; }
    public StudentEvaluationViewModel? Existing { get; set; }
    public bool CanCreate => Existing is null && Status is InternshipStatus.Completed or InternshipStatus.Failed;
    [Display(Name = "Kiến thức chuyên môn"), Required(ErrorMessage = "Nhập điểm kiến thức chuyên môn."), Range(typeof(decimal), "0", "10", ErrorMessage = "Điểm phải từ 0 đến 10.")]
    [ModelBinder(BinderType = typeof(EvaluationScoreModelBinder))] public decimal? ProfessionalKnowledge { get; set; }
    [Display(Name = "Kỹ năng làm việc"), Required(ErrorMessage = "Nhập điểm kỹ năng làm việc."), Range(typeof(decimal), "0", "10", ErrorMessage = "Điểm phải từ 0 đến 10.")]
    [ModelBinder(BinderType = typeof(EvaluationScoreModelBinder))] public decimal? WorkSkills { get; set; }
    [Display(Name = "Thái độ"), Required(ErrorMessage = "Nhập điểm thái độ."), Range(typeof(decimal), "0", "10", ErrorMessage = "Điểm phải từ 0 đến 10.")]
    [ModelBinder(BinderType = typeof(EvaluationScoreModelBinder))] public decimal? Attitude { get; set; }
    [Display(Name = "Tinh thần trách nhiệm"), Required(ErrorMessage = "Nhập điểm tinh thần trách nhiệm."), Range(typeof(decimal), "0", "10", ErrorMessage = "Điểm phải từ 0 đến 10.")]
    [ModelBinder(BinderType = typeof(EvaluationScoreModelBinder))] public decimal? Responsibility { get; set; }
    [Display(Name = "Khả năng giao tiếp"), Required(ErrorMessage = "Nhập điểm khả năng giao tiếp."), Range(typeof(decimal), "0", "10", ErrorMessage = "Điểm phải từ 0 đến 10.")]
    [ModelBinder(BinderType = typeof(EvaluationScoreModelBinder))] public decimal? Communication { get; set; }
    [Display(Name = "Mức độ hoàn thành công việc"), Required(ErrorMessage = "Nhập điểm mức độ hoàn thành công việc."), Range(typeof(decimal), "0", "10", ErrorMessage = "Điểm phải từ 0 đến 10.")]
    [ModelBinder(BinderType = typeof(EvaluationScoreModelBinder))] public decimal? TaskCompletion { get; set; }
    [Display(Name = "Nhận xét"), Required(ErrorMessage = "Nhập nhận xét đánh giá."), StringLength(2990, ErrorMessage = "Nhận xét tối đa 2990 ký tự.")]
    public string Comment { get; set; } = "";
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (ProfessionalKnowledge.HasValue && decimal.Round(ProfessionalKnowledge.Value, 2) != ProfessionalKnowledge.Value)
            yield return new ValidationResult("Điểm nhận tối đa hai chữ số thập phân.", [nameof(ProfessionalKnowledge)]);
        if (WorkSkills.HasValue && decimal.Round(WorkSkills.Value, 2) != WorkSkills.Value)
            yield return new ValidationResult("Điểm nhận tối đa hai chữ số thập phân.", [nameof(WorkSkills)]);
        if (Attitude.HasValue && decimal.Round(Attitude.Value, 2) != Attitude.Value)
            yield return new ValidationResult("Điểm nhận tối đa hai chữ số thập phân.", [nameof(Attitude)]);
        if (Responsibility.HasValue && decimal.Round(Responsibility.Value, 2) != Responsibility.Value)
            yield return new ValidationResult("Điểm nhận tối đa hai chữ số thập phân.", [nameof(Responsibility)]);
        if (Communication.HasValue && decimal.Round(Communication.Value, 2) != Communication.Value)
            yield return new ValidationResult("Điểm nhận tối đa hai chữ số thập phân.", [nameof(Communication)]);
        if (TaskCompletion.HasValue && decimal.Round(TaskCompletion.Value, 2) != TaskCompletion.Value)
            yield return new ValidationResult("Điểm nhận tối đa hai chữ số thập phân.", [nameof(TaskCompletion)]);
    }
}

