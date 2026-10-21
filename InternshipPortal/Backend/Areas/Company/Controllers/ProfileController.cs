using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Company.Controllers;

[Area("Company"), Authorize(Roles = "Company")]
public class ProfileController(CompanyProfileService profiles, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await profiles.GetAsync(users.GetUserId(User)!, cancellationToken);
        return model is null ? Forbid() : View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken)
    {
        var model = await profiles.GetAsync(users.GetUserId(User)!, cancellationToken);
        return model is null ? Forbid() : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CompanyProfileViewModel model, CancellationToken cancellationToken)
    {
        var userId = users.GetUserId(User)!;
        var current = await profiles.GetAsync(userId, cancellationToken);
        if (current is null) return Forbid();
        if (ModelState.IsValid)
        {
            var result = await profiles.UpdateAsync(userId, model, cancellationToken);
            if (result.Succeeded)
            {
                TempData["Success"] = "Đã cập nhật hồ sơ doanh nghiệp.";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }
        model.Email = current.Email;
        model.ApprovalStatusName = current.ApprovalStatusName;
        model.ApprovalNote = current.ApprovalNote;
        return View(model);
    }
}
