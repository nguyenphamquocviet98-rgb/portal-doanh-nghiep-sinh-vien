using System.ComponentModel.DataAnnotations;
using System.Reflection;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.CompanyDashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Areas.Company.Controllers;

[Area("Company"), Authorize(Roles = "Company")]
public class DashboardController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = users.GetUserId(User);
        var company = await db.Companies.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (company is null || !company.IsActive) return Forbid();

        var status = typeof(CompanyApprovalStatus).GetField(company.ApprovalStatus.ToString())?.GetCustomAttribute<DisplayAttribute>()?.Name;
        var canManage = company.ApprovalStatus == CompanyApprovalStatus.Approved;

        // 1. Thống kê Vị trí tuyển dụng
        var positionsQuery = db.InternshipPositions.AsNoTracking().Where(p => p.CompanyId == company.Id);
        var totalPositions = await positionsQuery.CountAsync(cancellationToken);
        var openPositions = await positionsQuery.CountAsync(p => p.Status == InternshipPositionStatus.Open, cancellationToken);

        // 2. Thống kê Hồ sơ ứng tuyển
        var applicationsQuery = db.Applications.AsNoTracking().Where(a => a.InternshipPosition.CompanyId == company.Id);
        var totalApplications = await applicationsQuery.CountAsync(cancellationToken);
        var pendingApplications = await applicationsQuery.CountAsync(a => a.Status == ApplicationStatus.Pending, cancellationToken);

        // 3. Thống kê Thực tập
        var internshipsQuery = db.Internships.AsNoTracking().Where(i => i.Application.InternshipPosition.CompanyId == company.Id);
        var totalInternships = await internshipsQuery.CountAsync(cancellationToken);
        var activeInternships = await internshipsQuery.CountAsync(i => i.Status == InternshipStatus.InProgress, cancellationToken);

        // 4. Thống kê Nhật ký & Đánh giá
        var totalDiaries = await db.InternshipDiaries.AsNoTracking()
            .Where(d => d.Internship.Application.InternshipPosition.CompanyId == company.Id)
            .CountAsync(cancellationToken);

        var pendingEvaluations = await db.Internships.AsNoTracking()
            .Where(i => i.Application.InternshipPosition.CompanyId == company.Id
                        && (i.Status == InternshipStatus.Completed || i.Status == InternshipStatus.Failed)
                        && !db.Evaluations.Any(e => e.InternshipId == i.Id))
            .CountAsync(cancellationToken);

        // Danh sách nhanh: Top 5 ứng viên mới nhất
        var recentApplicants = await applicationsQuery
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .Select(a => new CompanyRecentApplicantViewModel
            {
                Id = a.Id,
                FullName = a.Student.User.FullName,
                StudentCode = a.Student.StudentCode,
                PositionTitle = a.InternshipPosition.Title,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);

        // Danh sách nhanh: Top 5 thực tập sinh theo dõi gần đây
        var recentInterns = await internshipsQuery
            .OrderByDescending(i => i.CreatedAt)
            .Take(5)
            .Select(i => new CompanyActiveInternViewModel
            {
                Id = i.Id,
                FullName = i.Student.User.FullName,
                StudentCode = i.Student.StudentCode,
                PositionTitle = i.Application.InternshipPosition.Title,
                Status = i.Status,
                SupervisorName = i.SupervisorName,
                DiaryCount = db.InternshipDiaries.Count(d => d.InternshipId == i.Id),
                HasEvaluation = db.Evaluations.Any(e => e.InternshipId == i.Id)
            })
            .ToListAsync(cancellationToken);

        var model = new CompanyDashboardViewModel
        {
            FullName = company.Name,
            RoleName = "Doanh nghiệp",
            Heading = "Cùng tạo cơ hội,",
            Detail = $"Trạng thái: {status} · {company.Address}",
            Notice = company.ApprovalStatus == CompanyApprovalStatus.Pending ? "Hồ sơ đang chờ nhà trường duyệt. Bạn chưa được phép đăng vị trí thực tập."
                : company.ApprovalStatus == CompanyApprovalStatus.Rejected ? $"Hồ sơ chưa được duyệt. {company.ApprovalNote}" : null,
            OpenPositions = openPositions,
            TotalPositions = totalPositions,
            PendingApplications = pendingApplications,
            TotalApplications = totalApplications,
            ActiveInternships = activeInternships,
            TotalInternships = totalInternships,
            TotalDiaries = totalDiaries,
            PendingEvaluations = pendingEvaluations,
            CanManage = canManage,
            RecentApplicants = recentApplicants,
            RecentInterns = recentInterns
        };

        return View(model);
    }
}