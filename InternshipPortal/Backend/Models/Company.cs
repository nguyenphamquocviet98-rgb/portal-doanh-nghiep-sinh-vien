using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class Company : BaseEntity
{
    [Required, MaxLength(440)] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    [MaxLength(30)] public string? TaxCode { get; set; }
    [MaxLength(290)] public string? Website { get; set; }
    [Required, MaxLength(290)] public string Address { get; set; } = "";
    [MaxLength(150)] public string? BusinessField { get; set; }
    [MaxLength(1990)] public string? Description { get; set; }
    public CompanyApprovalStatus ApprovalStatus { get; set; } = CompanyApprovalStatus.Pending;
    [MaxLength(990)] public string? ApprovalNote { get; set; }
    public bool IsActive { get; set; } = true;
}
