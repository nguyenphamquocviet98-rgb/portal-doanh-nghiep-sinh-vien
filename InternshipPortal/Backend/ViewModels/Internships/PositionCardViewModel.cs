using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.Internships;

public class PositionCardViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = "";
    public string MajorName { get; set; } = "";
    public string Location { get; set; } = "";
    public WorkMode WorkMode { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int Quantity { get; set; }
    public int AcceptedCount { get; set; }
    public int AvailablePlaces => Math.Max(0, Quantity - AcceptedCount);
    public int DaysRemaining => Math.Max(0, (ApplicationDeadline.Date - DateTime.UtcNow.Date).Days);
    public string LogoUrl => $"/images/companies/company-{((Math.Max(1, CompanyId) - 1) % 6) + 1}.svg";
    public string AllowanceText => Id % 3 == 0 ? "Hỗ trợ 3 – 5 triệu" : (Id % 3 == 1 ? "Thỏa thuận / Có phụ cấp" : "Hỗ trợ 2 – 4 triệu");
}