using InternshipPortal.Data;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace InternshipPortal.Areas.Admin.Controllers;
[Area("Admin"), Authorize(Roles = "Admin"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class MonitoringController(ApplicationDbContext db) : Controller
{
    private async Task<IActionResult> ListAsync(IQueryable<AdminMonitoringRow> query, AdminMonitoringViewModel model, string kind, CancellationToken ct)
    {
        if (!ModelState.IsValid) model.Keyword = null;
        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            var term = model.Keyword.Trim();
            query = kind == "Positions"
                ? query.Where(x => x.Title.Contains(term) || x.Company.Contains(term))
                : query.Where(x => x.Title.Contains(term) || x.Company.Contains(term) || (x.Student != null && x.Student.Contains(term)) || (x.StudentCode != null && x.StudentCode.Contains(term)));
        }
        model.Kind = kind;
        model.TotalCount = await query.CountAsync(ct); model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d)); model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Rows = await query.OrderByDescending(x => x.Id).Skip((model.Page - 1) * 10).Take(10).ToListAsync(ct);
        return View("Index", model);
    }
    public Task<IActionResult> Positions(AdminMonitoringViewModel model, CancellationToken ct) => ListAsync(db.InternshipPositions.AsNoTracking().Select(x => new AdminMonitoringRow
    { Id = x.Id, Title = x.Title, Company = x.Company.Name, Period = x.InternshipPeriod.Name, Status = (int)x.Status }), model, "Positions", ct);
    public Task<IActionResult> Applications(AdminMonitoringViewModel model, CancellationToken ct) => ListAsync(db.Applications.AsNoTracking().Select(x => new AdminMonitoringRow
    { Id = x.Id, Title = x.InternshipPosition.Title, Company = x.InternshipPosition.Company.Name, Period = x.InternshipPosition.InternshipPeriod.Name, Student = x.Student.User.FullName, StudentCode = x.Student.StudentCode, Status = (int)x.Status }), model, "Applications", ct);
    public Task<IActionResult> Internships(AdminMonitoringViewModel model, CancellationToken ct) => ListAsync(db.Internships.AsNoTracking().Select(x => new AdminMonitoringRow
    { Id = x.Id, Title = x.Application.InternshipPosition.Title, Company = x.Application.InternshipPosition.Company.Name, Period = x.InternshipPeriod.Name, Student = x.Student.User.FullName, StudentCode = x.Student.StudentCode, Status = (int)x.Status }), model, "Internships", ct);
    public async Task<IActionResult> Position(int id, CancellationToken ct)
    {
        var model = await db.InternshipPositions.AsNoTracking().Include(x => x.Company).Include(x => x.Major).Include(x => x.InternshipPeriod).SingleOrDefaultAsync(x => x.Id == id, ct);
        return model is null ? NotFound() : View(model);
    }
    public async Task<IActionResult> Application(int id, CancellationToken ct)
    {
        var model = await db.Applications.AsNoTracking().Include(x => x.Student).ThenInclude(x => x.User).Include(x => x.InternshipPosition).ThenInclude(x => x.Company).SingleOrDefaultAsync(x => x.Id == id, ct);
        return model is null ? NotFound() : View(model);
    }
    public async Task<IActionResult> Internship(int id, CancellationToken ct)
    {
        var model = await db.Internships.AsNoTracking().Include(x => x.Student).ThenInclude(x => x.User).Include(x => x.InternshipPeriod).Include(x => x.Application).ThenInclude(x => x.InternshipPosition).ThenInclude(x => x.Company).SingleOrDefaultAsync(x => x.Id == id, ct);
        return model is null ? NotFound() : View(model);
    }
}
