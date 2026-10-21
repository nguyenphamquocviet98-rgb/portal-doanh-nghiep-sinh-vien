using InternshipPortal.Models;
using InternshipPortal.ViewModels.Internships;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternshipPortal.ViewModels.Home;

public class HomeIndexViewModel
{
    public IReadOnlyList<PositionCardViewModel> LatestPositions { get; set; } = [];
    public IReadOnlyList<HomeCompanyItemViewModel> FeaturedCompanies { get; set; } = [];
    public InternshipPeriod? CurrentPeriod { get; set; }
    public List<SelectListItem> MajorOptions { get; set; } = [];
    public List<SelectListItem> LocationOptions { get; set; } = [];
    public int TotalOpenPositions { get; set; }
    public int TotalApprovedCompanies { get; set; }
    public int TotalPlacedStudents { get; set; }
}

public class HomeCompanyItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? BusinessField { get; set; }
    public int OpenPositionsCount { get; set; }
    public string LogoUrl => $"/images/companies/company-{((Math.Max(1, Id) - 1) % 6) + 1}.svg";
}
