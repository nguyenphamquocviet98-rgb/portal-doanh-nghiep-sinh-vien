using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Internships;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public class PositionCatalogService(ApplicationDbContext db)
{
    public const int PageSize = 9;

    // Dùng cùng điều kiện cho danh sách, chi tiết và vị trí trên trang doanh nghiệp.
    private IQueryable<InternshipPosition> PublicPositions(DateTime now) => db.InternshipPositions.AsNoTracking()
        .Where(x => x.Status == InternshipPositionStatus.Open && x.ApplicationDeadline >= now
            && x.Company.ApprovalStatus == CompanyApprovalStatus.Approved && x.Company.IsActive && x.Company.User.IsActive);

    public async Task PopulateFiltersAsync(InternshipSearchViewModel model, CancellationToken cancellationToken)
    {
        var positions = PublicPositions(DateTime.UtcNow);
        model.Majors = await positions.Select(x => new { x.MajorId, x.Major.Name }).Distinct().OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.MajorId.ToString())).ToListAsync(cancellationToken);
        model.Companies = await positions.Select(x => new { x.CompanyId, x.Company.Name }).Distinct().OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.CompanyId.ToString())).ToListAsync(cancellationToken);
        model.Locations = await positions.Select(x => x.Location).Distinct().OrderBy(x => x)
            .Select(x => new SelectListItem(x, x)).ToListAsync(cancellationToken);
    }

    public async Task SearchAsync(InternshipSearchViewModel model, CancellationToken cancellationToken)
    {
        var query = PublicPositions(DateTime.UtcNow);
        var keyword = model.Keyword?.Trim();
        if (!string.IsNullOrWhiteSpace(keyword)) query = query.Where(x => x.Title.Contains(keyword));
        if (model.MajorId.HasValue) query = query.Where(x => x.MajorId == model.MajorId.Value);
        if (model.CompanyId.HasValue) query = query.Where(x => x.CompanyId == model.CompanyId.Value);
        if (!string.IsNullOrWhiteSpace(model.Location)) query = query.Where(x => x.Location == model.Location.Trim());
        if (model.WorkMode.HasValue) query = query.Where(x => x.WorkMode == model.WorkMode.Value);
        model.TotalCount = await query.CountAsync(cancellationToken);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / (double)PageSize));
        model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Positions = await Cards(query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
            .Skip((model.Page - 1) * PageSize).Take(PageSize)).ToListAsync(cancellationToken);
    }

    public Task<PositionDetailsViewModel?> GetDetailsAsync(int id, CancellationToken cancellationToken) =>
        PublicPositions(DateTime.UtcNow).Where(x => x.Id == id).Select(x => new PositionDetailsViewModel
        {
            Id = x.Id, Code = x.Code, Title = x.Title, CompanyId = x.CompanyId, CompanyName = x.Company.Name,
            MajorName = x.Major.Name, Location = x.Location, WorkMode = x.WorkMode, ApplicationDeadline = x.ApplicationDeadline,
            Quantity = x.Quantity, AcceptedCount = db.Applications.Count(a => a.InternshipPositionId == x.Id && a.Status == ApplicationStatus.Approved),
            Description = x.Description, Requirements = x.Requirements, PeriodName = x.InternshipPeriod.Name,
            StartDate = x.StartDate, EndDate = x.EndDate, CompanyAddress = x.Company.Address
        }).SingleOrDefaultAsync(cancellationToken);

    public async Task<CompanyDetailsViewModel?> GetCompanyAsync(int id, CancellationToken cancellationToken)
    {
        var company = await db.Companies.AsNoTracking()
            .Where(x => x.Id == id && x.ApprovalStatus == CompanyApprovalStatus.Approved && x.IsActive && x.User.IsActive)
            .Select(x => new { x.Id, x.Name, x.Address, x.BusinessField, x.Description, x.Website }).SingleOrDefaultAsync(cancellationToken);
        if (company is null) return null;
        var safeWebsite = Uri.TryCreate(company.Website, UriKind.Absolute, out var website) &&
            (website.Scheme == Uri.UriSchemeHttp || website.Scheme == Uri.UriSchemeHttps) ? website.AbsoluteUri : null;
        return new CompanyDetailsViewModel
        {
            Id = company.Id, Name = company.Name, Address = company.Address, BusinessField = company.BusinessField,
            Description = company.Description, SafeWebsite = safeWebsite,
            Positions = await Cards(PublicPositions(DateTime.UtcNow).Where(x => x.CompanyId == id)
                .OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)).ToListAsync(cancellationToken)
        };
    }

    private IQueryable<PositionCardViewModel> Cards(IQueryable<InternshipPosition> query) => query.Select(x => new PositionCardViewModel
    {
        Id = x.Id, Code = x.Code, Title = x.Title, CompanyId = x.CompanyId, CompanyName = x.Company.Name,
        MajorName = x.Major.Name, Location = x.Location, WorkMode = x.WorkMode, ApplicationDeadline = x.ApplicationDeadline,
        Quantity = x.Quantity, AcceptedCount = db.Applications.Count(a => a.InternshipPositionId == x.Id && a.Status == ApplicationStatus.Approved)
    });
}