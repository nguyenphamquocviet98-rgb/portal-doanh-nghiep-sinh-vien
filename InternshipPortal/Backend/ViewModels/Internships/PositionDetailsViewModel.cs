namespace InternshipPortal.ViewModels.Internships;

public class PositionDetailsViewModel : PositionCardViewModel
{
    public string Description { get; set; } = "";
    public string Requirements { get; set; } = "";
    public string PeriodName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string CompanyAddress { get; set; } = "";
}