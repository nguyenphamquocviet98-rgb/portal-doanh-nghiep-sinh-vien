namespace InternshipPortal.ViewModels.Internships;

public class CompanyDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public string? BusinessField { get; set; }
    public string? Description { get; set; }
    public string? SafeWebsite { get; set; }
    public List<PositionCardViewModel> Positions { get; set; } = [];
}