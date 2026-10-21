using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.CompanyApplications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Company.Controllers;

[Area("Company"), Authorize(Roles = "Company"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class ApplicationsController(CompanyApplicationService applications, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(CompanyApplicationListViewModel model, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await applications.CanAccessAsync(userId, ct)) return Forbid();
        if (!ModelState.IsValid) { model.Keyword = null; model.Status = null; model.PositionId = null; }
        return await applications.ListAsync(userId, model, ct) ? View(model) : NotFound();
    }

    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await applications.CanAccessAsync(userId, ct)) return Forbid();
        var model = await applications.GetDetailsAsync(userId, id, ct);
        return model is null ? NotFound() : View(model);
    }

    public async Task<IActionResult> DownloadCv(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await applications.CanAccessAsync(userId, ct)) return Forbid();
        var path = await applications.GetCvPathAsync(userId, id, ct);
        if (path is null) return NotFound("Không tìm thấy CV của đơn ứng tuyển này.");
        Response.Headers.CacheControl = "private, no-store";
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return PhysicalFile(path, "application/pdf", $"CV-don-{id}.pdf");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, UpdateApplicationViewModel model, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await applications.CanAccessAsync(userId, ct)) return Forbid();
        var details = await applications.GetDetailsAsync(userId, id, ct);
        if (details is null) return NotFound();
        if (ModelState.IsValid)
        {
            var result = await applications.UpdateStatusAsync(userId, id, model, ct);
            if (result.NotFound) return NotFound();
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            ModelState.AddModelError("", result.Message);
        }
        // Reload the actual state after a stale form or competing request.
        details = await applications.GetDetailsAsync(userId, id, ct);
        if (details is null) return NotFound();
        details.Decision.Note = model.Note;
        return View("Details", details);
    }
}
