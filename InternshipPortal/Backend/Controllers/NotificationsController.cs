using System.Security.Claims;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Controllers;

[Authorize]
public class NotificationsController(NotificationService notificationService) : Controller
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index(int page = 1, bool onlyUnread = false, CancellationToken ct = default)
    {
        var model = new NotificationListViewModel
        {
            Page = page,
            OnlyUnread = onlyUnread
        };

        await notificationService.ListAsync(UserId, model, ct);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct = default)
    {
        var targetUrl = await notificationService.MarkAsReadAsync(UserId, id, ct);
        if (!string.IsNullOrWhiteSpace(targetUrl) && Url.IsLocalUrl(targetUrl))
        {
            return Redirect(targetUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct = default)
    {
        await notificationService.MarkAllAsReadAsync(UserId, ct);
        TempData["Success"] = "Đã đánh dấu tất cả thông báo là đã đọc.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UnreadCount(CancellationToken ct = default)
    {
        var count = await notificationService.GetUnreadCountAsync(UserId, ct);
        return Json(new { unreadCount = count });
    }

    [HttpGet]
    public async Task<IActionResult> Recent(CancellationToken ct = default)
    {
        var items = await notificationService.GetRecentAsync(UserId, 5, ct);
        var unreadCount = await notificationService.GetUnreadCountAsync(UserId, ct);
        return Json(new { unreadCount, items });
    }
}
