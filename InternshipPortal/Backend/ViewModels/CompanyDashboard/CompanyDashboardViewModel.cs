using InternshipPortal.Models.Enums;

namespace InternshipPortal.ViewModels.CompanyDashboard;

public class CompanyDashboardViewModel : PortalViewModel
{
    // 1. Chỉ số Vị trí tuyển dụng
    public int OpenPositions { get; set; }
    public int TotalPositions { get; set; }

    // 2. Chỉ số Hồ sơ ứng tuyển
    public int PendingApplications { get; set; }
    public int TotalApplications { get; set; }

    // 3. Chỉ số Thực tập
    public int ActiveInternships { get; set; }
    public int TotalInternships { get; set; }

    // 4. Chỉ số Nhật ký & Đánh giá
    public int TotalDiaries { get; set; }
    public int PendingEvaluations { get; set; }

    // Phân quyền theo duyệt của trường
    public bool CanManage { get; set; }

    // Danh sách nhanh gần đây
    public IReadOnlyList<CompanyRecentApplicantViewModel> RecentApplicants { get; set; } = [];
    public IReadOnlyList<CompanyActiveInternViewModel> RecentInterns { get; set; } = [];
}

public class CompanyRecentApplicantViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string StudentCode { get; set; } = "";
    public string PositionTitle { get; set; } = "";
    public ApplicationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CompanyActiveInternViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string StudentCode { get; set; } = "";
    public string PositionTitle { get; set; } = "";
    public InternshipStatus Status { get; set; }
    public string? SupervisorName { get; set; }
    public int DiaryCount { get; set; }
    public bool HasEvaluation { get; set; }
}
