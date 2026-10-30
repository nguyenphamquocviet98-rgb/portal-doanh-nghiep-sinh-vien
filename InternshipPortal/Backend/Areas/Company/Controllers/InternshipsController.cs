using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.CompanyInternships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Company.Controllers;

[Area("Company"), Authorize(Roles = "Company"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class InternshipsController(CompanyInternshipService internships, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(CompanyInternshipListViewModel model, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        if (!ModelState.IsValid)
        {
            if (ModelState.TryGetValue(nameof(model.Status), out var statusState) && statusState.Errors.Count > 0)
            {
                statusState.Errors.Clear();
                ModelState.AddModelError(nameof(model.Status), "Trạng thái không hợp lệ.");
            }
            model.Keyword = null;
            model.Status = null;
        }
        await internships.ListAsync(userId, model, ct);
        return View(model);
    }

    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var model = await internships.GetAsync(userId, id, ct);
        return model is null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Diary(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var model = await internships.GetDiaryAsync(userId, id, ct);
        return model is null ? NotFound() : View("~/Frontend/Areas/Student/Views/Internships/Diary.cshtml", model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var model = await internships.EditFormAsync(userId, id, ct);
        return model is null ? NotFound() : View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Evaluation(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var model = await internships.EvaluationAsync(userId, id, ct);
        return model is null ? NotFound() : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Evaluation(int id, CompanyEvaluationViewModel model, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var current = await internships.EvaluationAsync(userId, id, ct);
        if (current is null) return NotFound();
        if (ModelState.IsValid)
        {
            var result = await internships.CreateEvaluationAsync(userId, id, model, ct);
            if (result.NotFound) return NotFound();
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Evaluation), new { id });
            }
            ModelState.AddModelError("", result.Message);
        }
        current = await internships.EvaluationAsync(userId, id, ct);
        if (current is null) return NotFound();
        model.InternshipId = id;
        model.FullName = current.FullName;
        model.Status = current.Status;
        model.Existing = current.Existing;
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditCompanyInternshipViewModel model, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var current = await internships.EditFormAsync(userId, id, ct);
        if (current is null) return NotFound();
        if (ModelState.IsValid)
        {
            var result = await internships.UpdateAsync(userId, id, model, ct);
            if (result.NotFound) return NotFound();
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Details), new { id });
            }
            ModelState.AddModelError("", result.Message);
        }
        model.Id = id;
        model.FullName = current.FullName;
        model.CurrentStatus = current.CurrentStatus;
        // Preserve the submitted version: a stale form must be explicitly reloaded.
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmStart(int id, string? supervisorName, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var current = await internships.EditFormAsync(userId, id, ct);
        if (current is null) return NotFound();
        if (current.CurrentStatus != InternshipStatus.NotStarted)
        {
            TempData["Error"] = "Chỉ có thể xác nhận bắt đầu thực tập cho sinh viên chưa bắt đầu.";
            return RedirectToAction(nameof(Details), new { id });
        }

        current.Status = InternshipStatus.InProgress;
        if (!string.IsNullOrWhiteSpace(supervisorName))
        {
            current.SupervisorName = supervisorName.Trim();
        }
        else if (string.IsNullOrWhiteSpace(current.SupervisorName))
        {
            current.SupervisorName = "Cán bộ hướng dẫn Doanh nghiệp";
        }

        var result = await internships.UpdateAsync(userId, id, current, ct);
        if (result.Succeeded)
        {
            TempData["Success"] = "Đã xác nhận sinh viên chính thức bắt đầu kỳ thực tập!";
        }
        else
        {
            TempData["Error"] = result.Message;
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmComplete(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var current = await internships.EditFormAsync(userId, id, ct);
        if (current is null) return NotFound();
        if (current.CurrentStatus != InternshipStatus.InProgress)
        {
            TempData["Error"] = "Chỉ có thể xác nhận hoàn thành khi sinh viên đang trong quá trình thực tập.";
            return RedirectToAction(nameof(Details), new { id });
        }

        current.Status = InternshipStatus.Completed;
        if (string.IsNullOrWhiteSpace(current.SupervisorName))
        {
            current.SupervisorName = "Cán bộ hướng dẫn Doanh nghiệp";
        }

        var result = await internships.UpdateAsync(userId, id, current, ct);
        if (result.Succeeded)
        {
            TempData["Success"] = "Đã xác nhận sinh viên hoàn thành kỳ thực tập!";
        }
        else
        {
            TempData["Error"] = result.Message;
        }
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RemindDiary(int id, CancellationToken ct)
    {
        var userId = users.GetUserId(User)!;
        if (!await internships.CanAccessAsync(userId, ct)) return Forbid();
        var result = await internships.SendDiaryReminderAsync(userId, id, ct);
        TempData[result.Succeeded ? "Success" : "Error"] = result.Message;
        return result.NotFound ? NotFound() : RedirectToAction(nameof(Details), new { id });
    }
}
