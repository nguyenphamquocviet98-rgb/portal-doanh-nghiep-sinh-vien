using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class InternshipPosition : BaseEntity
{
    [Required, MaxLength(30)] public string Code { get; set; } = "";
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public int MajorId { get; set; }
    public Major Major { get; set; } = null!;
    public int InternshipPeriodId { get; set; }
    public InternshipPeriod InternshipPeriod { get; set; } = null!;
    [Required, MaxLength(200)] public string Title { get; set; } = "";
    [Required, MaxLength(2990)] public string Description { get; set; } = "";
    [Required, MaxLength(1990)] public string Requirements { get; set; } = "";
    public int Quantity { get; set; } = 1;
    [Required, MaxLength(200)] public string Location { get; set; } = "";
    public WorkMode WorkMode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public InternshipPositionStatus Status { get; set; } = InternshipPositionStatus.Open;
}
