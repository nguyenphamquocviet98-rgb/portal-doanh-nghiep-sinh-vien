namespace InternshipPortal.ViewModels.Internships;

public class StudentEvaluationViewModel
{
    public int InternshipId { get; set; }
    public string PositionTitle { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public DateTime? EvaluatedAt { get; set; }
    public string? Comment { get; set; }
    public List<EvaluationCriterionViewModel> Criteria { get; set; } = [];
    public bool HasEvaluation => EvaluatedAt.HasValue;
    public decimal AverageScore => Criteria.Count == 0 ? 0 : Criteria.Average(x => x.Score);
}

public class EvaluationCriterionViewModel
{
    public string Name { get; set; } = "";
    public decimal Score { get; set; }
}