using System.ComponentModel.DataAnnotations;
using System.Data;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.CompanyApplications;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace InternshipPortal.Services;

public class CompanyApplicationService(ApplicationDbContext db, PdfUploadService files)
{
    public Task<bool> CanAccessAsync(string userId, CancellationToken ct) => db.Companies.AnyAsync(
        x => x.UserId == userId && x.IsActive && x.User.IsActive && x.ApprovalStatus == CompanyApprovalStatus.Approved, ct);

    // Apply the same ownership and active approval checks to list, details and CV.
    private IQueryable<Application> OwnedApplications(string userId) => db.Applications.AsNoTracking().Where(
        x => x.InternshipPosition.Company.UserId == userId && x.InternshipPosition.Company.IsActive
            && x.InternshipPosition.Company.User.IsActive && x.InternshipPosition.Company.ApprovalStatus == CompanyApprovalStatus.Approved);

    public async Task<bool> ListAsync(string userId, CompanyApplicationListViewModel model, CancellationToken ct)
    {
        var ownedPositions = db.InternshipPositions.AsNoTracking().Where(x => x.Company.UserId == userId
            && x.Company.IsActive && x.Company.User.IsActive && x.Company.ApprovalStatus == CompanyApprovalStatus.Approved);
        if (model.PositionId.HasValue && !await ownedPositions.AnyAsync(x => x.Id == model.PositionId, ct)) return false;
        model.Positions = await ownedPositions.OrderBy(x => x.Title)
            .Select(x => new SelectListItem(x.Title + " · " + x.Code, x.Id.ToString())).ToListAsync(ct);
        var query = OwnedApplications(userId);
        if (model.PositionId.HasValue) query = query.Where(x => x.InternshipPositionId == model.PositionId);
        if (model.Status.HasValue) query = query.Where(x => x.Status == model.Status);
        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            var keyword = model.Keyword.Trim();
            query = query.Where(x => x.Student.User.FullName.Contains(keyword) || x.Student.StudentCode.Contains(keyword));
        }
        model.TotalCount = await query.CountAsync(ct);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Applications = await query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .Skip((model.Page - 1) * 10).Take(10).Select(x => new CompanyApplicationDetailsViewModel
            {
                Id = x.Id, PositionId = x.InternshipPositionId, PositionTitle = x.InternshipPosition.Title,
                FullName = x.Student.User.FullName, StudentCode = x.Student.StudentCode,
                Status = x.Status, CreatedAt = x.CreatedAt
            }).ToListAsync(ct);
        return true;
    }

    public async Task<CompanyApplicationDetailsViewModel?> GetDetailsAsync(string userId, int id, CancellationToken ct)
    {
        var application = await OwnedApplications(userId).Include(x => x.InternshipPosition)
            .Include(x => x.Student).ThenInclude(x => x.User)
            .Include(x => x.Student).ThenInclude(x => x.Major).ThenInclude(x => x.Faculty)
            .SingleOrDefaultAsync(x => x.Id == id, ct);
        if (application is null) return null;
        var student = application.Student;
        return new CompanyApplicationDetailsViewModel
        {
            Id = application.Id, PositionId = application.InternshipPositionId, PositionTitle = application.InternshipPosition.Title,
            FullName = student.User.FullName, StudentCode = student.StudentCode, Email = student.User.Email ?? "",
            PhoneNumber = student.User.PhoneNumber, ClassName = student.ClassName,
            MajorName = student.Major.Name, FacultyName = student.Major.Faculty.Name, Gpa = student.Gpa,
            Skills = student.Skills, Bio = student.Bio,
            CoverLetter = application.CoverLetter, Note = application.Note, Status = application.Status,
            CreatedAt = application.CreatedAt, HasCv = files.ResolvePrivatePath(application.CvPath) is not null
        };
    }

    public async Task<string?> GetCvPathAsync(string userId, int id, CancellationToken ct)
    {
        var path = await OwnedApplications(userId).Where(x => x.Id == id).Select(x => x.CvPath).SingleOrDefaultAsync(ct);
        return files.ResolvePrivatePath(path);
    }

    public async Task<ApplicationActionResult> UpdateStatusAsync(string userId, int id, UpdateApplicationViewModel model, CancellationToken ct)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true))
            return new(false, string.Join(" ", errors.Select(x => x.ErrorMessage)));
        if (model.TargetStatus == ApplicationStatus.Rejected && string.IsNullOrWhiteSpace(model.Note))
            return new(false, "Vui lòng nhập lý do từ chối.");

        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var application = await db.Applications.Include(x => x.Student).ThenInclude(x => x.User)
                .Include(x => x.InternshipPosition).ThenInclude(x => x.Company).ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == id && x.InternshipPosition.Company.UserId == userId
                    && x.InternshipPosition.Company.IsActive && x.InternshipPosition.Company.User.IsActive
                    && x.InternshipPosition.Company.ApprovalStatus == CompanyApprovalStatus.Approved, ct);
            if (application is null) return new(false, "Không tìm thấy đơn thuộc doanh nghiệp đang được phép xử lý.", NotFound: true);
            if (application.Status != model.ExpectedStatus)
                return new(false, "Trạng thái đơn vừa thay đổi. Vui lòng tải lại hồ sơ trước khi xử lý.");
            var target = model.TargetStatus!.Value;
            var allowed = (application.Status == ApplicationStatus.Pending && target == ApplicationStatus.Viewed)
                || (application.Status == ApplicationStatus.Viewed && target == ApplicationStatus.Interviewed)
                || (application.Status == ApplicationStatus.Interviewed && target == ApplicationStatus.Approved)
                || (IsPending(application.Status) && target == ApplicationStatus.Approved)
                || (IsPending(application.Status) && target == ApplicationStatus.Rejected);
            if (!allowed) return new(false, "Không được chuyển ngược hoặc xử lý đơn đã kết thúc.");

            var position = application.InternshipPosition;
            if (target == ApplicationStatus.Approved)
            {
                if (!application.Student.User.IsActive) return new(false, "Tài khoản sinh viên đã ngừng hoạt động.");
                if (await db.Internships.AnyAsync(x => x.StudentId == application.StudentId && x.InternshipPeriodId == position.InternshipPeriodId, ct)
                    || await db.Applications.AnyAsync(x => x.StudentId == application.StudentId && x.Status == ApplicationStatus.Approved
                        && x.InternshipPosition.InternshipPeriodId == position.InternshipPeriodId, ct))
                    return new(false, "Sinh viên đã được tiếp nhận trong đợt này.");
                var accepted = await db.Applications.CountAsync(x => x.InternshipPositionId == position.Id && x.Status == ApplicationStatus.Approved, ct);
                if (accepted >= position.Quantity) return new(false, "Vị trí đã đủ chỉ tiêu tuyển.");
                db.Internships.Add(new Internship
                {
                    ApplicationId = application.Id, StudentId = application.StudentId,
                    InternshipPeriodId = position.InternshipPeriodId, StartDate = position.StartDate,
                    EndDate = position.EndDate, Status = InternshipStatus.NotStarted
                });
                var remaining = await db.Applications.Where(x => x.Id != id && x.StudentId == application.StudentId
                    && x.InternshipPosition.InternshipPeriodId == position.InternshipPeriodId
                    && (x.Status == ApplicationStatus.Pending || x.Status == ApplicationStatus.Viewed || x.Status == ApplicationStatus.Interviewed)).ToListAsync(ct);
                foreach (var other in remaining)
                {
                    other.Status = ApplicationStatus.Cancelled;
                    other.Note = "Tự hủy vì sinh viên đã được tiếp nhận ở vị trí " + position.Title + " trong cùng đợt thực tập.";
                    AddNotification(application.Student.UserId, other.Id, ApplicationStatus.Cancelled,
                        NotificationType.ApplicationStatusChanged, "Đơn ứng tuyển đã tự hủy", other.Note);
                }
            }
            application.Status = target;
            if (target == ApplicationStatus.Rejected) application.Note = model.Note!.Trim();
            var title = target switch
            {
                ApplicationStatus.Approved => "Đơn ứng tuyển đã được duyệt",
                ApplicationStatus.Rejected => "Đơn ứng tuyển bị từ chối",
                ApplicationStatus.Viewed => "Doanh nghiệp đã xem đơn",
                _ => "Đã cập nhật phỏng vấn"
            };
            var message = target == ApplicationStatus.Rejected ? application.Note!
                : $"Doanh nghiệp đã cập nhật đơn ứng tuyển vị trí {position.Title}. Bạn có thể xem chi tiết trong lịch sử ứng tuyển.";
            AddNotification(application.Student.UserId, application.Id, target,
                target == ApplicationStatus.Approved ? NotificationType.ApplicationApproved
                    : target == ApplicationStatus.Rejected ? NotificationType.ApplicationRejected : NotificationType.ApplicationStatusChanged,
                title, message);
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new(true, target == ApplicationStatus.Approved
                ? "Đã duyệt, tạo hồ sơ thực tập Chưa bắt đầu và hủy các đơn chưa kết thúc trong cùng đợt."
                : "Đã cập nhật trạng thái đơn và tạo thông báo cho sinh viên.", id);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return new(false, "Dữ liệu vừa được xử lý trong một yêu cầu khác. Vui lòng tải lại hồ sơ.");
        }
        catch (Exception ex) when (IsDeadlock(ex))
        {
            return new(false, "Có yêu cầu đang xử lý đồng thời. Vui lòng tải lại hồ sơ rồi thử lại.");
        }
    }

    private static bool IsPending(ApplicationStatus status) => status is ApplicationStatus.Pending or ApplicationStatus.Viewed or ApplicationStatus.Interviewed;

    private void AddNotification(string userId, int id, ApplicationStatus status, NotificationType type, string title, string message) =>
        db.Notifications.Add(new Notification
        {
            UserId = userId, Type = type, Title = title, Message = message,
            TargetUrl = $"/Student/Applications/Details/{id}", DeduplicationKey = $"application:{id}:{status.ToString().ToLowerInvariant()}"
        });

    private static bool IsDeadlock(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
            if (current is SqlException { Number: 1205 }) return true;
        return false;
    }
}
