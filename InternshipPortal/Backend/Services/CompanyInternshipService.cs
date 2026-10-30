using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.CompanyInternships;
using InternshipPortal.ViewModels.Internships;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace InternshipPortal.Services;

public class CompanyInternshipService(ApplicationDbContext db)
{
    public async Task<CompanyEvaluationViewModel?> EvaluationAsync(string userId, int id, CancellationToken ct)
    {
        var internship = await Owned(userId).Include(x => x.Student).ThenInclude(x => x.User)
            .Include(x => x.Application).ThenInclude(x => x.InternshipPosition).ThenInclude(x => x.Company)
            .SingleOrDefaultAsync(x => x.Id == id, ct);
        if (internship is null) return null;
        var model = new CompanyEvaluationViewModel { InternshipId = id, FullName = internship.Student.User.FullName, Status = internship.Status };
        var evaluation = await db.Evaluations.AsNoTracking().SingleOrDefaultAsync(x => x.InternshipId == id, ct);
        if (evaluation is not null)
            model.Existing = new StudentEvaluationViewModel
            {
                InternshipId = id, PositionTitle = internship.Application.InternshipPosition.Title,
                CompanyName = internship.Application.InternshipPosition.Company.Name,
                EvaluatedAt = evaluation.CreatedAt, Comment = evaluation.Comment,
                Criteria =
                [
                    new() { Name = "Kiến thức chuyên môn", Score = evaluation.ProfessionalKnowledge },
                    new() { Name = "Kỹ năng làm việc", Score = evaluation.WorkSkills },
                    new() { Name = "Thái độ", Score = evaluation.Attitude },
                    new() { Name = "Tinh thần trách nhiệm", Score = evaluation.Responsibility },
                    new() { Name = "Khả năng giao tiếp", Score = evaluation.Communication },
                    new() { Name = "Mức độ hoàn thành công việc", Score = evaluation.TaskCompletion }
                ]
            };
        return model;
    }

    public async Task<CompanyInternshipUpdateResult> CreateEvaluationAsync(string userId, int id, CompanyEvaluationViewModel model, CancellationToken ct)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true))
            return new(false, string.Join(" ", errors.Select(x => x.ErrorMessage)));
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var internship = await Owned(userId).Include(x => x.Student).SingleOrDefaultAsync(x => x.Id == id, ct);
            if (internship is null) return new(false, "Không tìm thấy hồ sơ thuộc doanh nghiệp.", true);
            if (internship.Status is not (InternshipStatus.Completed or InternshipStatus.Failed))
                return new(false, "Chỉ được đánh giá khi hồ sơ Hoàn thành hoặc Không hoàn thành.");
            if (await db.Evaluations.AnyAsync(x => x.InternshipId == id, ct))
                return new(false, "Hồ sơ đã có đánh giá. Hãy tải lại để xem kết quả.");
            db.Evaluations.Add(new Evaluation
            {
                InternshipId = id, ProfessionalKnowledge = model.ProfessionalKnowledge!.Value,
                WorkSkills = model.WorkSkills!.Value, Attitude = model.Attitude!.Value,
                Responsibility = model.Responsibility!.Value, Communication = model.Communication!.Value,
                TaskCompletion = model.TaskCompletion!.Value, Comment = model.Comment.Trim()
            });
            db.Notifications.Add(new Notification
            {
                UserId = internship.Student.UserId, Type = NotificationType.NewEvaluation,
                Title = "Có đánh giá thực tập mới", Message = "Doanh nghiệp đã gửi kết quả đánh giá thực tập. Bạn có thể xem sáu tiêu chí và nhận xét trong hồ sơ thực tập.",
                TargetUrl = $"/Student/Internships/Evaluation/{id}", DeduplicationKey = $"evaluation:{id}"
            });
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new(true, "Đã gửi đánh giá và tạo thông báo cho sinh viên.");
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return new(false, "Dữ liệu đánh giá bị trùng. Hãy tải lại hồ sơ trước khi thử lại.");
        }
        catch (Exception exception) when (IsDeadlock(exception))
        {
            return new(false, "Có yêu cầu đang xử lý đồng thời. Hãy tải lại hồ sơ rồi thử lại.");
        }
    }

    public async Task<CompanyInternshipUpdateResult> SendDiaryReminderAsync(string userId, int id, CancellationToken ct)
    {
        var internship = await Owned(userId).Include(x => x.Student).ThenInclude(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == id, ct);
        if (internship is null) return new(false, "Không tìm thấy hồ sơ thực tập thuộc doanh nghiệp.", true);
        if (internship.Status != InternshipStatus.InProgress)
            return new(false, "Chỉ có thể nhắc nhật ký khi sinh viên đang thực tập.");
        if (await db.InternshipDiaries.AnyAsync(x => x.InternshipId == id, ct))
            return new(false, "Sinh viên đã nộp nhật ký, không cần gửi nhắc.");

        var key = $"diary-reminder:{id}:{DateTime.UtcNow:yyyyMMdd}";
        if (await db.Notifications.AnyAsync(x => x.DeduplicationKey == key, ct))
            return new(false, "Đã gửi nhắc nhật ký hôm nay.");

        db.Notifications.Add(new Notification
        {
            UserId = internship.Student.UserId,
            Type = NotificationType.Manual,
            Title = "Nhắc nộp nhật ký thực tập",
            Message = "Doanh nghiệp đề nghị bạn cập nhật và nộp nhật ký thực tập để theo dõi tiến độ.",
            TargetUrl = $"/Student/Internships/Diary/{id}",
            DeduplicationKey = key
        });
        await db.SaveChangesAsync(ct);
        return new(true, "Đã gửi nhắc nộp nhật ký cho sinh viên.");
    }

    private static string Version(Internship internship) =>
        (internship.UpdatedAt ?? internship.CreatedAt).Ticks.ToString(CultureInfo.InvariantCulture);

    public async Task<EditCompanyInternshipViewModel?> EditFormAsync(string userId, int id, CancellationToken ct)
    {
        var internship = await Owned(userId).Include(x => x.Student).ThenInclude(x => x.User).SingleOrDefaultAsync(x => x.Id == id, ct);
        return internship is null ? null : new EditCompanyInternshipViewModel
        {
            Id = id, FullName = internship.Student.User.FullName, Version = Version(internship),
            Status = internship.Status, CurrentStatus = internship.Status, SupervisorName = internship.SupervisorName ?? "",
            SupervisorEmail = internship.SupervisorEmail, SupervisorPhone = internship.SupervisorPhone
        };
    }

    public async Task<CompanyInternshipUpdateResult> UpdateAsync(string userId, int id, EditCompanyInternshipViewModel model, CancellationToken ct)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true))
            return new(false, string.Join(" ", errors.Select(x => x.ErrorMessage)));
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            var internship = await Owned(userId).AsTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
            if (internship is null) return new(false, "Không tìm thấy hồ sơ thuộc doanh nghiệp.", true);
            if (Version(internship) != model.Version)
                return new(false, "Hồ sơ vừa thay đổi. Hãy tải lại trang trước khi lưu.");
            var target = model.Status!.Value;
            var allowed = target == internship.Status
                || internship.Status == InternshipStatus.NotStarted && target == InternshipStatus.InProgress
                || internship.Status == InternshipStatus.InProgress && target is InternshipStatus.Completed or InternshipStatus.Failed;
            if (!allowed) return new(false, "Không được bỏ bước, chuyển ngược hoặc đổi kết quả thực tập đã kết thúc.");
            internship.SupervisorName = model.SupervisorName.Trim();
            internship.SupervisorEmail = string.IsNullOrWhiteSpace(model.SupervisorEmail) ? null : model.SupervisorEmail.Trim();
            internship.SupervisorPhone = string.IsNullOrWhiteSpace(model.SupervisorPhone) ? null : model.SupervisorPhone.Trim();
            internship.Status = target;
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new(true, "Đã lưu người hướng dẫn và trạng thái thực tập.");
        }
        catch (Exception exception) when (IsDeadlock(exception))
        {
            return new(false, "Có yêu cầu đang xử lý đồng thời. Hãy tải lại hồ sơ rồi thử lại.");
        }
    }

    private static bool IsDeadlock(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
            if (current is SqlException { Number: 1205 }) return true;
        return false;
    }

    public Task<bool> CanAccessAsync(string userId, CancellationToken ct) => db.Companies.AnyAsync(
        x => x.UserId == userId && x.IsActive && x.User.IsActive && x.ApprovalStatus == CompanyApprovalStatus.Approved, ct);

    private IQueryable<Internship> Owned(string userId) => db.Internships.AsNoTracking().Where(x =>
        x.Application.InternshipPosition.Company.UserId == userId && x.Application.InternshipPosition.Company.IsActive
        && x.Application.InternshipPosition.Company.User.IsActive
        && x.Application.InternshipPosition.Company.ApprovalStatus == CompanyApprovalStatus.Approved);

    private static IQueryable<CompanyInternshipViewModel> Project(IQueryable<Internship> query) => query.Select(x => new CompanyInternshipViewModel
    {
        Id = x.Id, ApplicationId = x.ApplicationId, FullName = x.Student.User.FullName, StudentCode = x.Student.StudentCode,
        PositionTitle = x.Application.InternshipPosition.Title, PeriodName = x.InternshipPeriod.Name,
        StartDate = x.StartDate, EndDate = x.EndDate, Status = x.Status,
        SupervisorName = x.SupervisorName, SupervisorEmail = x.SupervisorEmail, SupervisorPhone = x.SupervisorPhone
    });

    public async Task ListAsync(string userId, CompanyInternshipListViewModel model, CancellationToken ct)
    {
        var query = Owned(userId);
        if (model.Status.HasValue) query = query.Where(x => x.Status == model.Status);
        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            var keyword = model.Keyword.Trim();
            query = query.Where(x => x.Student.User.FullName.Contains(keyword) || x.Student.StudentCode.Contains(keyword));
        }
        model.TotalCount = await query.CountAsync(ct);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Internships = await Project(query.OrderByDescending(x => x.Id).Skip((model.Page - 1) * 10).Take(10)).ToListAsync(ct);
    }

    public async Task<CompanyInternshipViewModel?> GetAsync(string userId, int id, CancellationToken ct)
    {
        var model = await Project(Owned(userId).Where(x => x.Id == id)).SingleOrDefaultAsync(ct);
        if (model is null) return null;
        model.Diaries = await db.InternshipDiaries.AsNoTracking().Where(x => Owned(userId).Any(i => i.Id == x.InternshipId) && x.InternshipId == id)
            .OrderBy(x => x.WeekNumber).Select(x => new DiaryDetailsViewModel
            {
                Id = x.Id, InternshipId = x.InternshipId, WeekNumber = x.WeekNumber,
                StartDate = x.StartDate, EndDate = x.EndDate, CreatedAt = x.CreatedAt
            }).ToListAsync(ct);
        return model;
    }

    public Task<DiaryDetailsViewModel?> GetDiaryAsync(string userId, int id, CancellationToken ct) =>
        db.InternshipDiaries.AsNoTracking().Where(x => x.Id == id && Owned(userId).Any(i => i.Id == x.InternshipId))
            .Select(x => new DiaryDetailsViewModel
            {
                Id = x.Id, InternshipId = x.InternshipId, WeekNumber = x.WeekNumber,
                StartDate = x.StartDate, EndDate = x.EndDate, CreatedAt = x.CreatedAt, UpdatedAt = x.UpdatedAt,
                WorkContent = x.WorkContent, Results = x.Results, Difficulties = x.Difficulties, Note = x.Note
            }).SingleOrDefaultAsync(ct);
}
