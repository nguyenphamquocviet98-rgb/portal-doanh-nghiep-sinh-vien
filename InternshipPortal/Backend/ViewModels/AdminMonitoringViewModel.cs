using System.ComponentModel.DataAnnotations;
namespace InternshipPortal.ViewModels;
public class AdminMonitoringViewModel
{
    [Display(Name = "Tên vị trí, doanh nghiệp hoặc sinh viên"), StringLength(200)] public string? Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public string Kind { get; set; } = "";
    public List<AdminMonitoringRow> Rows { get; set; } = [];
}
public class AdminMonitoringRow
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Company { get; set; } = "";
    public string? Student { get; set; }
    public string? StudentCode { get; set; }
    public string Period { get; set; } = "";
    public int Status { get; set; }
}
