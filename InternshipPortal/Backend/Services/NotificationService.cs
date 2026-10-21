using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.ViewModels.Notifications;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public class NotificationService(ApplicationDbContext db)
{
    public Task<int> GetUnreadCountAsync(string userId, CancellationToken ct = default) =>
        db.Notifications.AsNoTracking().CountAsync(x => x.UserId == userId && !x.IsRead, ct);

    public async Task<IReadOnlyList<NotificationItemViewModel>> GetRecentAsync(string userId, int count = 5, CancellationToken ct = default)
    {
        return await db.Notifications.AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(count)
            .Select(x => new NotificationItemViewModel
            {
                Id = x.Id,
                Type = x.Type,
                Title = x.Title,
                Message = x.Message,
                TargetUrl = x.TargetUrl,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);
    }

    public async Task ListAsync(string userId, NotificationListViewModel model, CancellationToken ct = default)
    {
        var query = db.Notifications.AsNoTracking().Where(x => x.UserId == userId);
        model.UnreadCount = await query.CountAsync(x => !x.IsRead, ct);

        if (model.OnlyUnread)
        {
            query = query.Where(x => !x.IsRead);
        }

        model.TotalCount = await query.CountAsync(ct);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);

        model.Items = await query.OrderByDescending(x => x.CreatedAt)
            .Skip((model.Page - 1) * 10)
            .Take(10)
            .Select(x => new NotificationItemViewModel
            {
                Id = x.Id,
                Type = x.Type,
                Title = x.Title,
                Message = x.Message,
                TargetUrl = x.TargetUrl,
                IsRead = x.IsRead,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);
    }

    public async Task<string?> MarkAsReadAsync(string userId, int id, CancellationToken ct = default)
    {
        var notification = await db.Notifications.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, ct);
        if (notification is null) return null;

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
        }

        return notification.TargetUrl;
    }

    public async Task<int> MarkAllAsReadAsync(string userId, CancellationToken ct = default)
    {
        var unread = await db.Notifications.Where(x => x.UserId == userId && !x.IsRead).ToListAsync(ct);
        if (unread.Count == 0) return 0;

        var now = DateTime.UtcNow;
        foreach (var item in unread)
        {
            item.IsRead = true;
            item.ReadAt = now;
        }

        await db.SaveChangesAsync(ct);
        return unread.Count;
    }
}
