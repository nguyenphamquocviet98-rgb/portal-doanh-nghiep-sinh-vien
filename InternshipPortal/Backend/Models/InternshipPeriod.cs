using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class InternshipPeriod : BaseEntity
{
    [Required, MaxLength(30)] public string Code { get; set; } = "";
    [Required, MaxLength(150)] public string Name { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public InternshipPeriodStatus Status { get; set; } = InternshipPeriodStatus.Planned;
}