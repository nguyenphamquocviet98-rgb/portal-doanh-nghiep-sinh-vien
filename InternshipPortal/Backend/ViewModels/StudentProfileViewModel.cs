namespace InternshipPortal.ViewModels;

public class StudentProfileViewModel
{
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string StudentCode { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string FacultyName { get; set; } = "";
    public string MajorName { get; set; } = "";
    public decimal? Gpa { get; set; }
    public string? Skills { get; set; }
    public string? Bio { get; set; }
    public bool HasCv { get; set; }
}