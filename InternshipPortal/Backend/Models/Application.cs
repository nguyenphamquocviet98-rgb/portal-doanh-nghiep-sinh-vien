using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class Application : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int InternshipPositionId { get; set; }
    public InternshipPosition InternshipPosition { get; set; } = null!;
    [Required, MaxLength(490)] public string CvPath { get; set; } = "";
    [MaxLength(1990)] public string? CoverLetter { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    [MaxLength(1990)] public string? Note { get; set; }
}