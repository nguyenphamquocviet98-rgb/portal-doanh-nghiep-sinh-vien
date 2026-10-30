using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.AdminDashboard;

public class AdminDashboardViewModel : PortalViewModel
{
    // Thống kê tổng quan
    public int TotalStudents { get; set; }
    public int TotalCompanies { get; set; }
    public int PendingCompanies { get; set; }
    public int TotalPositions { get; set; }
    public int OpenPositions { get; set; }
    public int TotalApplications { get; set; }
    public int TotalInternships { get; set; }
    public int ApplicationsInReview { get; set; }
    public int ActiveInternships { get; set; }
    public int InternshipsWithoutDiary { get; set; }
    public string? CurrentPeriodName { get; set; }
    public DateTime? CurrentPeriodStartDate { get; set; }
    public DateTime? CurrentPeriodEndDate { get; set; }
    public IReadOnlyList<AdminRecentApplicationViewModel> RecentApplications { get; set; } = [];

    // Biểu đồ 1: Tỷ lệ sinh viên có nơi thực tập
    public int PlacedStudentsCount { get; set; }
    public int UnplacedStudentsCount { get; set; }
    public double PlacementRate => TotalStudents > 0 
        ? Math.Round((double)PlacedStudentsCount / TotalStudents * 100, 1) 
        : 0;

    // Biểu đồ 2: Số lượng đơn ứng tuyển theo ngành
    public IReadOnlyList<string> MajorLabels { get; set; } = [];
    public IReadOnlyList<int> MajorApplicationCounts { get; set; } = [];

    // Biểu đồ 3: Phân bố trạng thái thực tập
    public int NotStartedCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int FailedCount { get; set; }
}

public class AdminRecentApplicationViewModel
{
    public int Id { get; set; }
    public string StudentName { get; set; } = "";
    public string StudentCode { get; set; } = "";
    public string PositionTitle { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public ApplicationStatus Status { get; set; }
}
