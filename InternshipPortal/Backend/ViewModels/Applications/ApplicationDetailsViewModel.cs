using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.Applications;

public class ApplicationDetailsViewModel
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public string PositionTitle { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public ApplicationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CoverLetter { get; set; }
    public string? Note { get; set; }
    public bool HasCv { get; set; }
    public bool CanWithdraw => Status == ApplicationStatus.Pending;
}