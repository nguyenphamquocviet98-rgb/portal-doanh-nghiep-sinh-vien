using InternshipPortal.Services;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace InternshipPortal.Areas.Admin.Controllers;
[Area("Admin"), Authorize(Roles = "Admin"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AcademicUnitsController(AcademicUnitService units) : Controller
{
    public async Task<IActionResult> Index(bool major = false, CancellationToken ct = default)
    {
        ViewData["IsMajor"] = major;
        return View(await units.ListAsync(major, ct));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(bool major, int? id, CancellationToken ct)
    {
        var model = await units.FormAsync(major, id, ct);
        return model is null ? NotFound() : View(model);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(bool major, int? id, AcademicUnitViewModel model, CancellationToken ct)
    {
        if (id.HasValue && await units.FormAsync(major, id, ct) is null) return NotFound();
        if (ModelState.IsValid)
        {
            var result = await units.SaveAsync(major, id, model, ct);
            if (result.NotFound) return NotFound();
            if (result.Success) { TempData["Success"] = result.Message; return RedirectToAction(nameof(Index), new { major }); }
            ModelState.AddModelError("", result.Message);
        }
        model.IsMajor = major;
        await units.OptionsAsync(model, ct);
        return View(model);
    }
}
