using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.AdminCompanies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace InternshipPortal.Services;
public class AdminCompanyService(ApplicationDbContext db)
{
    private static string Version(Company x) => (x.UpdatedAt ?? x.CreatedAt).Ticks.ToString(CultureInfo.InvariantCulture);
    public async Task ListAsync(AdminCompanyListViewModel model, CancellationToken ct)
    {
        var query = db.Companies.AsNoTracking();
        if (model.Status.HasValue) query = query.Where(x => x.ApprovalStatus == model.Status);
        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            var keyword = model.Keyword.Trim();
            query = query.Where(x => x.Name.Contains(keyword) || x.User.Email!.Contains(keyword));
        }
        model.TotalCount = await query.CountAsync(ct);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Companies = await query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .Skip((model.Page - 1) * 10).Take(10).Select(x => new AdminCompanyDetailsViewModel
            { Id = x.Id, Name = x.Name, Email = x.User.Email ?? "", Status = x.ApprovalStatus, IsActive = x.IsActive && x.User.IsActive }).ToListAsync(ct);
    }
    public async Task<AdminCompanyDetailsViewModel?> GetAsync(int id, CancellationToken ct)
    {
        var x = await db.Companies.AsNoTracking().Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id, ct);
        return x is null ? null : new AdminCompanyDetailsViewModel
        {
            Id = id, Name = x.Name, Email = x.User.Email ?? "", Address = x.Address, TaxCode = x.TaxCode,
            Website = x.Website, BusinessField = x.BusinessField, Description = x.Description,
            ApprovalNote = x.ApprovalNote, Status = x.ApprovalStatus, IsActive = x.IsActive && x.User.IsActive,
            Decision = new() { Version = Version(x) }
        };
    }
    public async Task<(bool Success, bool NotFound, string Message)> DecideAsync(string adminId, int id, AdminCompanyDecisionViewModel model, CancellationToken ct)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true)) return (false, false, "Dữ liệu quyết định không hợp lệ.");
        if (model.TargetStatus is not (CompanyApprovalStatus.Approved or CompanyApprovalStatus.Rejected)) return (false, false, "Chỉ được duyệt hoặc từ chối hồ sơ.");
        if (model.TargetStatus == CompanyApprovalStatus.Rejected && string.IsNullOrWhiteSpace(model.Note)) return (false, false, "Nhập lý do từ chối.");
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            if (!await db.Users.AnyAsync(x => x.Id == adminId && x.IsActive, ct)
                || !await (from ur in db.UserRoles join r in db.Roles on ur.RoleId equals r.Id where ur.UserId == adminId && r.Name == "Admin" select ur).AnyAsync(ct))
                return (false, false, "Tài khoản không còn quyền Admin đang hoạt động.");
            var company = await db.Companies.Include(x => x.User).SingleOrDefaultAsync(x => x.Id == id, ct);
            if (company is null) return (false, true, "Không tìm thấy doanh nghiệp.");
            if (!company.IsActive || !company.User.IsActive) return (false, false, "Doanh nghiệp hoặc tài khoản đã ngừng hoạt động.");
            if (company.ApprovalStatus != CompanyApprovalStatus.Pending || Version(company) != model.Version)
                return (false, false, "Hồ sơ vừa thay đổi hoặc đã được xử lý. Hãy tải lại trang.");
            company.ApprovalStatus = model.TargetStatus.Value;
            company.ApprovalNote = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note.Trim();
            var approved = company.ApprovalStatus == CompanyApprovalStatus.Approved;
            db.Notifications.Add(new Notification
            {
                UserId = company.UserId, Type = approved ? NotificationType.CompanyApproved : NotificationType.Manual,
                Title = approved ? "Doanh nghiệp đã được duyệt" : "Hồ sơ doanh nghiệp bị từ chối",
                Message = approved ? "Nhà trường đã duyệt hồ sơ. Bạn có thể sử dụng chức năng đăng vị trí thực tập." : company.ApprovalNote!,
                TargetUrl = "/Company/Profile", DeduplicationKey = $"company-review:{id}"
            });
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return (true, false, approved ? "Đã duyệt doanh nghiệp và tạo thông báo." : "Đã từ chối và gửi lý do trong thông báo.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
        { return (false, false, "Dữ liệu xử lý bị trùng. Hãy tải lại hồ sơ."); }
        catch (Exception ex) when (IsDeadlock(ex))
        { return (false, false, "Có yêu cầu xử lý đồng thời. Hãy tải lại hồ sơ rồi thử lại."); }
    }
    private static bool IsDeadlock(Exception ex)
    {
        for (Exception? current = ex; current is not null; current = current.InnerException)
            if (current is SqlException { Number: 1205 }) return true;
        return false;
    }
}
