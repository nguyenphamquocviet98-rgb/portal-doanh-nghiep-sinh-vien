using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipPortal.Models;

public class Evaluation : BaseEntity
{
    public int InternshipId { get; set; }
    public Internship Internship { get; set; } = null!;
    public decimal ProfessionalKnowledge { get; set; }
    public decimal WorkSkills { get; set; }
    public decimal Attitude { get; set; }
    public decimal Responsibility { get; set; }
    public decimal Communication { get; set; }
    public decimal TaskCompletion { get; set; }
    [Required, MaxLength(2990)] public string Comment { get; set; } = "";
    [NotMapped] public decimal AverageScore =>
        (ProfessionalKnowledge + WorkSkills + Attitude + Responsibility + Communication + TaskCompletion) / 6m;
}