using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternshipPortal.ViewModels.CompanyPositions;

public class PositionFormViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Vui lòng nhập tên vị trí."), StringLength(200, ErrorMessage = "Tên vị trí tối đa 200 ký tự."), Display(Name = "Tên vị trí")]
    public string Title { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập mô tả."), StringLength(2990, ErrorMessage = "Mô tả tối đa 2990 ký tự."), Display(Name = "Mô tả công việc")]
    public string Description { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập yêu cầu."), StringLength(1990, ErrorMessage = "Yêu cầu tối đa 1990 ký tự."), Display(Name = "Yêu cầu ứng viên")]
    public string Requirements { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập địa điểm."), StringLength(200, ErrorMessage = "Địa điểm tối đa 200 ký tự."), Display(Name = "Địa điểm")]
    public string Location { get; set; } = "";
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng tuyển phải lớn hơn 0."), Display(Name = "Số lượng tuyển")]
    public int Quantity { get; set; } = 1;
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ngành."), Display(Name = "Ngành")]
    public int MajorId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn đợt thực tập."), Display(Name = "Đợt thực tập")]
    public int InternshipPeriodId { get; set; }
    [EnumDataType(typeof(WorkMode), ErrorMessage = "Hình thức thực tập không hợp lệ."), Display(Name = "Hình thức")]
    public WorkMode WorkMode { get; set; }
    [DataType(DataType.Date), Display(Name = "Ngày bắt đầu")]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date), Display(Name = "Ngày kết thúc")]
    public DateTime EndDate { get; set; }
    [Display(Name = "Hạn ứng tuyển (giờ Việt Nam)")]
    public DateTime ApplicationDeadline { get; set; }
    [BindNever] public string Code { get; set; } = "";
    [BindNever] public bool HasApplications { get; set; }
    [BindNever] public List<SelectListItem> Majors { get; set; } = [];
    [BindNever] public List<SelectListItem> Periods { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate == default) yield return new("Vui lòng chọn ngày bắt đầu.", [nameof(StartDate)]);
        if (EndDate == default) yield return new("Vui lòng chọn ngày kết thúc.", [nameof(EndDate)]);
        if (ApplicationDeadline <= DateTime.MinValue.AddHours(7)) yield return new("Vui lòng chọn hạn ứng tuyển hợp lệ.", [nameof(ApplicationDeadline)]);
        if (StartDate.Date > EndDate.Date) yield return new("Ngày kết thúc phải từ ngày bắt đầu trở đi.", [nameof(EndDate)]);
    }
}

public class CompanyPositionRow
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public string MajorName { get; set; } = "";
    public string PeriodName { get; set; } = "";
    public InternshipPositionStatus Status { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int Quantity { get; set; }
    public int ApplicationCount { get; set; }
    public int AcceptedCount { get; set; }
}

public class CompanyPositionListViewModel
{
    [StringLength(200, ErrorMessage = "Từ khóa tối đa 200 ký tự."), Display(Name = "Tìm theo tên hoặc mã")]
    public string? Keyword { get; set; }
    [EnumDataType(typeof(InternshipPositionStatus), ErrorMessage = "Trạng thái không hợp lệ."), Display(Name = "Trạng thái")]
    public InternshipPositionStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public bool CanManage { get; set; }
    public List<CompanyPositionRow> Positions { get; set; } = [];
}
