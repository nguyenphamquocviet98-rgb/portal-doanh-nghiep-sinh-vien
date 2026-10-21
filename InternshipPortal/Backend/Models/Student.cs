using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models;

public class Student : BaseEntity
{
    [Required, MaxLength(440)] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    [Required, MaxLength(30)] public string StudentCode { get; set; } = "";
    public int MajorId { get; set; }
    public Major Major { get; set; } = null!;
    [Required, MaxLength(50)] public string ClassName { get; set; } = "";
    public decimal? Gpa { get; set; }
    [MaxLength(490)] public string? CvPath { get; set; }
    [MaxLength(990)] public string? Skills { get; set; }
    [MaxLength(1990)] public string? Bio { get; set; }
}