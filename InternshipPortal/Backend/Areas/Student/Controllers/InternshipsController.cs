using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.Internships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Student.Controllers;

[Area("Student"), Authorize(Roles = "Student")]
public class InternshipsController(StudentInternshipService internships, UserManager<ApplicationUser> users) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await internships.ListAsync(users.GetUserId(User)!, cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var model = await internships.GetAsync(users.GetUserId(User)!, id, cancellationToken);
        return model is null ? NotFound("Không tìm thấy hồ sơ thực tập của bạn.") : View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Diary(int id, CancellationToken cancellationToken)
    {
        var model = await internships.GetDiaryAsync(users.GetUserId(User)!, id, cancellationToken);
        return model is null ? NotFound("Không tìm thấy nhật ký thực tập của bạn.") : View(model);
    }

    [HttpGet]
    public async Task<IActionResult> CreateDiary(int internshipId, int? weekNumber, CancellationToken cancellationToken)
    {
        var model = await internships.CreateFormAsync(users.GetUserId(User)!, internshipId, weekNumber, cancellationToken);
        return model is null ? NotFound("Không tìm thấy hồ sơ thực tập của bạn.") : View("DiaryForm", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDiary(DiaryFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var result = await internships.SaveAsync(users.GetUserId(User)!, model, null, cancellationToken);
            if (result.NotFound) return NotFound(result.Message);
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Diary), new { id = result.Id });
            }
            ModelState.AddModelError("", result.Message);
        }
        var current = await internships.CreateFormAsync(users.GetUserId(User)!, model.InternshipId, model.WeekNumber, cancellationToken);
        if (current is null) return NotFound("Không tìm thấy hồ sơ thực tập của bạn.");
        CopyInput(model, current);
        return View("DiaryForm", current);
    }

    [HttpGet]
    public async Task<IActionResult> EditDiary(int id, CancellationToken cancellationToken)
    {
        var model = await internships.EditFormAsync(users.GetUserId(User)!, id, cancellationToken);
        return model is null ? NotFound("Không tìm thấy nhật ký của bạn.") : View("DiaryForm", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDiary(int id, DiaryFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var result = await internships.SaveAsync(users.GetUserId(User)!, model, id, cancellationToken);
            if (result.NotFound) return NotFound(result.Message);
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Diary), new { id = result.Id });
            }
            ModelState.AddModelError("", result.Message);
        }
        var current = await internships.EditFormAsync(users.GetUserId(User)!, id, cancellationToken);
        if (current is null) return NotFound("Không tìm thấy nhật ký của bạn.");
        ModelState.Remove(nameof(DiaryFormViewModel.InternshipId));
        CopyInput(model, current);
        return View("DiaryForm", current);
    }

    [HttpGet]
    public async Task<IActionResult> Evaluation(int id, CancellationToken cancellationToken)
    {
        var model = await internships.EvaluationAsync(users.GetUserId(User)!, id, cancellationToken);
        return model is null ? NotFound("Không tìm thấy hồ sơ thực tập của bạn.") : View(model);
    }

    private static void CopyInput(DiaryFormViewModel source, DiaryFormViewModel target)
    {
        target.WeekNumber = source.WeekNumber;
        target.WorkContent = source.WorkContent;
        target.Results = source.Results;
        target.Difficulties = source.Difficulties;
        target.Note = source.Note;
    }
}
