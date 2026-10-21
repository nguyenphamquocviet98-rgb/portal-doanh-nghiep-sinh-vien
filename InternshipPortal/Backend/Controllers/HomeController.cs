using System.Diagnostics;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Home;
using InternshipPortal.ViewModels.Internships;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Controllers;

public class HomeController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var openPositionsQuery = db.InternshipPositions.AsNoTracking()
            .Where(x => x.Status == InternshipPositionStatus.Open && x.ApplicationDeadline >= now
                && x.Company.ApprovalStatus == CompanyApprovalStatus.Approved && x.Company.IsActive && x.Company.User.IsActive);

        var latestPositions = await openPositionsQuery
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(9)
            .Select(x => new PositionCardViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Title = x.Title,
                CompanyId = x.CompanyId,
                CompanyName = x.Company.Name,
                MajorName = x.Major.Name,
                Location = x.Location,
                WorkMode = x.WorkMode,
                ApplicationDeadline = x.ApplicationDeadline,
                Quantity = x.Quantity,
                AcceptedCount = db.Applications.Count(a => a.InternshipPositionId == x.Id && a.Status == ApplicationStatus.Approved)
            })
            .ToListAsync(ct);

        var featuredCompanies = await db.Companies.AsNoTracking()
            .Where(x => x.ApprovalStatus == CompanyApprovalStatus.Approved && x.IsActive && x.User.IsActive)
            .OrderBy(x => x.Name)
            .Take(6)
            .Select(x => new HomeCompanyItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                BusinessField = x.BusinessField,
                OpenPositionsCount = db.InternshipPositions.Count(p => p.CompanyId == x.Id && p.Status == InternshipPositionStatus.Open && p.ApplicationDeadline >= now)
            })
            .ToListAsync(ct);

        var currentPeriod = await db.InternshipPeriods.AsNoTracking()
            .Where(x => x.Status == InternshipPeriodStatus.Open)
            .OrderByDescending(x => x.StartDate)
            .FirstOrDefaultAsync(ct);

        var majorOptions = await db.Majors.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToListAsync(ct);

        var locationOptions = await openPositionsQuery
            .Select(x => x.Location)
            .Distinct()
            .OrderBy(x => x)
            .Select(x => new SelectListItem(x, x))
            .ToListAsync(ct);

        var totalPositions = await openPositionsQuery.CountAsync(ct);
        var totalCompanies = await db.Companies.CountAsync(x => x.ApprovalStatus == CompanyApprovalStatus.Approved && x.IsActive && x.User.IsActive, ct);
        var totalPlaced = await db.Applications.CountAsync(x => x.Status == ApplicationStatus.Approved, ct);

        var model = new HomeIndexViewModel
        {
            LatestPositions = latestPositions,
            FeaturedCompanies = featuredCompanies,
            CurrentPeriod = currentPeriod,
            MajorOptions = majorOptions,
            LocationOptions = locationOptions,
            TotalOpenPositions = totalPositions,
            TotalApprovedCompanies = totalCompanies,
            TotalPlacedStudents = totalPlaced
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
