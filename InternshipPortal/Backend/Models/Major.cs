using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models;

public class Major : BaseEntity
{
    public int FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;
    [Required, MaxLength(20)] public string Code { get; set; } = "";
    [Required, MaxLength(150)] public string Name { get; set; } = "";
    public bool IsActive { get; set; } = true;
}