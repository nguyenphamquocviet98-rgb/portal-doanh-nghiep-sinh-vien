using System.Globalization;
using System.Text;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        if (!environment.IsDevelopment())
            throw new InvalidOperationException("Dữ liệu demo chỉ được tạo trong môi trường Development.");

        var db = services.GetRequiredService<ApplicationDbContext>();
        var users = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");
        var password = configuration["Seed:DemoPassword"];
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Chưa cấu hình Seed:DemoPassword trong User Secrets.");

        if (await users.FindByEmailAsync("admin@demo.edu.vn") is not null)
        {
            if (await db.Students.CountAsync() < 20 || await db.Companies.CountAsync() < 6 || await db.InternshipPositions.CountAsync() < 18)
                throw new InvalidOperationException("Dữ liệu demo chưa đầy đủ. Kiểm tra dữ liệu hiện có trước khi bổ sung, không xóa database.");
            logger.LogInformation("Dữ liệu demo đã có; giữ nguyên tài khoản và dữ liệu hiện tại.");
            return;
        }

        var createdFiles = new List<string>();
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            foreach (var role in new[] { "Admin", "Company", "Student" })
                if (!await roles.RoleExistsAsync(role))
                    Check(await roles.CreateAsync(new IdentityRole(role)));

            await CreateUserAsync(users, "admin@demo.edu.vn", "Quản trị Nhà trường", "Admin", password);
            var faculties = new[]
            {
                new Faculty { Code = "CNTT", Name = "Khoa Công nghệ thông tin" },
                new Faculty { Code = "KT", Name = "Khoa Kinh tế" },
                new Faculty { Code = "NN", Name = "Khoa Ngoại ngữ" }
            };
            db.Faculties.AddRange(faculties);
            await db.SaveChangesAsync();
            var majors = new[]
            {
                new Major { Code = "KTPM", Name = "Kỹ thuật phần mềm", FacultyId = faculties[0].Id },
                new Major { Code = "HTTT", Name = "Hệ thống thông tin", FacultyId = faculties[0].Id },
                new Major { Code = "QTKD", Name = "Quản trị kinh doanh", FacultyId = faculties[1].Id },
                new Major { Code = "MKT", Name = "Marketing", FacultyId = faculties[1].Id },
                new Major { Code = "NNA", Name = "Ngôn ngữ Anh", FacultyId = faculties[2].Id },
                new Major { Code = "NNT", Name = "Ngôn ngữ Trung Quốc", FacultyId = faculties[2].Id }
            };
            db.Majors.AddRange(majors);
            var today = DateTime.UtcNow.Date;
            var period = new InternshipPeriod
            {
                Code = "DEMO-01", Name = "Đợt thực tập mẫu", StartDate = today.AddDays(-120),
                EndDate = today.AddDays(120), Status = InternshipPeriodStatus.Open
            };
            db.InternshipPeriods.Add(period);
            await db.SaveChangesAsync();

            var companyNames = new[] { "Công ty Công nghệ An Việt", "Công ty Giải pháp Sao Mai", "Công ty Thương mại Minh An",
                "Công ty Truyền thông Hướng Dương", "Công ty Dịch vụ Ngôn ngữ Kết Nối", "Công ty Công nghệ Tân Phát" };
            var companies = new List<Company>();
            for (var i = 0; i < companyNames.Length; i++)
            {
                var email = i == 0 ? "company@demo.vn" : $"company{i + 1:00}@demo.vn";
                var user = await CreateUserAsync(users, email, $"Đại diện {companyNames[i]}", "Company", password);
                companies.Add(new Company
                {
                    UserId = user.Id, Name = companyNames[i], Address = $"{20 + i * 10} Nguyễn Văn Cừ, TP. Hồ Chí Minh",
                    BusinessField = "Đào tạo và phát triển nhân lực", Description = "Doanh nghiệp giả lập dùng cho kịch bản demo đồ án.",
                    ApprovalStatus = i == 5 ? CompanyApprovalStatus.Pending : CompanyApprovalStatus.Approved
                });
            }
            db.Companies.AddRange(companies);
            await db.SaveChangesAsync();

            var titles = new[] { "Thực tập sinh lập trình .NET", "Thực tập sinh phân tích hệ thống", "Thực tập sinh kinh doanh",
                "Thực tập sinh Marketing", "Thực tập sinh biên dịch tiếng Anh", "Thực tập sinh biên dịch tiếng Trung" };
            var positions = new List<InternshipPosition>();
            for (var i = 0; i < 18; i++)
            {
                var historical = i is >= 4 and <= 7;
                var start = historical ? today.AddDays(-90) : i is 2 or 3 ? today.AddDays(-20) : today.AddDays(10);
                var end = historical ? today.AddDays(-10) : i is 2 or 3 ? today.AddDays(40) : today.AddDays(90);
                positions.Add(new InternshipPosition
                {
                    Code = $"TT-DEMO-{i + 1:000}", Title = titles[i % titles.Length], CompanyId = companies[i % 5].Id,
                    MajorId = majors[i % majors.Length].Id, InternshipPeriodId = period.Id,
                    Description = "Tham gia công việc thực tế cùng nhóm chuyên môn và được hướng dẫn theo từng tuần.",
                    Requirements = "Có kiến thức nền tảng của ngành, chủ động học hỏi và có tinh thần trách nhiệm.",
                    Quantity = 3, Location = i % 3 == 0 ? "TP. Hồ Chí Minh" : i % 3 == 1 ? "Hà Nội" : "Đà Nẵng",
                    WorkMode = (WorkMode)(i % 3), StartDate = start, EndDate = end,
                    ApplicationDeadline = historical ? today.AddDays(-100) : today.AddDays(7 + i),
                    Status = historical ? InternshipPositionStatus.Closed : InternshipPositionStatus.Open,
                    CreatedAt = historical ? today.AddDays(-110) : today.AddDays(-25)
                });
            }
            db.InternshipPositions.AddRange(positions);
            await db.SaveChangesAsync();

            var studentNames = new[] { "Nguyễn Minh Anh", "Trần Quốc Bảo", "Lê Hoàng Nam", "Phạm Thảo Vy", "Võ Gia Huy",
                "Đặng Thu Hà", "Bùi Tuấn Kiệt", "Đỗ Ngọc Linh", "Hồ Minh Đức", "Ngô Thanh Trúc",
                "Dương Hải Đăng", "Lý Khánh An", "Mai Phương Thảo", "Huỳnh Nhật Minh", "Phan Bảo Ngọc",
                "Cao Quỳnh Như", "Tạ Đức Thành", "Đinh Hồng Nhung", "Vũ Anh Khoa", "Trương Mỹ Duyên" };
            var students = new List<Student>();
            for (var i = 0; i < studentNames.Length; i++)
            {
                var email = i == 0 ? "student@demo.edu.vn" : $"student{i + 1:00}@demo.edu.vn";
                var user = await CreateUserAsync(users, email, studentNames[i], "Student", password);
                students.Add(new Student
                {
                    UserId = user.Id, StudentCode = $"DEMO{i + 1:000}", MajorId = majors[i % majors.Length].Id,
                    ClassName = $"DH{20 + i % 3}", Skills = "Làm việc nhóm, tin học văn phòng",
                    Bio = "Sinh viên giả lập dùng để kiểm tra nghiệp vụ.",
                    CvPath = CreateDemoPdf(environment, $"Demo CV - Student {i + 1:00}", createdFiles)
                });
            }
            db.Students.AddRange(students);
            await db.SaveChangesAsync();

            var applications = new List<Application>();
            var otherStatuses = new[] { ApplicationStatus.Pending, ApplicationStatus.Viewed, ApplicationStatus.Interviewed,
                ApplicationStatus.Rejected, ApplicationStatus.Withdrawn };
            for (var i = 0; i < students.Count; i++)
            {
                var status = i < 8 ? ApplicationStatus.Approved : otherStatuses[(i - 8) % otherStatuses.Length];
                var position = positions[i < 8 ? i : 8 + (i - 8) % 10];
                applications.Add(new Application
                {
                    StudentId = students[i].Id, InternshipPositionId = position.Id, CvPath = students[i].CvPath!,
                    CoverLetter = "Em mong muốn học hỏi kinh nghiệm làm việc thực tế tại doanh nghiệp.", Status = status,
                    Note = status == ApplicationStatus.Rejected ? "Kỹ năng hiện tại chưa phù hợp với yêu cầu vị trí." : null,
                    CreatedAt = position.CreatedAt.AddDays(1)
                });
            }
            applications.Add(new Application
            {
                StudentId = students[0].Id, InternshipPositionId = positions[1].Id, CvPath = students[0].CvPath!,
                Status = ApplicationStatus.Cancelled, Note = "Đã được tiếp nhận tại vị trí khác trong cùng đợt.",
                CreatedAt = today.AddDays(-24)
            });
            db.Applications.AddRange(applications);
            await db.SaveChangesAsync();

            var internships = new List<Internship>();
            for (var i = 0; i < 8; i++)
            {
                internships.Add(new Internship
                {
                    ApplicationId = applications[i].Id, StudentId = students[i].Id, InternshipPeriodId = period.Id,
                    StartDate = positions[i].StartDate, EndDate = positions[i].EndDate,
                    Status = (InternshipStatus)(i / 2), SupervisorName = "Nguyễn Văn Hướng Dẫn",
                    SupervisorEmail = "mentor@demo.vn", CreatedAt = applications[i].CreatedAt.AddDays(1)
                });
            }
            db.Internships.AddRange(internships);
            await db.SaveChangesAsync();
            for (var i = 2; i < internships.Count; i++)
            {
                db.InternshipDiaries.Add(new InternshipDiary
                {
                    InternshipId = internships[i].Id, WeekNumber = 1, StartDate = internships[i].StartDate,
                    EndDate = internships[i].StartDate.AddDays(6), WorkContent = "Tìm hiểu quy trình làm việc và thực hiện nhiệm vụ đầu tiên.",
                    Results = "Nắm được công cụ và quy trình của nhóm.", Difficulties = "Cần thêm thời gian làm quen hệ thống.",
                    CreatedAt = internships[i].StartDate.AddDays(6)
                });
                if (i >= 4)
                {
                    db.Evaluations.Add(new Evaluation
                    {
                        InternshipId = internships[i].Id, ProfessionalKnowledge = i < 6 ? 8 : 4,
                        WorkSkills = i < 6 ? 8 : 4, Attitude = 8, Responsibility = i < 6 ? 9 : 5,
                        Communication = 7, TaskCompletion = i < 6 ? 8 : 3,
                        Comment = i < 6 ? "Hoàn thành tốt nhiệm vụ, có tinh thần học hỏi." : "Chưa hoàn thành yêu cầu của kỳ thực tập.",
                        CreatedAt = internships[i].EndDate.AddDays(1)
                    });
                    var path = CreateDemoPdf(environment, $"Demo Internship Report {i + 1:00}", createdFiles);
                    db.InternshipDocuments.Add(new InternshipDocument
                    {
                        InternshipId = internships[i].Id, DocumentType = DocumentType.Report,
                        OriginalFileName = $"Bao-cao-demo-{i + 1:00}.pdf", FilePath = path,
                        FileSize = new FileInfo(Path.Combine(environment.WebRootPath, path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar))).Length,
                        CreatedAt = internships[i].EndDate
                    });
                }
                db.Notifications.Add(new Notification
                {
                    UserId = students[i].UserId, Type = i >= 4 ? NotificationType.NewEvaluation : NotificationType.ApplicationApproved,
                    Title = i >= 4 ? "Đã có đánh giá thực tập" : "Bạn đã được tiếp nhận thực tập",
                    Message = "Thông báo mẫu phục vụ kiểm tra đồ án.", DeduplicationKey = $"demo:internship:{internships[i].Id}",
                    IsRead = i % 2 == 0, ReadAt = i % 2 == 0 ? DateTime.UtcNow : null
                });
            }
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
            logger.LogInformation("Đã tạo demo: 3 khoa, 6 ngành, 1 đợt, 6 doanh nghiệp, 18 vị trí, 20 sinh viên, 21 đơn, 8 hồ sơ, 6 nhật ký, 4 tài liệu, 4 đánh giá, 6 thông báo.");
        }
        catch
        {
            await transaction.RollbackAsync();
            foreach (var path in createdFiles)
                if (File.Exists(path)) File.Delete(path);
            throw;
        }
    }

    private static async Task<ApplicationUser> CreateUserAsync(UserManager<ApplicationUser> users, string email, string fullName, string role, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName, EmailConfirmed = true };
        Check(await users.CreateAsync(user, password));
        Check(await users.AddToRoleAsync(user, role));
        return user;
    }

    private static void Check(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
    }

    // PDF nhỏ hợp lệ để demo tải file; không dùng hồ sơ cá nhân thật.
    private static string CreateDemoPdf(IWebHostEnvironment environment, string title, List<string> createdFiles)
    {
        var folder = Path.Combine(environment.WebRootPath, "uploads", "demo");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid():N}.pdf";
        var content = $"BT /F1 18 Tf 50 780 Td ({title}) Tj 0 -35 Td /F1 11 Tf (Example document for development only.) Tj ET";
        var objects = new[]
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
            $"<< /Length {Encoding.ASCII.GetByteCount(content)} >>\nstream\n{content}\nendstream"
        };
        var pdf = new StringBuilder("%PDF-1.4\n");
        var offsets = new List<int>();
        for (var i = 0; i < objects.Length; i++)
        {
            offsets.Add(Encoding.ASCII.GetByteCount(pdf.ToString()));
            pdf.Append($"{i + 1} 0 obj\n{objects[i]}\nendobj\n");
        }
        var xref = Encoding.ASCII.GetByteCount(pdf.ToString());
        pdf.Append($"xref\n0 {objects.Length + 1}\n0000000000 65535 f \n");
        foreach (var offset in offsets) pdf.Append(offset.ToString("D10", CultureInfo.InvariantCulture)).Append(" 00000 n \n");
        pdf.Append($"trailer\n<< /Size {objects.Length + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF\n");
        var fullPath = Path.Combine(folder, fileName);
        File.WriteAllText(fullPath, pdf.ToString(), Encoding.ASCII);
        createdFiles.Add(fullPath);
        return $"/uploads/demo/{fileName}";
    }
}