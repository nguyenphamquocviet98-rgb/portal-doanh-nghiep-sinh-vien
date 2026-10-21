using InternshipPortal.Data;
using System.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Internships;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public record DiaryActionResult(bool Succeeded, string Message, int? Id = null, bool NotFound = false);

public class StudentInternshipService(ApplicationDbContext db)
{
    public Task<List<StudentInternshipViewModel>> ListAsync(string userId, CancellationToken cancellationToken) =>
        db.Internships.AsNoTracking().Where(x => x.Student.UserId == userId).OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .Select(x => new StudentInternshipViewModel
            {
                Id = x.Id, PositionTitle = x.Application.InternshipPosition.Title,
                CompanyName = x.Application.InternshipPosition.Company.Name, PeriodName = x.InternshipPeriod.Name,
                StartDate = x.StartDate, EndDate = x.EndDate, Status = x.Status,
                DiaryCount = db.InternshipDiaries.Count(d => d.InternshipId == x.Id)
            }).ToListAsync(cancellationToken);

    public async Task<StudentInternshipViewModel?> GetAsync(string userId, int id, CancellationToken cancellationToken)
    {
        var model = await db.Internships.AsNoTracking().Where(x => x.Id == id && x.Student.UserId == userId)
            .Select(x => new StudentInternshipViewModel
            {
                Id = x.Id, PositionTitle = x.Application.InternshipPosition.Title,
                CompanyName = x.Application.InternshipPosition.Company.Name, PeriodName = x.InternshipPeriod.Name,
                StartDate = x.StartDate, EndDate = x.EndDate, Status = x.Status,
                SupervisorName = x.SupervisorName, SupervisorEmail = x.SupervisorEmail, SupervisorPhone = x.SupervisorPhone
            }).SingleOrDefaultAsync(cancellationToken);
        if (model is null) return null;
        model.TotalWeeks = TotalWeeks(model.StartDate, model.EndDate);
        model.Diaries = await db.InternshipDiaries.AsNoTracking().Where(x => x.InternshipId == id)
            .OrderBy(x => x.WeekNumber).Select(x => new DiaryDetailsViewModel
            {
                Id = x.Id, InternshipId = x.InternshipId, WeekNumber = x.WeekNumber,
                StartDate = x.StartDate, EndDate = x.EndDate, CreatedAt = x.CreatedAt, UpdatedAt = x.UpdatedAt
            }).ToListAsync(cancellationToken);
        model.DiaryCount = model.Diaries.Count;
        return model;
    }

    public Task<DiaryDetailsViewModel?> GetDiaryAsync(string userId, int id, CancellationToken cancellationToken) =>
        db.InternshipDiaries.AsNoTracking().Where(x => x.Id == id && x.Internship.Student.UserId == userId)
            .Select(x => new DiaryDetailsViewModel
            {
                Id = x.Id, InternshipId = x.InternshipId, WeekNumber = x.WeekNumber, StartDate = x.StartDate,
                EndDate = x.EndDate, WorkContent = x.WorkContent, Results = x.Results,
                Difficulties = x.Difficulties, Note = x.Note, CreatedAt = x.CreatedAt, UpdatedAt = x.UpdatedAt,
                CanEdit = x.Internship.Status == InternshipStatus.InProgress
            }).SingleOrDefaultAsync(cancellationToken);

    public async Task<DiaryFormViewModel?> CreateFormAsync(string userId, int internshipId, int? weekNumber, CancellationToken cancellationToken)
    {
        var internship = await OwnedInternship(userId, internshipId).SingleOrDefaultAsync(cancellationToken);
        if (internship is null) return null;
        var usedWeeks = await db.InternshipDiaries.Where(x => x.InternshipId == internshipId).Select(x => x.WeekNumber).ToListAsync(cancellationToken);
        var firstMissing = 1;
        while (usedWeeks.Contains(firstMissing)) firstMissing++;
        return Form(internship, Math.Clamp(weekNumber ?? firstMissing, 1, TotalWeeks(internship.StartDate, internship.EndDate)));
    }

    public async Task<DiaryFormViewModel?> EditFormAsync(string userId, int id, CancellationToken cancellationToken)
    {
        var diary = await db.InternshipDiaries.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.Internship.Student.UserId == userId, cancellationToken);
        if (diary is null) return null;
        var internship = await OwnedInternship(userId, diary.InternshipId).SingleAsync(cancellationToken);
        var form = Form(internship, diary.WeekNumber);
        form.DiaryId = diary.Id;
        form.WorkContent = diary.WorkContent;
        form.Results = diary.Results;
        form.Difficulties = diary.Difficulties;
        form.Note = diary.Note;
        return form;
    }

    private IQueryable<Internship> OwnedInternship(string userId, int id) => db.Internships.AsNoTracking()
        .Include(x => x.Application).ThenInclude(x => x.InternshipPosition).ThenInclude(x => x.Company)
        .Where(x => x.Id == id && x.Student.UserId == userId);

    private static int TotalWeeks(DateTime start, DateTime end) =>
        Math.Max(1, ((end.AddHours(7).Date - start.AddHours(7).Date).Days + 1 + 6) / 7);

    private static (DateTime Start, DateTime End) WeekRange(DateTime start, DateTime end, int week)
    {
        var localStart = start.AddHours(7).Date.AddDays((week - 1) * 7);
        var localEnd = end.AddHours(7).Date;
        var weekEnd = localStart.AddDays(6) < localEnd ? localStart.AddDays(6) : localEnd;
        // Nhật ký lưu ngày theo UTC tương ứng 00:00 giờ Việt Nam.
        return (DateTime.SpecifyKind(localStart.AddHours(-7), DateTimeKind.Utc), DateTime.SpecifyKind(weekEnd.AddHours(-7), DateTimeKind.Utc));
    }

    private static DiaryFormViewModel Form(Internship internship, int week)
    {
        var dates = WeekRange(internship.StartDate, internship.EndDate, week);
        return new DiaryFormViewModel
        {
            InternshipId = internship.Id, WeekNumber = week, PositionTitle = internship.Application.InternshipPosition.Title,
            CompanyName = internship.Application.InternshipPosition.Company.Name, TotalWeeks = TotalWeeks(internship.StartDate, internship.EndDate),
            StartDate = dates.Start, EndDate = dates.End, InternshipStartDate = internship.StartDate, InternshipEndDate = internship.EndDate,
            CanWrite = internship.Status == InternshipStatus.InProgress
        };
    }

    public async Task<DiaryActionResult> SaveAsync(string userId, DiaryFormViewModel model, int? diaryId, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            InternshipDiary? diary = null;
            var internshipId = model.InternshipId;
            if (diaryId.HasValue)
            {
                diary = await db.InternshipDiaries.SingleOrDefaultAsync(x => x.Id == diaryId.Value && x.Internship.Student.UserId == userId, cancellationToken);
                if (diary is null) return new(false, "Không tìm thấy nhật ký của bạn.", NotFound: true);
                internshipId = diary.InternshipId;
            }
            var internship = await OwnedInternship(userId, internshipId).SingleOrDefaultAsync(cancellationToken);
            if (internship is null) return new(false, "Không tìm thấy hồ sơ thực tập của bạn.", NotFound: true);
            if (internship.Status != InternshipStatus.InProgress)
                return new(false, "Chỉ được tạo hoặc sửa nhật ký khi hồ sơ Đang thực tập.");
            var total = TotalWeeks(internship.StartDate, internship.EndDate);
            if (model.WeekNumber < 1 || model.WeekNumber > total)
                return new(false, $"Tuần cần nằm trong khoảng 1 đến {total} của kỳ thực tập.");
            if (await db.InternshipDiaries.AnyAsync(x => x.InternshipId == internshipId && x.WeekNumber == model.WeekNumber
                && (!diaryId.HasValue || x.Id != diaryId.Value), cancellationToken))
                return new(false, "Tuần này đã có nhật ký. Hãy mở nhật ký hiện có để chỉnh sửa.");
            var dates = WeekRange(internship.StartDate, internship.EndDate, model.WeekNumber);
            if (diary is null)
            {
                diary = new InternshipDiary { InternshipId = internshipId };
                db.InternshipDiaries.Add(diary);
            }
            diary.WeekNumber = model.WeekNumber;
            diary.StartDate = dates.Start;
            diary.EndDate = dates.End;
            diary.WorkContent = model.WorkContent.Trim();
            diary.Results = model.Results.Trim();
            diary.Difficulties = model.Difficulties?.Trim();
            diary.Note = model.Note?.Trim();
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new(true, "Đã lưu nhật ký thực tập.", diary.Id);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await transaction.RollbackAsync(cancellationToken);
            return new(false, "Tuần này vừa được lưu ở một yêu cầu khác. Vui lòng kiểm tra danh sách nhật ký.");
        }
        catch (Exception exception) when (IsDeadlock(exception))
        {
            return new(false, "Có yêu cầu đang xử lý đồng thời. Vui lòng kiểm tra nhật ký rồi thử lại.");
        }
    }

    private static bool IsDeadlock(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
            if (current is SqlException { Number: 1205 }) return true;
        return false;
    }

    public async Task<StudentEvaluationViewModel?> EvaluationAsync(string userId, int internshipId, CancellationToken cancellationToken)
    {
        var internship = await OwnedInternship(userId, internshipId).SingleOrDefaultAsync(cancellationToken);
        if (internship is null) return null;
        var model = new StudentEvaluationViewModel
        {
            InternshipId = internshipId, PositionTitle = internship.Application.InternshipPosition.Title,
            CompanyName = internship.Application.InternshipPosition.Company.Name
        };
        if (internship.Status is not (InternshipStatus.Completed or InternshipStatus.Failed)) return model;
        var evaluation = await db.Evaluations.AsNoTracking().SingleOrDefaultAsync(x => x.InternshipId == internshipId, cancellationToken);
        if (evaluation is null) return model;
        model.EvaluatedAt = evaluation.CreatedAt;
        model.Comment = evaluation.Comment;
        model.Criteria =
        [
            new() { Name = "Kiến thức chuyên môn", Score = evaluation.ProfessionalKnowledge },
            new() { Name = "Kỹ năng làm việc", Score = evaluation.WorkSkills },
            new() { Name = "Thái độ", Score = evaluation.Attitude },
            new() { Name = "Tinh thần trách nhiệm", Score = evaluation.Responsibility },
            new() { Name = "Khả năng giao tiếp", Score = evaluation.Communication },
            new() { Name = "Mức độ hoàn thành công việc", Score = evaluation.TaskCompletion }
        ];
        return model;
    }
}
