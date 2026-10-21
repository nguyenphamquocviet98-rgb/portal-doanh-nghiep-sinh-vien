using System.ComponentModel.DataAnnotations;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternshipPortal.ViewModels.Internships;

public class StudentDocumentsViewModel
{
    public int InternshipId { get; set; }
    public string PositionTitle { get; set; } = "";
    public string CompanyName { get; set; } = "";
    public InternshipStatus Status { get; set; }
    public bool CanUpload => Status is InternshipStatus.InProgress or InternshipStatus.Completed or InternshipStatus.Failed;
    public List<StudentDocumentViewModel> Documents { get; set; } = [];
}

public class StudentDocumentViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DocumentType DocumentType { get; set; }
    public long FileSize { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool HasFile { get; set; }
}

public class UploadDocumentViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Hồ sơ thực tập chưa hợp lệ.")]
    public int InternshipId { get; set; }
    [EnumDataType(typeof(DocumentType), ErrorMessage = "Loại tài liệu chưa hợp lệ.")]
    public DocumentType DocumentType { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn file để nộp.")]
    public IFormFile? File { get; set; }
}