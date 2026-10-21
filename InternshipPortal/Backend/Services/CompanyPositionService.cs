using System.ComponentModel.DataAnnotations;
using System.Data;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.CompanyPositions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public record PositionActionResult(bool Succeeded, string Message, int? Id = null, bool NotFound = false, bool Forbidden = false);

public class CompanyPositionService(ApplicationDbContext db)
{
    private IQueryable<Company> ActiveCompanies(string userId) => db.Companies
        .Where(x => x.UserId == userId && x.IsActive && x.User.IsActive);
    private IQueryable<InternshipPosition> OwnedPositions(string userId) => db.InternshipPositions
        .Where(x => x.Company.UserId == userId && x.Company.IsActive && x.Company.User.IsActive);

    public Task<bool> CanManageAsync(string userId, CancellationToken ct) =>
        ActiveCompanies(userId).AnyAsync(x => x.ApprovalStatus == CompanyApprovalStatus.Approved, ct);

    public async Task<bool> ListAsync(string userId, CompanyPositionListViewModel model, CancellationToken ct)
    {
        var company = await ActiveCompanies(userId).AsNoTracking().SingleOrDefaultAsync(ct);
        if (company is null) return false;
        model.CanManage = company.ApprovalStatus == CompanyApprovalStatus.Approved;
        var query = OwnedPositions(userId).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            var keyword = model.Keyword.Trim();
            query = query.Where(x => x.Title.Contains(keyword) || x.Code.Contains(keyword));
        }
        if (model.Status.HasValue) query = query.Where(x => x.Status == model.Status.Value);
        model.TotalCount = await query.CountAsync(ct);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Positions = await query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .Skip((model.Page - 1) * 10).Take(10).Select(x => new CompanyPositionRow
            {
                Id = x.Id, Code = x.Code, Title = x.Title, Status = x.Status,
                MajorName = x.Major.Name, PeriodName = x.InternshipPeriod.Name,
                Quantity = x.Quantity, ApplicationDeadline = x.ApplicationDeadline,
                ApplicationCount = db.Applications.Count(a => a.InternshipPositionId == x.Id),
                AcceptedCount = db.Applications.Count(a => a.InternshipPositionId == x.Id && a.Status == ApplicationStatus.Approved)
            }).ToListAsync(ct);
        return true;
    }

    public async Task<PositionFormViewModel?> GetEditAsync(string userId, int id, CancellationToken ct)
    {
        var position = await OwnedPositions(userId).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        if (position is null) return null;
        var model = new PositionFormViewModel
        {
            Code = position.Code, Title = position.Title, Description = position.Description,
            Requirements = position.Requirements, Location = position.Location, Quantity = position.Quantity,
            MajorId = position.MajorId, InternshipPeriodId = position.InternshipPeriodId, WorkMode = position.WorkMode,
            StartDate = position.StartDate.Date, EndDate = position.EndDate.Date,
            ApplicationDeadline = DateTime.SpecifyKind(position.ApplicationDeadline, DateTimeKind.Utc).AddHours(7),
            HasApplications = await db.Applications.AnyAsync(x => x.InternshipPositionId == id, ct)
        };
        await PopulateOptionsAsync(model, ct);
        return model;
    }

    public async Task PopulateOptionsAsync(PositionFormViewModel model, CancellationToken ct)
    {
        model.Majors = await db.Majors.AsNoTracking()
            .Where(x => (x.IsActive && x.Faculty.IsActive) || x.Id == model.MajorId).OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToListAsync(ct);
        model.Periods = await db.InternshipPeriods.AsNoTracking()
            .Where(x => x.Status != InternshipPeriodStatus.Closed || x.Id == model.InternshipPeriodId)
            .OrderByDescending(x => x.StartDate).Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToListAsync(ct);
    }

    public async Task<PositionActionResult> SaveAsync(string userId, int? id, PositionFormViewModel model, CancellationToken ct)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true))
            return new(false, string.Join(" ", errors.Select(x => x.ErrorMessage)));

        // Shared with application submission: serialize checks and writes to prevent capacity/ownership races.
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var company = await ActiveCompanies(userId).SingleOrDefaultAsync(ct);
            if (company is null || company.ApprovalStatus != CompanyApprovalStatus.Approved)
                return new(false, "Doanh nghiệp phải được duyệt và đang hoạt động để quản lý vị trí.", Forbidden: true);
            InternshipPosition? position = null;
            if (id.HasValue)
            {
                position = await db.InternshipPositions.SingleOrDefaultAsync(x => x.Id == id && x.CompanyId == company.Id, ct);
                if (position is null) return new(false, "Không tìm thấy vị trí của bạn.", NotFound: true);
                var hasApplications = await db.Applications.AnyAsync(x => x.InternshipPositionId == id, ct);
                if (hasApplications && (model.MajorId != position.MajorId || model.InternshipPeriodId != position.InternshipPeriodId))
                    return new(false, "Vị trí đã có đơn ứng tuyển, không được đổi ngành hoặc đợt thực tập.");
                var accepted = await db.Applications.CountAsync(x => x.InternshipPositionId == id && x.Status == ApplicationStatus.Approved, ct);
                if (model.Quantity < accepted) return new(false, $"Số lượng tuyển không được nhỏ hơn {accepted} sinh viên đã được duyệt.");
            }
            if ((position is null || position.MajorId != model.MajorId) &&
                !await db.Majors.AnyAsync(x => x.Id == model.MajorId && x.IsActive && x.Faculty.IsActive, ct))
                return new(false, "Vui lòng chọn ngành đang hoạt động.");
            if ((position is null || position.InternshipPeriodId != model.InternshipPeriodId) &&
                !await db.InternshipPeriods.AnyAsync(x => x.Id == model.InternshipPeriodId && x.Status != InternshipPeriodStatus.Closed, ct))
                return new(false, "Vui lòng chọn đợt thực tập chưa đóng.");
            var deadline = DateTime.SpecifyKind(model.ApplicationDeadline, DateTimeKind.Unspecified).AddHours(-7);
            // Keep historic deadlines editable without forcing a position back into recruitment.
            if ((position is null || deadline != position.ApplicationDeadline) && deadline <= DateTime.UtcNow)
                return new(false, "Hạn ứng tuyển mới phải ở tương lai (giờ Việt Nam).");
            if (position is null)
            {
                position = new InternshipPosition { CompanyId = company.Id, Code = "VT-" + Guid.NewGuid().ToString("N")[..26] };
                db.InternshipPositions.Add(position);
            }
            position.Title = model.Title.Trim();
            position.Description = model.Description.Trim();
            position.Requirements = model.Requirements.Trim();
            position.Location = model.Location.Trim();
            position.Quantity = model.Quantity;
            position.MajorId = model.MajorId;
            position.InternshipPeriodId = model.InternshipPeriodId;
            position.WorkMode = model.WorkMode;
            position.StartDate = model.StartDate.Date;
            position.EndDate = model.EndDate.Date;
            position.ApplicationDeadline = deadline;
            var isNew = !id.HasValue;
            await db.SaveChangesAsync(ct);

            if (isNew)
            {
                var students = await db.Students
                    .Where(s => s.MajorId == position.MajorId && s.User.IsActive)
                    .Select(s => s.UserId)
                    .Take(50)
                    .ToListAsync(ct);

                foreach (var studentUserId in students)
                {
                    db.Notifications.Add(new Notification
                    {
                        UserId = studentUserId,
                        Type = NotificationType.NewPosition,
                        Title = "Vị trí thực tập mới phù hợp chuyên ngành",
                        Message = $"Doanh nghiệp {company.Name} vừa mở tuyển vị trí {position.Title}.",
                        TargetUrl = $"/Internships/Details/{position.Id}",
                        DeduplicationKey = $"new-pos:{position.Id}:{studentUserId}"
                    });
                }
                await db.SaveChangesAsync(ct);
            }

            await transaction.CommitAsync(ct);
            return new(true, id.HasValue ? "Đã cập nhật vị trí thực tập." : "Đã đăng vị trí thực tập.", position.Id);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return new(false, "Mã vị trí bị trùng. Vui lòng thử lại.");
        }
        catch (Exception ex) when (IsDeadlock(ex))
        {
            return new(false, "Có thao tác đồng thời. Vui lòng tải lại danh sách trước khi thử lại.");
        }
    }

    public async Task<PositionActionResult> CloseAsync(string userId, int id, CancellationToken ct)
    {
        // Atomic update rechecks ownership, approval and current state at the moment of closing.
        var changed = await OwnedPositions(userId)
            .Where(x => x.Id == id && x.Company.ApprovalStatus == CompanyApprovalStatus.Approved && x.Status == InternshipPositionStatus.Open)
            .ExecuteUpdateAsync(update => update.SetProperty(x => x.Status, InternshipPositionStatus.Closed)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), ct);
        if (changed == 1) return new(true, "Đã đóng vị trí. Các đơn ứng tuyển đã gửi được giữ nguyên.", id);
        if (!await CanManageAsync(userId, ct)) return new(false, "Doanh nghiệp chưa được phép quản lý vị trí.", Forbidden: true);
        if (!await OwnedPositions(userId).AnyAsync(x => x.Id == id, ct)) return new(false, "Không tìm thấy vị trí của bạn.", NotFound: true);
        return new(true, "Vị trí đã đóng trước đó.", id);
    }

    private static bool IsDeadlock(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
            if (current is SqlException { Number: 1205 }) return true;
        return false;
    }
}
