using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class InternshipDocument : BaseEntity
{
    public int InternshipId { get; set; }
    public Internship Internship { get; set; } = null!;
    public DocumentType DocumentType { get; set; }
    [Required, MaxLength(200)] public string OriginalFileName { get; set; } = "";
    [Required, MaxLength(490)] public string FilePath { get; set; } = "";
    public long FileSize { get; set; }
}