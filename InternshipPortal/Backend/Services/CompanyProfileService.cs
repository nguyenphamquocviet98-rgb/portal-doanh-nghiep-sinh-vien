using System.ComponentModel.DataAnnotations;
using System.Reflection;
using InternshipPortal.Data;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public class CompanyProfileService(ApplicationDbContext db)
{
    public async Task<CompanyProfileViewModel?> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var company = await db.Companies.AsNoTracking().Include(x => x.User)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.IsActive && x.User.IsActive, cancellationToken);
        if (company is null) return null;
        return new CompanyProfileViewModel
        {
            Name = company.Name, TaxCode = company.TaxCode, Website = company.Website,
            Address = company.Address, BusinessField = company.BusinessField, Description = company.Description,
            Email = company.User.Email ?? "", ApprovalNote = company.ApprovalNote,
            ApprovalStatusName = company.ApprovalStatus.GetType().GetField(company.ApprovalStatus.ToString())?
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? company.ApprovalStatus.ToString()
        };
    }

    public async Task<IdentityResult> UpdateAsync(string userId, CompanyProfileViewModel model, CancellationToken cancellationToken)
    {
        // Validate again so callers outside MVC cannot save invalid data.
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(model, new ValidationContext(model), errors, true))
            return IdentityResult.Failed(errors.Select(x => new IdentityError { Description = x.ErrorMessage! }).ToArray());
        if (!string.IsNullOrWhiteSpace(model.Website) &&
            (!Uri.TryCreate(model.Website.Trim(), UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) || string.IsNullOrEmpty(uri.Host)))
            return Failure("Website phải là địa chỉ http:// hoặc https:// hợp lệ.");

        var company = await db.Companies.SingleOrDefaultAsync(
            x => x.UserId == userId && x.IsActive && x.User.IsActive, cancellationToken);
        if (company is null) return Failure("Không tìm thấy hồ sơ doanh nghiệp đang hoạt động của bạn.");
        // Only these fields are editable; approval and account ownership remain controlled by the school.
        company.Name = model.Name.Trim();
        company.Address = model.Address.Trim();
        company.TaxCode = Clean(model.TaxCode);
        company.Website = Clean(model.Website);
        company.BusinessField = Clean(model.BusinessField);
        company.Description = Clean(model.Description);
        await db.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    private static IdentityResult Failure(string message) => IdentityResult.Failed(new IdentityError { Description = message });
}
