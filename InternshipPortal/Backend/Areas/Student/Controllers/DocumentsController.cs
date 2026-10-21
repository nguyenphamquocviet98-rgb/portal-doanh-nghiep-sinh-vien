using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.Internships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Areas.Student.Controllers;

[Area("Student"), Authorize(Roles = "Student")]
public class DocumentsController(StudentDocumentService documents, UserManager<ApplicationUser> users, ILogger<DocumentsController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int internshipId, CancellationToken cancellationToken)
    {
        var model = await documents.GetAsync(users.GetUserId(User)!, internshipId, cancellationToken);
        return model is null ? NotFound("Không tìm thấy hồ sơ thực tập của bạn.") : View(model);
    }

    [HttpPost, ValidateAntiForgeryToken, RequestSizeLimit(12 * 1024 * 1024), RequestFormLimits(MultipartBodyLengthLimit = 12 * 1024 * 1024)]
    public async Task<IActionResult> Upload(UploadDocumentViewModel model, CancellationToken cancellationToken)
    {
        if (model.InternshipId < 1) return BadRequest("Hồ sơ thực tập chưa hợp lệ.");
        if (!ModelState.IsValid)
        {
            TempData["DocumentError"] = "Vui lòng chọn loại tài liệu hợp lệ và file PDF/DOCX tối đa 10 MB.";
            return RedirectToAction(nameof(Index), new { internshipId = model.InternshipId });
        }
        try
        {
            var result = await documents.UploadAsync(users.GetUserId(User)!, model, cancellationToken);
            if (result.NotFound) return NotFound(result.Message);
            TempData[result.Succeeded ? "Success" : "DocumentError"] = result.Message;
        }
        catch (InvalidOperationException exception)
        {
            TempData["DocumentError"] = exception.Message;
        }
        catch (IOException exception)
        {
            logger.LogError(exception, "Không lưu được tài liệu thực tập.");
            TempData["DocumentError"] = "Chưa lưu được tài liệu. Vui lòng thử lại sau.";
        }
        return RedirectToAction(nameof(Index), new { internshipId = model.InternshipId });
    }

    [HttpGet]
    public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
    {
        var file = await documents.GetDownloadAsync(users.GetUserId(User)!, id, cancellationToken);
        if (file is null) return NotFound("Không tìm thấy tài liệu của bạn.");
        Response.Headers.CacheControl = "private, no-store";
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        var type = Path.GetExtension(file.Value.Path).Equals(".docx", StringComparison.OrdinalIgnoreCase)
            ? "application/vnd.openxmlformats-officedocument.wordprocessingml.document" : "application/pdf";
        return PhysicalFile(file.Value.Path, type, file.Value.Name);
    }
}