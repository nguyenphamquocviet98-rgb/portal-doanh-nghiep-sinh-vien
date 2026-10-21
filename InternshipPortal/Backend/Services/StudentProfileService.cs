using InternshipPortal.Data;
using InternshipPortal.ViewModels;
using InternshipPortal.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public class StudentProfileService(ApplicationDbContext db, PdfUploadService files)
{
    public async Task<StudentProfileViewModel?> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var student = await db.Students.AsNoTracking().Include(x => x.User).Include(x => x.Major).ThenInclude(x => x.Faculty)
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (student is null) return null;
        return new StudentProfileViewModel
        {
            FullName = student.User.FullName, Email = student.User.Email ?? "", PhoneNumber = student.User.PhoneNumber,
            StudentCode = student.StudentCode, ClassName = student.ClassName, FacultyName = student.Major.Faculty.Name,
            MajorName = student.Major.Name, Gpa = student.Gpa, Skills = student.Skills, Bio = student.Bio,
            HasCv = files.ResolvePrivatePath(student.CvPath) is not null
        };
    }

    public async Task<EditStudentProfileViewModel?> GetEditAsync(string userId, CancellationToken cancellationToken)
    {
        var student = await db.Students.AsNoTracking().Include(x => x.User).Include(x => x.Major)
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (student is null) return null;
        var model = new EditStudentProfileViewModel
        {
            FullName = student.User.FullName, StudentCode = student.StudentCode, ClassName = student.ClassName,
            PhoneNumber = student.User.PhoneNumber, FacultyId = student.Major.FacultyId, MajorId = student.MajorId,
            Gpa = student.Gpa, Skills = student.Skills, Bio = student.Bio, Email = student.User.Email ?? ""
        };
        await PopulateOptionsAsync(model, cancellationToken);
        return model;
    }

    public async Task PopulateOptionsAsync(EditStudentProfileViewModel model, CancellationToken cancellationToken)
    {
        model.Faculties = await db.Faculties.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToListAsync(cancellationToken);
        model.Majors = await db.Majors.AsNoTracking().Where(x => x.IsActive && x.Faculty.IsActive).OrderBy(x => x.Name)
            .Select(x => new MajorOptionViewModel { Id = x.Id, FacultyId = x.FacultyId, Name = x.Name }).ToListAsync(cancellationToken);
    }

    public async Task<IdentityResult> UpdateAsync(string userId, EditStudentProfileViewModel model, CancellationToken cancellationToken)
    {
        var student = await db.Students.Include(x => x.User).SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (student is null) return Failure("Không tìm thấy hồ sơ sinh viên của bạn.");
        var code = model.StudentCode.Trim().ToUpperInvariant();
        if (await db.Students.AnyAsync(x => x.StudentCode == code && x.Id != student.Id, cancellationToken))
            return Failure("Mã sinh viên này đã được sử dụng.");
        if (!await db.Majors.AnyAsync(x => x.Id == model.MajorId && x.FacultyId == model.FacultyId && x.IsActive && x.Faculty.IsActive, cancellationToken))
            return Failure("Ngành không thuộc khoa được chọn hoặc đã ngừng sử dụng.");
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            student.StudentCode = code;
            student.ClassName = model.ClassName.Trim();
            student.MajorId = model.MajorId;
            student.Gpa = model.Gpa;
            student.Skills = model.Skills?.Trim();
            student.Bio = model.Bio?.Trim();
            student.User.FullName = model.FullName.Trim();
            var phone = model.PhoneNumber?.Trim();
            if (student.User.PhoneNumber != phone) student.User.PhoneNumberConfirmed = false;
            student.User.PhoneNumber = phone;
            student.User.ConcurrencyStamp = Guid.NewGuid().ToString();
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return IdentityResult.Success;
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await transaction.RollbackAsync(cancellationToken);
            return Failure("Mã sinh viên vừa được sử dụng. Vui lòng kiểm tra lại.");
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Failure("Hồ sơ vừa thay đổi ở một phiên khác. Vui lòng tải lại và thử lại.");
        }
    }

    public async Task<string?> GetCvPathAsync(string userId, CancellationToken cancellationToken)
    {
        var path = await db.Students.AsNoTracking().Where(x => x.UserId == userId).Select(x => x.CvPath).SingleOrDefaultAsync(cancellationToken);
        return files.ResolvePrivatePath(path);
    }

    public async Task<bool> ReplaceCvAsync(string userId, IFormFile file, CancellationToken cancellationToken)
    {
        var student = await db.Students.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (student is null) return false;
        var path = await files.SaveCvAsync(file, cancellationToken);
        try
        {
            student.CvPath = path;
            await db.SaveChangesAsync(cancellationToken);
            // Giữ file cũ vì các đơn đã gửi có thể đang tham chiếu CV đó.
            return true;
        }
        catch
        {
            files.DeleteNewUpload(path);
            throw;
        }
    }

    private static IdentityResult Failure(string message) => IdentityResult.Failed(new IdentityError { Code = "Profile", Description = message });
}