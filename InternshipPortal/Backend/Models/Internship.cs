using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class Internship : BaseEntity
{
    public int ApplicationId { get; set; }
    public Application Application { get; set; } = null!;
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int InternshipPeriodId { get; set; }
    public InternshipPeriod InternshipPeriod { get; set; } = null!;
    [MaxLength(150)] public string? SupervisorName { get; set; }
    [MaxLength(246)] public string? SupervisorEmail { get; set; }
    [MaxLength(30)] public string? SupervisorPhone { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
}