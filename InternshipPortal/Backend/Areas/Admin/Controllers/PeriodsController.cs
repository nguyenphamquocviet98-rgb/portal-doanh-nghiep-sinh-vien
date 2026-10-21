using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace InternshipPortal.Areas.Admin.Controllers;
[Area("Admin"), Authorize(Roles = "Admin"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class PeriodsController(ApplicationDbContext db) : Controller
{
    private static string Version(InternshipPeriod x) => (x.UpdatedAt ?? x.CreatedAt).Ticks.ToString(CultureInfo.InvariantCulture);
    public async Task<IActionResult> Index(CancellationToken ct) => View(await db.InternshipPeriods.AsNoTracking().OrderByDescending(x => x.StartDate).ToListAsync(ct));
    [HttpGet]
    public async Task<IActionResult> Edit(int? id, CancellationToken ct)
    {
        if (!id.HasValue) return View(new PeriodFormViewModel { Status = InternshipPeriodStatus.Planned });
        var x = await db.InternshipPeriods.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        return x is null ? NotFound() : View(new PeriodFormViewModel
        { Id = x.Id, Code = x.Code, Name = x.Name, StartDate = x.StartDate.AddHours(7).Date, EndDate = x.EndDate.AddHours(7).Date, Status = x.Status, Version = Version(x) });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, PeriodFormViewModel model, CancellationToken ct)
    {
        model.Id = id ?? 0;
        if (!ModelState.IsValid) return View(model);
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var x = id.HasValue ? await db.InternshipPeriods.SingleOrDefaultAsync(x => x.Id == id, ct) : new InternshipPeriod();
            if (x is null) return NotFound();
            if (id.HasValue && Version(x) != model.Version) ModelState.AddModelError("", "Đợt vừa thay đổi. Hãy tải lại trang.");
            var code = model.Code.Trim().ToUpperInvariant();
            if (await db.InternshipPeriods.AnyAsync(x => x.Code == code && (!id.HasValue || x.Id != id), ct)) ModelState.AddModelError(nameof(model.Code), "Mã đợt đã tồn tại.");
            if (!ModelState.IsValid) return View(model);
            x.Code = code; x.Name = model.Name.Trim(); x.Status = model.Status!.Value;
            x.StartDate = DateTime.SpecifyKind(model.StartDate!.Value.Date.AddHours(-7), DateTimeKind.Utc);
            x.EndDate = DateTime.SpecifyKind(model.EndDate!.Value.Date.AddHours(-7), DateTimeKind.Utc);
            if (!id.HasValue) db.InternshipPeriods.Add(x);
            await db.SaveChangesAsync(ct); await tx.CommitAsync(ct);
            TempData["Success"] = "Đã lưu đợt thực tập.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 }) { ModelState.AddModelError("", "Mã đợt vừa được sử dụng. Hãy tải lại."); }
        catch (Exception ex) when (Deadlock(ex)) { ModelState.AddModelError("", "Có thao tác đồng thời. Hãy tải lại trang."); }
        return View(model);
    }
    private static bool Deadlock(Exception ex)
    {
        for (Exception? e = ex; e is not null; e = e.InnerException) if (e is SqlException { Number: 1205 }) return true;
        return false;
    }
}
