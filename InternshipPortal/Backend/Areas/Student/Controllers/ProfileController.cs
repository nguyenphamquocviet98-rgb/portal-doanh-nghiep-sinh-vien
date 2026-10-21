using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Student.Controllers;

[Area("Student"), Authorize(Roles = "Student")]
public class ProfileController(StudentProfileService profiles, UserManager<ApplicationUser> users, ILogger<ProfileController> logger) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = users.GetUserId(User)!;
        var profile = await profiles.GetAsync(userId, cancellationToken);
        return profile is null ? Forbid() : View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> Cv(CancellationToken cancellationToken)
    {
        var profile = await profiles.GetAsync(users.GetUserId(User)!, cancellationToken);
        return profile is null ? Forbid() : View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(CancellationToken cancellationToken)
    {
        var model = await profiles.GetEditAsync(users.GetUserId(User)!, cancellationToken);
        return model is null ? Forbid() : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditStudentProfileViewModel model, CancellationToken cancellationToken)
    {
        var userId = users.GetUserId(User)!;
        var current = await profiles.GetAsync(userId, cancellationToken);
        if (current is null) return Forbid();
        if (ModelState.IsValid)
        {
            var result = await profiles.UpdateAsync(userId, model, cancellationToken);
            if (result.Succeeded)
            {
                TempData["Success"] = "Đã cập nhật hồ sơ sinh viên.";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }
        model.Email = current.Email;
        await profiles.PopulateOptionsAsync(model, cancellationToken);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken, RequestSizeLimit(12 * 1024 * 1024), RequestFormLimits(MultipartBodyLengthLimit = 12 * 1024 * 1024)]
    public async Task<IActionResult> UploadCv(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || !ModelState.IsValid)
        {
            TempData["CvError"] = "Vui lòng chọn CV định dạng PDF, tối đa 10 MB.";
            return RedirectToAction(nameof(Index));
        }
        try
        {
            var saved = await profiles.ReplaceCvAsync(users.GetUserId(User)!, file, cancellationToken);
            if (!saved) return Forbid();
            TempData["Success"] = "Đã lưu CV mới. CV trong các đơn đã gửi được giữ nguyên.";
        }
        catch (InvalidOperationException exception)
        {
            TempData["CvError"] = exception.Message;
        }
        catch (IOException exception)
        {
            logger.LogError(exception, "Không lưu được CV sinh viên.");
            TempData["CvError"] = "Chưa lưu được CV. Vui lòng thử lại sau.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DownloadCv(CancellationToken cancellationToken)
    {
        var path = await profiles.GetCvPathAsync(users.GetUserId(User)!, cancellationToken);
        if (path is null) return NotFound("Không tìm thấy CV của bạn.");
        Response.Headers.CacheControl = "private, no-store";
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return PhysicalFile(path, "application/pdf", "CV-cua-toi.pdf");
    }
}
