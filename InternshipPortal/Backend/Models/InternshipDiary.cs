using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models;

public class InternshipDiary : BaseEntity
{
    public int InternshipId { get; set; }
    public Internship Internship { get; set; } = null!;
    public int WeekNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    [Required, MaxLength(2990)] public string WorkContent { get; set; } = "";
    [Required, MaxLength(2990)] public string Results { get; set; } = "";
    [MaxLength(2990)] public string? Difficulties { get; set; }
    [MaxLength(1990)] public string? Note { get; set; }
}
