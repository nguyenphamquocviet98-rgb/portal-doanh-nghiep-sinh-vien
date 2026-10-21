using System.ComponentModel.DataAnnotations;

namespace InternshipPortal.Models.Enums;

public enum CompanyApprovalStatus
{
    [Display(Name = "Chờ duyệt")] Pending = 0,
    [Display(Name = "Đã duyệt")] Approved = 1,
    [Display(Name = "Từ chối")] Rejected = 2
}
public enum InternshipPeriodStatus
{
    [Display(Name = "Chưa mở")] Planned = 0,
    [Display(Name = "Đang mở")] Open = 1,
    [Display(Name = "Đã đóng")] Closed = 2
}
public enum InternshipPositionStatus
{
    [Display(Name = "Đang tuyển")] Open = 0,
    [Display(Name = "Đã đóng")] Closed = 1
}
public enum WorkMode
{
    [Display(Name = "Tại doanh nghiệp")] OnSite = 0,
    [Display(Name = "Từ xa")] Remote = 1,
    [Display(Name = "Kết hợp")] Hybrid = 2
}
public enum ApplicationStatus
{
    [Display(Name = "Chờ duyệt")] Pending = 0,
    [Display(Name = "Đã xem")] Viewed = 1,
    [Display(Name = "Đã phỏng vấn")] Interviewed = 2,
    [Display(Name = "Đã duyệt")] Approved = 3,
    [Display(Name = "Từ chối")] Rejected = 4,
    [Display(Name = "Đã rút")] Withdrawn = 5,
    [Display(Name = "Đã hủy")] Cancelled = 6
}
public enum InternshipStatus
{
    [Display(Name = "Chưa bắt đầu")] NotStarted = 0,
    [Display(Name = "Đang thực tập")] InProgress = 1,
    [Display(Name = "Hoàn thành")] Completed = 2,
    [Display(Name = "Không hoàn thành")] Failed = 3
}
public enum DocumentType
{
    [Display(Name = "Báo cáo")] Report = 0,
    [Display(Name = "Tài liệu thực tập")] SupportingDocument = 1
}
public enum NotificationType
{
    [Display(Name = "Đơn được duyệt")] ApplicationApproved = 0,
    [Display(Name = "Đơn bị từ chối")] ApplicationRejected = 1,
    [Display(Name = "Trạng thái ứng tuyển")] ApplicationStatusChanged = 2,
    [Display(Name = "Vị trí mới")] NewPosition = 3,
    [Display(Name = "Doanh nghiệp được duyệt")] CompanyApproved = 4,
    [Display(Name = "Nhắc hạn ứng tuyển")] ApplicationDeadline = 5,
    [Display(Name = "Nhắc kết thúc thực tập")] InternshipEnding = 6,
    [Display(Name = "Đánh giá mới")] NewEvaluation = 7,
    [Display(Name = "Thông báo doanh nghiệp")] Manual = 8
}