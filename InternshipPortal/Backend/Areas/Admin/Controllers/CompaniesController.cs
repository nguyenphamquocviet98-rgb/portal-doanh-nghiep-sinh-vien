using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.AdminCompanies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace InternshipPortal.Areas.Admin.Controllers;
[Area("Admin"), Authorize(Roles = "Admin"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class CompaniesController(AdminCompanyService companies, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(AdminCompanyListViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) { model.Keyword = null; model.Status = null; }
        await companies.ListAsync(model, ct);
        return View(model);
    }
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var model = await companies.GetAsync(id, ct);
        return model is null ? NotFound() : View(model);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Decide(int id, [Bind(Prefix = "Decision")] AdminCompanyDecisionViewModel decision, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await companies.DecideAsync(users.GetUserId(User)!, id, decision, ct);
            if (result.NotFound) return NotFound();
            if (result.Success) { TempData["Success"] = result.Message; return RedirectToAction(nameof(Details), new { id }); }
            ModelState.AddModelError("", result.Message);
        }
        var model = await companies.GetAsync(id, ct);
        if (model is null) return NotFound();
        model.Decision = decision;
        return View("Details", model);
    }
}
