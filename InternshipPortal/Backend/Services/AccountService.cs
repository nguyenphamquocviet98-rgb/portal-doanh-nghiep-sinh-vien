using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public class AccountService(ApplicationDbContext db, UserManager<ApplicationUser> users, RoleManager<IdentityRole> roles)
{
    public async Task<IdentityResult> RegisterStudentAsync(RegisterStudentViewModel model, CancellationToken cancellationToken)
    {
        if (!await db.Majors.AnyAsync(x => x.Id == model.MajorId && x.IsActive && x.FacultyId == model.FacultyId && x.Faculty.IsActive, cancellationToken))
            return Failure("Ngành được chọn không thuộc khoa hoặc đã ngừng sử dụng.");
        var code = model.StudentCode.Trim().ToUpperInvariant();
        if (await db.Students.AnyAsync(x => x.StudentCode == code, cancellationToken))
            return Failure("Mã sinh viên này đã được sử dụng.");
        return await RegisterAsync(model, "Student", user =>
        {
            db.Students.Add(new Student
            {
                UserId = user.Id, StudentCode = code, MajorId = model.MajorId, ClassName = model.ClassName.Trim()
            });
        }, cancellationToken);
    }

    public Task<IdentityResult> RegisterCompanyAsync(RegisterCompanyViewModel model, CancellationToken cancellationToken) =>
        RegisterAsync(model, "Company", user =>
        {
            db.Companies.Add(new Company
            {
                UserId = user.Id, Name = model.CompanyName.Trim(), Address = model.Address.Trim(),
                TaxCode = model.TaxCode?.Trim(), Website = model.Website?.Trim(),
                ApprovalStatus = CompanyApprovalStatus.Pending
            });
        }, cancellationToken);

    private async Task<IdentityResult> RegisterAsync(RegisterViewModel model, string role, Action<ApplicationUser> createProfile, CancellationToken cancellationToken)
    {
        if (!await roles.RoleExistsAsync(role))
            return Failure("Hệ thống chưa khởi tạo vai trò. Vui lòng liên hệ nhà trường.");
        var email = model.Email.Trim();
        if (await users.FindByEmailAsync(email) is not null)
            return Failure("Email này đã được sử dụng.");
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = new ApplicationUser
            {
                UserName = email, Email = email, FullName = model.FullName.Trim(), PhoneNumber = model.PhoneNumber?.Trim()
            };
            var result = await users.CreateAsync(user, model.Password);
            if (!result.Succeeded) return result;
            result = await users.AddToRoleAsync(user, role);
            if (!result.Succeeded) return result;
            createProfile(user);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return IdentityResult.Success;
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await transaction.RollbackAsync(cancellationToken);
            return Failure("Email hoặc mã sinh viên vừa được sử dụng. Vui lòng kiểm tra lại.");
        }
    }

    private static IdentityResult Failure(string message) => IdentityResult.Failed(new IdentityError { Code = "Registration", Description = message });
}