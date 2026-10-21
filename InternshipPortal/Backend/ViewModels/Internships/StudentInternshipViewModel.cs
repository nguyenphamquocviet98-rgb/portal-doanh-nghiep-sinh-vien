using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.Internships;

public class StudentInternshipViewModel
{
    public int Id { get; set; }
    public string PositionTitle { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public string PeriodName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public InternshipStatus Status { get; set; }
    public string? SupervisorName { get; set; }
    public string? SupervisorEmail { get; set; }
    public string? SupervisorPhone { get; set; }
    public int DiaryCount { get; set; }
    public int TotalWeeks { get; set; }
    public bool CanWriteDiary => Status == InternshipStatus.InProgress;
    public List<DiaryDetailsViewModel> Diaries { get; set; } = [];
}

public class DiaryDetailsViewModel
{
    public int Id { get; set; }
    public int InternshipId { get; set; }
    public int WeekNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string WorkContent { get; set; } = "";
    public string Results { get; set; } = "";
    public string? Difficulties { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool CanEdit { get; set; }
}
