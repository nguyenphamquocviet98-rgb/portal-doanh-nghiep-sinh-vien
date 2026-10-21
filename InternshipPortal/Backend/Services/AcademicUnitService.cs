using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace InternshipPortal.Services;
public class AcademicUnitService(ApplicationDbContext db)
{
    private static string Version(BaseEntity x) => (x.UpdatedAt ?? x.CreatedAt).Ticks.ToString(CultureInfo.InvariantCulture);
    public Task<List<AcademicUnitViewModel>> ListAsync(bool major, CancellationToken ct) => major
        ? db.Majors.AsNoTracking().OrderBy(x => x.Code).Select(x => new AcademicUnitViewModel { Id = x.Id, Code = x.Code, Name = x.Name, IsActive = x.IsActive, FacultyName = x.Faculty.Name, IsMajor = true }).ToListAsync(ct)
        : db.Faculties.AsNoTracking().OrderBy(x => x.Code).Select(x => new AcademicUnitViewModel { Id = x.Id, Code = x.Code, Name = x.Name, IsActive = x.IsActive }).ToListAsync(ct);
    public async Task<AcademicUnitViewModel?> FormAsync(bool major, int? id, CancellationToken ct)
    {
        var model = new AcademicUnitViewModel { IsMajor = major };
        if (id.HasValue)
        {
            if (major)
            {
                var x = await db.Majors.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
                if (x is null) return null;
                model.Id = x.Id; model.Code = x.Code; model.Name = x.Name; model.IsActive = x.IsActive; model.FacultyId = x.FacultyId; model.Version = Version(x);
            }
            else
            {
                var x = await db.Faculties.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
                if (x is null) return null;
                model.Id = x.Id; model.Code = x.Code; model.Name = x.Name; model.IsActive = x.IsActive; model.Version = Version(x);
            }
        }
        await OptionsAsync(model, ct);
        return model;
    }
    public async Task OptionsAsync(AcademicUnitViewModel model, CancellationToken ct)
    {
        model.Faculties = await db.Faculties.AsNoTracking().OrderBy(x => x.Code)
            .Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(x.Name + (x.IsActive ? "" : " (ngừng hoạt động)"), x.Id.ToString())).ToListAsync(ct);
    }
    public async Task<(bool Success, bool NotFound, string Message)> SaveAsync(bool major, int? id, AcademicUnitViewModel model, CancellationToken ct)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true)) return (false, false, "Mã và tên không hợp lệ.");
        var code = model.Code.Trim().ToUpperInvariant();
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            if (major)
            {
                if (!model.FacultyId.HasValue || !await db.Faculties.AnyAsync(x => x.Id == model.FacultyId, ct)) return (false, false, "Chọn khoa tồn tại.");
                if (await db.Majors.AnyAsync(x => x.Code == code && (!id.HasValue || x.Id != id), ct)) return (false, false, "Mã ngành đã tồn tại.");
                var entity = id.HasValue ? await db.Majors.SingleOrDefaultAsync(x => x.Id == id, ct) : new Major();
                if (entity is null) return (false, true, "Không tìm thấy ngành.");
                if (id.HasValue && Version(entity) != model.Version) return (false, false, "Dữ liệu vừa thay đổi. Hãy tải lại trang.");
                entity.Code = code; entity.Name = model.Name.Trim(); entity.IsActive = model.IsActive; entity.FacultyId = model.FacultyId.Value;
                if (!id.HasValue) db.Majors.Add(entity);
            }
            else
            {
                if (await db.Faculties.AnyAsync(x => x.Code == code && (!id.HasValue || x.Id != id), ct)) return (false, false, "Mã khoa đã tồn tại.");
                var entity = id.HasValue ? await db.Faculties.SingleOrDefaultAsync(x => x.Id == id, ct) : new Faculty();
                if (entity is null) return (false, true, "Không tìm thấy khoa.");
                if (id.HasValue && Version(entity) != model.Version) return (false, false, "Dữ liệu vừa thay đổi. Hãy tải lại trang.");
                entity.Code = code; entity.Name = model.Name.Trim(); entity.IsActive = model.IsActive;
                if (!id.HasValue) db.Faculties.Add(entity);
            }
            await db.SaveChangesAsync(ct); await tx.CommitAsync(ct);
            return (true, false, "Đã lưu thông tin.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 }) { return (false, false, "Mã vừa được dùng. Hãy tải lại trang."); }
        catch (Exception ex) when (Deadlock(ex)) { return (false, false, "Có thao tác đồng thời. Hãy tải lại trang."); }
    }
    private static bool Deadlock(Exception ex)
    {
        for (Exception? e = ex; e is not null; e = e.InnerException) if (e is SqlException { Number: 1205 }) return true;
        return false;
    }
}
