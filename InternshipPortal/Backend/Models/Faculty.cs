using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models;

public class Faculty : BaseEntity
{
    [Required, MaxLength(20)] public string Code { get; set; } = "";
    [Required, MaxLength(150)] public string Name { get; set; } = "";
    public bool IsActive { get; set; } = true;
}