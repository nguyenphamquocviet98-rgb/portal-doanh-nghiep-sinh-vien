using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Areas.Student.Controllers;

[Area("Student"), Authorize(Roles = "Student")]
public class DashboardController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = users.GetUserId(User);
        var student = await db.Students.AsNoTracking().Include(x => x.User).Include(x => x.Major)
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (student is null) return Forbid();
        return View(new PortalViewModel
        {
            FullName = student.User.FullName, RoleName = "Sinh viên", Heading = "Chào bạn,",
            Detail = $"{student.StudentCode} · {student.ClassName} · {student.Major.Name}"
        });
    }
}