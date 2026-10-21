using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.Notifications;

public class NotificationItemViewModel
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public string? TargetUrl { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    public string TimeAgo
    {
        get
        {
            var diff = DateTime.UtcNow - CreatedAt;
            if (diff.TotalMinutes < 1) return "Vừa xong";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} phút trước";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} giờ trước";
            if (diff.TotalDays < 30) return $"{(int)diff.TotalDays} ngày trước";
            return CreatedAt.AddHours(7).ToString("dd/MM/yyyy");
        }
    }
}

public class NotificationListViewModel
{
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public bool OnlyUnread { get; set; }
    public IReadOnlyList<NotificationItemViewModel> Items { get; set; } = [];
}
