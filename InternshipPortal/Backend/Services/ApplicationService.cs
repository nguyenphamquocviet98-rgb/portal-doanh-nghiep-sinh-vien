using System.Data;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Applications;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public record ApplicationActionResult(bool Succeeded, string Message, int? Id = null, bool NotFound = false);

public class ApplicationService(ApplicationDbContext db, PdfUploadService files, PositionCatalogService catalog)
{
    public async Task<ApplyViewModel?> GetFormAsync(string userId, int positionId, CancellationToken cancellationToken)
    {
        var student = await db.Students.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        var position = await catalog.GetDetailsAsync(positionId, cancellationToken);
        if (student is null || position is null) return null;
        var existing = await db.Applications.AsNoTracking().Where(x => x.StudentId == student.Id && x.InternshipPositionId == positionId)
            .Select(x => (int?)x.Id).SingleOrDefaultAsync(cancellationToken);
        var periodId = await db.InternshipPositions.Where(x => x.Id == positionId).Select(x => x.InternshipPeriodId).SingleAsync(cancellationToken);
        var hasCv = files.ResolvePrivatePath(student.CvPath) is not null;
        var accepted = await HasAcceptanceAsync(student.Id, periodId, cancellationToken);
        return new ApplyViewModel
        {
            PositionId = positionId, Title = position.Title, CompanyName = position.CompanyName, Location = position.Location,
            WorkMode = position.WorkMode, ApplicationDeadline = position.ApplicationDeadline, HasCv = hasCv, ExistingApplicationId = existing,
            BlockReason = existing.HasValue ? "Bạn đã ứng tuyển vị trí này. Mỗi sinh viên chỉ gửi một lần cho một vị trí, kể cả đơn đã rút."
                : accepted ? "Bạn đã được tiếp nhận thực tập trong đợt này, không thể gửi thêm đơn."
                : position.AvailablePlaces == 0 ? "Vị trí đã đủ chỉ tiêu tuyển."
                : !hasCv ? "Bạn cần tải CV lên hồ sơ trước khi gửi đơn." : null
        };
    }

    private async Task<bool> HasAcceptanceAsync(int studentId, int periodId, CancellationToken cancellationToken) =>
        await db.Internships.AnyAsync(x => x.StudentId == studentId && x.InternshipPeriodId == periodId, cancellationToken)
        || await db.Applications.AnyAsync(x => x.StudentId == studentId && x.Status == ApplicationStatus.Approved
            && x.InternshipPosition.InternshipPeriodId == periodId, cancellationToken);

    public async Task<ApplicationActionResult> SubmitAsync(string userId, ApplyViewModel model, CancellationToken cancellationToken)
    {
        // Kiểm tra lại trong transaction, không tin điều kiện đã hiển thị ở form GET.
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var student = await db.Students.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (student is null) return new(false, "Không tìm thấy hồ sơ sinh viên.", NotFound: true);
            var position = await db.InternshipPositions.Include(x => x.Company).ThenInclude(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == model.PositionId, cancellationToken);
            if (position is null || position.Status != InternshipPositionStatus.Open || position.ApplicationDeadline < DateTime.UtcNow
                || position.Company.ApprovalStatus != CompanyApprovalStatus.Approved || !position.Company.IsActive || !position.Company.User.IsActive)
                return new(false, "Vị trí đã đóng, hết hạn hoặc hiện không nhận hồ sơ.");
            if (await db.Applications.AnyAsync(x => x.StudentId == student.Id && x.InternshipPositionId == position.Id, cancellationToken))
                return new(false, "Bạn đã ứng tuyển vị trí này và không thể gửi lại.");
            if (await HasAcceptanceAsync(student.Id, position.InternshipPeriodId, cancellationToken))
                return new(false, "Bạn đã được tiếp nhận thực tập trong đợt này.");
            var acceptedCount = await db.Applications.CountAsync(x => x.InternshipPositionId == position.Id && x.Status == ApplicationStatus.Approved, cancellationToken);
            if (acceptedCount >= position.Quantity) return new(false, "Vị trí đã đủ chỉ tiêu tuyển.");
            if (files.ResolvePrivatePath(student.CvPath) is null) return new(false, "Vui lòng tải CV hợp lệ lên hồ sơ trước khi gửi đơn.");
            var application = new Application
            {
                StudentId = student.Id, InternshipPositionId = position.Id, CvPath = student.CvPath!,
                CoverLetter = string.IsNullOrWhiteSpace(model.CoverLetter) ? null : model.CoverLetter.Trim(), Status = ApplicationStatus.Pending
            };
            db.Applications.Add(application);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new(true, "Đã gửi đơn ứng tuyển. Bạn có thể theo dõi trạng thái trong lịch sử.", application.Id);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await transaction.RollbackAsync(cancellationToken);
            return new(false, "Đơn ứng tuyển đã tồn tại. Vui lòng kiểm tra lịch sử.");
        }
        catch (Exception exception) when (IsDeadlock(exception))
        {
            // SQL Server đã rollback transaction bị deadlock; Dispose sẽ giải phóng tài nguyên.
            return new(false, "Có yêu cầu đang được xử lý đồng thời. Vui lòng kiểm tra lịch sử rồi thử lại.");
        }
    }

    private static bool IsDeadlock(Exception exception)
    {
        // EF có thể bọc SqlException trong nhiều lớp exception.
        for (Exception? current = exception; current is not null; current = current.InnerException)
            if (current is SqlException { Number: 1205 }) return true;
        return false;
    }

    public async Task ListAsync(string userId, ApplicationListViewModel model, CancellationToken cancellationToken)
    {
        var query = db.Applications.AsNoTracking().Where(x => x.Student.UserId == userId);
        if (model.Status.HasValue) query = query.Where(x => x.Status == model.Status.Value);
        model.TotalCount = await query.CountAsync(cancellationToken);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Applications = await query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .Skip((model.Page - 1) * 10).Take(10).Select(x => new ApplicationDetailsViewModel
            {
                Id = x.Id, PositionId = x.InternshipPositionId, PositionTitle = x.InternshipPosition.Title,
                CompanyName = x.InternshipPosition.Company.Name, Status = x.Status, CreatedAt = x.CreatedAt, UpdatedAt = x.UpdatedAt
            }).ToListAsync(cancellationToken);
    }

    public async Task<ApplicationDetailsViewModel?> GetDetailsAsync(string userId, int id, CancellationToken cancellationToken)
    {
        var application = await db.Applications.AsNoTracking().Include(x => x.InternshipPosition).ThenInclude(x => x.Company)
            .SingleOrDefaultAsync(x => x.Id == id && x.Student.UserId == userId, cancellationToken);
        if (application is null) return null;
        return new ApplicationDetailsViewModel
        {
            Id = application.Id, PositionId = application.InternshipPositionId, PositionTitle = application.InternshipPosition.Title,
            CompanyName = application.InternshipPosition.Company.Name, Status = application.Status, CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt, CoverLetter = application.CoverLetter, Note = application.Note,
            HasCv = files.ResolvePrivatePath(application.CvPath) is not null
        };
    }

    public async Task<ApplicationActionResult> WithdrawAsync(string userId, int id, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        var application = await db.Applications.AsNoTracking().Where(x => x.Id == id && x.Student.UserId == userId)
            .Select(x => new { x.Id, x.Student.UserId }).SingleOrDefaultAsync(cancellationToken);
        if (application is null) return new(false, "Không tìm thấy đơn ứng tuyển của bạn.", NotFound: true);
        var now = DateTime.UtcNow;
        var changed = await db.Applications.Where(x => x.Id == id && x.Student.UserId == userId && x.Status == ApplicationStatus.Pending)
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.Status, ApplicationStatus.Withdrawn).SetProperty(x => x.UpdatedAt, now), cancellationToken);
        if (changed == 0) return new(false, "Chỉ được rút đơn đang Chờ duyệt. Trạng thái có thể vừa thay đổi.");
        db.Notifications.Add(new Notification
        {
            UserId = userId, Type = NotificationType.ApplicationStatusChanged, Title = "Đã rút đơn ứng tuyển",
            Message = "Bạn đã rút đơn đang chờ duyệt. Đơn được giữ trong lịch sử và không thể gửi lại vào cùng vị trí.",
            DeduplicationKey = $"application:{id}:withdrawn", TargetUrl = $"/Student/Applications/Details/{id}"
        });
        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return new(true, "Đã rút đơn ứng tuyển. Lịch sử được giữ nguyên.", id);
    }

    public async Task<string?> GetCvPathAsync(string userId, int id, CancellationToken cancellationToken)
    {
        var path = await db.Applications.AsNoTracking().Where(x => x.Id == id && x.Student.UserId == userId)
            .Select(x => x.CvPath).SingleOrDefaultAsync(cancellationToken);
        return files.ResolvePrivatePath(path);
    }
}
