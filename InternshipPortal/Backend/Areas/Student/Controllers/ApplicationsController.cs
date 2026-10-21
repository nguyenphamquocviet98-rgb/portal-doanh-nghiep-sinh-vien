using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Student.Controllers;

[Area("Student"), Authorize(Roles = "Student")]
public class ApplicationsController(ApplicationService applications, UserManager<ApplicationUser> users) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(ApplicationListViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid) await applications.ListAsync(users.GetUserId(User)!, model, cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int positionId, CancellationToken cancellationToken)
    {
        var model = await applications.GetFormAsync(users.GetUserId(User)!, positionId, cancellationToken);
        return model is null ? NotFound("Vị trí không tồn tại hoặc hiện không nhận hồ sơ.") : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ApplyViewModel model, CancellationToken cancellationToken)
    {
        if (model.PositionId < 1) return BadRequest("Vị trí thực tập chưa hợp lệ.");
        if (ModelState.IsValid)
        {
            var result = await applications.SubmitAsync(users.GetUserId(User)!, model, cancellationToken);
            if (result.NotFound) return NotFound(result.Message);
            if (result.Succeeded)
            {
                TempData["Success"] = result.Message;
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", result.Message);
        }
        var current = await applications.GetFormAsync(users.GetUserId(User)!, model.PositionId, cancellationToken);
        if (current is null)
        {
            TempData["ApplicationError"] = "Vị trí hiện không nhận hồ sơ. Đơn chưa được gửi.";
            return RedirectToAction(nameof(Index));
        }
        current.CoverLetter = model.CoverLetter;
        return View(current);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var model = await applications.GetDetailsAsync(users.GetUserId(User)!, id, cancellationToken);
        return model is null ? NotFound("Không tìm thấy đơn ứng tuyển của bạn.") : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Withdraw(int id, CancellationToken cancellationToken)
    {
        var result = await applications.WithdrawAsync(users.GetUserId(User)!, id, cancellationToken);
        if (result.NotFound) return NotFound(result.Message);
        TempData[result.Succeeded ? "Success" : "ApplicationError"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> DownloadCv(int id, CancellationToken cancellationToken)
    {
        var path = await applications.GetCvPathAsync(users.GetUserId(User)!, id, cancellationToken);
        if (path is null) return NotFound("Không tìm thấy CV trong đơn ứng tuyển của bạn.");
        Response.Headers.CacheControl = "private, no-store";
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return PhysicalFile(path, "application/pdf", $"CV-don-{id}.pdf");
    }
}