using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.CompanyPositions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Company.Controllers;

[Area("Company"), Authorize(Roles = "Company")]
public class PositionsController(CompanyPositionService positions, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(CompanyPositionListViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) { model.Keyword = null; model.Status = null; }
        return await positions.ListAsync(users.GetUserId(User)!, model, ct) ? View(model) : Forbid();
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        if (!await positions.CanManageAsync(users.GetUserId(User)!, ct)) return Forbid();
        var today = DateTime.UtcNow.AddHours(7).Date;
        var model = new PositionFormViewModel { StartDate = today.AddDays(30), EndDate = today.AddDays(90), ApplicationDeadline = today.AddDays(14).AddHours(23).AddMinutes(59) };
        await positions.PopulateOptionsAsync(model, ct);
        return View("Form", model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        if (!await positions.CanManageAsync(users.GetUserId(User)!, ct)) return Forbid();
        var model = await positions.GetEditAsync(users.GetUserId(User)!, id, ct);
        if (model is null) return NotFound();
        ViewData["PositionId"] = id;
        return View("Form", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(PositionFormViewModel model, CancellationToken ct) => SaveAsync(null, model, ct);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, PositionFormViewModel model, CancellationToken ct) => SaveAsync(id, model, ct);

    private async Task<IActionResult> SaveAsync(int? id, PositionFormViewModel model, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await positions.CanManageAsync(userId, ct)) return Forbid();
        PositionFormViewModel? current = null;
        if (id.HasValue)
        {
            current = await positions.GetEditAsync(userId, id.Value, ct);
            if (current is null) return NotFound();
        }
        if (ModelState.IsValid)
        {
            var result = await positions.SaveAsync(userId, id, model, ct);
            if (result.Forbidden) return Forbid();
            if (result.NotFound) return NotFound();
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result.Message);
        }
        if (current is not null)
        {
            model.Code = current.Code;
            model.HasApplications = current.HasApplications;
            if (current.HasApplications)
            {
                // Re-display the actual locked values even when a forged form posts others.
                model.MajorId = current.MajorId;
                model.InternshipPeriodId = current.InternshipPeriodId;
                ModelState.Remove(nameof(model.MajorId));
                ModelState.Remove(nameof(model.InternshipPeriodId));
            }
        }
        await positions.PopulateOptionsAsync(model, ct);
        ViewData["PositionId"] = id;
        return View("Form", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int id, CancellationToken ct)
    {
        var result = await positions.CloseAsync(users.GetUserId(User)!, id, ct);
        if (result.Forbidden) return Forbid();
        if (result.NotFound) return NotFound();
        TempData[result.Succeeded ? "Success" : "PositionError"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
