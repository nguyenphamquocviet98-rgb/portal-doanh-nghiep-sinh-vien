using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace InternshipPortal.Models;

public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(150)] public string FullName { get; set; } = "";
    [MaxLength(490)] public string? AvatarPath { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}