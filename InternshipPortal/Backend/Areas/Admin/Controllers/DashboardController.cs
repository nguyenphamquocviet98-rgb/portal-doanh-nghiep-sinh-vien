using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.AdminDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = "Admin")]
public class DashboardController(UserManager<ApplicationUser> users, ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var user = await users.GetUserAsync(User);
        if (user is null) return Challenge();

        // 1. Thống kê tổng số lượng
        var totalStudents = await db.Students.CountAsync(x => x.User.IsActive, ct);
        var totalCompanies = await db.Companies.CountAsync(x => x.IsActive && x.User.IsActive && x.ApprovalStatus == CompanyApprovalStatus.Approved, ct);
        var pendingCompanies = await db.Companies.CountAsync(x => x.IsActive && x.User.IsActive && x.ApprovalStatus == CompanyApprovalStatus.Pending, ct);
        var totalPositions = await db.InternshipPositions.CountAsync(ct);
        var openPositions = await db.InternshipPositions.CountAsync(x => x.Status == InternshipPositionStatus.Open, ct);
        var totalApplications = await db.Applications.CountAsync(ct);
        var totalInternships = await db.Internships.CountAsync(ct);
        var applicationsInReview = await db.Applications.CountAsync(x => x.Status == ApplicationStatus.Pending
            || x.Status == ApplicationStatus.Viewed || x.Status == ApplicationStatus.Interviewed, ct);
        var activeInternships = await db.Internships.CountAsync(x => x.Status == InternshipStatus.InProgress, ct);
        var internshipsWithoutDiary = await db.Internships.CountAsync(x => x.Status == InternshipStatus.InProgress
            && !db.InternshipDiaries.Any(d => d.InternshipId == x.Id), ct);
        var currentPeriod = await db.InternshipPeriods.AsNoTracking()
            .Where(x => x.Status == InternshipPeriodStatus.Open)
            .OrderByDescending(x => x.StartDate)
            .Select(x => new { x.Name, x.StartDate, x.EndDate })
            .FirstOrDefaultAsync(ct);

        // 2. Biểu đồ 1: Tỷ lệ sinh viên có nơi thực tập
        var placedCount = await db.Internships.Select(x => x.StudentId).Distinct().CountAsync(ct);
        var unplacedCount = Math.Max(0, totalStudents - placedCount);

        // 3. Biểu đồ 2: Số lượng đơn ứng tuyển theo chuyên ngành
        var majorStats = await db.Majors.AsNoTracking()
            .Where(m => m.IsActive)
            .Select(m => new
            {
                Name = m.Name,
                Count = db.Applications.Count(a => a.Student.MajorId == m.Id)
            })
            .OrderByDescending(x => x.Count)
            .Take(6)
            .ToListAsync(ct);

        // 4. Biểu đồ 3: Phân bố trạng thái thực tập
        var notStarted = await db.Internships.CountAsync(x => x.Status == InternshipStatus.NotStarted, ct);
        var inProgress = await db.Internships.CountAsync(x => x.Status == InternshipStatus.InProgress, ct);
        var completed = await db.Internships.CountAsync(x => x.Status == InternshipStatus.Completed, ct);
        var failed = await db.Internships.CountAsync(x => x.Status == InternshipStatus.Failed, ct);
        var recentApplications = await db.Applications.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id).Take(5)
            .Select(x => new AdminRecentApplicationViewModel
            {
                Id = x.Id,
                StudentName = x.Student.User.FullName,
                StudentCode = x.Student.StudentCode,
                PositionTitle = x.InternshipPosition.Title,
                CompanyName = x.InternshipPosition.Company.Name,
                CreatedAt = x.CreatedAt,
                Status = x.Status
            }).ToListAsync(ct);

        var model = new AdminDashboardViewModel
        {
            FullName = user.FullName,
            RoleName = "Nhà trường",
            Heading = "Bàn làm việc Quản trị",
            Detail = "Giám sát số liệu, đợt thực tập và hoạt động kết nối doanh nghiệp.",
            
            TotalStudents = totalStudents,
            TotalCompanies = totalCompanies,
            PendingCompanies = pendingCompanies,
            TotalPositions = totalPositions,
            OpenPositions = openPositions,
            TotalApplications = totalApplications,
            TotalInternships = totalInternships,
            ApplicationsInReview = applicationsInReview,
            ActiveInternships = activeInternships,
            InternshipsWithoutDiary = internshipsWithoutDiary,
            CurrentPeriodName = currentPeriod?.Name,
            CurrentPeriodStartDate = currentPeriod?.StartDate,
            CurrentPeriodEndDate = currentPeriod?.EndDate,
            RecentApplications = recentApplications,

            PlacedStudentsCount = placedCount,
            UnplacedStudentsCount = unplacedCount,

            MajorLabels = majorStats.Select(x => x.Name).ToList(),
            MajorApplicationCounts = majorStats.Select(x => x.Count).ToList(),

            NotStartedCount = notStarted,
            InProgressCount = inProgress,
            CompletedCount = completed,
            FailedCount = failed
        };

        return View(model);
    }
}
