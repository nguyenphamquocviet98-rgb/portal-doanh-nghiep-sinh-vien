using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;

namespace InternshipPortal.Models;

public class Notification : BaseEntity
{
    [Required, MaxLength(440)] public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;
    public NotificationType Type { get; set; }
    [Required, MaxLength(200)] public string Title { get; set; } = "";
    [Required, MaxLength(1990)] public string Message { get; set; } = "";
    [MaxLength(490)] public string? TargetUrl { get; set; }
    [MaxLength(200)] public string? DeduplicationKey { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}