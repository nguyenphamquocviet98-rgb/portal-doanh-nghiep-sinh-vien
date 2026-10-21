# Cổng thông tin Thực tập và Kết nối Doanh nghiệp

Đã có ASP.NET Core MVC .NET 8, Razor Views, Bootstrap 5, EF Core 8/SQL Server, đăng nhập và đăng ký bằng Identity. Có cổng ban đầu cho Student, Company và Admin.

## Chạy từ PowerShell

Cần .NET SDK 8. Từ thư mục D:\Portal\code:

```powershell
dotnet restore .\InternshipPortal\InternshipPortal.csproj
dotnet build .\InternshipPortal\InternshipPortal.csproj --no-restore
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Restore chuẩn bị dependency; build biên dịch và kiểm tra lỗi; run khởi động website.

Mở http://localhost:5086. Dừng bằng Ctrl+C tại terminal đang chạy. Nếu cổng bị chiếm, dừng bản đang chạy hoặc thêm `--urls http://localhost:5087` vào lệnh run.

## Các file chính

- Program.cs: cấu hình dịch vụ, HTTP local và routing MVC/Area.
- Controllers/HomeController.cs: trả trang chủ, quyền riêng tư và lỗi.
- Views/Home/Index.cshtml: trang chủ tiếng Việt.
- Views/Home/Privacy.cshtml: thông tin quyền riêng tư ban đầu.
- Views/Shared/_Layout.cshtml: menu, footer, Bootstrap.
- Views/Shared/Error.cshtml: thông báo lỗi tiếng Việt.
- Controllers/AccountController.cs: đăng nhập, đăng ký, đăng xuất và chuyển cổng theo role.
- Services/AccountService.cs: tạo tài khoản, role và hồ sơ trong một transaction.
- Filters/ActiveAccountFilter.cs: chặn tài khoản bị khóa/ngừng hoạt động cả trong phiên đang đăng nhập.
- Areas/Admin, Areas/Company, Areas/Student: trang đầu của các cổng, kiểm tra role và lấy hồ sơ theo user đăng nhập.
- Models: 12 Entity nghiệp vụ, ApplicationUser và enum.
- Data/ApplicationDbContext.cs: các bảng, quan hệ, unique index và check constraint.
- Data/SeedData.cs: dữ liệu demo và tài khoản mẫu.
- Migrations: lịch sử thay đổi cấu trúc database.
- ViewModels/Account: các form có giới hạn độ dài khớp database.

## Checklist

- Trang chủ hiển thị tiếng Việt và ba thẻ đối tượng sử dụng.
- Menu Quyền riêng tư truy cập được.
- CSS và Bootstrap tải được.
- Khi thu nhỏ cửa sổ, kiểm tra thẻ xếp dọc và menu thu gọn.

Database, đăng ký, đăng nhập và phân quyền cơ bản đã được kiểm tra. Hướng dẫn giai đoạn 6: [Tài khoản và phân quyền](../docs/08-authentication.md).

Mở http://localhost:5086/Account/Login hoặc http://localhost:5086/Account/Register. Sau đăng nhập, hệ thống chuyển về đúng cổng.

Student đã có xem/sửa hồ sơ và CV tại http://localhost:5086/Student/Profile. Chỉ nhận CV PDF tối đa 10 MB; thay CV giữ nguyên bản trong các đơn đã gửi. Phần GPA chờ xác nhận cách hỗ trợ thang điểm. Xem [Hồ sơ và CV](../docs/09-student-profile.md).

Mẫu CV hai cột tại http://localhost:5086/Student/Profile/Cv: lấy thông tin từ hồ sơ, cho chỉnh nội dung/ảnh tại trang, in hoặc lưu PDF khổ A4 rồi tải lên CV. Nội dung soạn thêm không tự lưu vào hồ sơ; hãy giữ bản PDF trước khi rời trang. Xem [Mẫu CV](../docs/10-cv-template.md).

Danh sách vị trí công khai tại http://localhost:5086/Internships, tìm theo tên và lọc ngành/địa điểm/doanh nghiệp/hình thức; có trang chi tiết vị trí và doanh nghiệp. Chỉ công khai vị trí đang tuyển, chưa hết hạn, doanh nghiệp đã duyệt và đang hoạt động. Xem [Tìm kiếm vị trí](../docs/11-position-catalog.md).

Student có thể gửi đơn từ chi tiết vị trí, xem lịch sử tại http://localhost:5086/Student/Applications và rút đơn Chờ duyệt. CV được giữ theo thời điểm gửi; không gửi lại vị trí đã ứng tuyển. Để thử gửi mới bằng dữ liệu demo, dùng student09@demo.edu.vn; student@demo.edu.vn đã được tiếp nhận trong đợt nên bị chặn gửi thêm đúng quy tắc. Xem [Ứng tuyển](../docs/12-student-applications.md).

Theo dõi thực tập và nhật ký tại http://localhost:5086/Student/Internships. Dùng student03@demo.edu.vn để thử hồ sơ Đang thực tập và tạo/sửa nhật ký. Tuần được tính từ ngày bắt đầu, mỗi tuần 7 ngày; hồ sơ đã kết thúc chỉ xem. Xem [Thực tập và nhật ký](../docs/13-student-internships-diaries.md).

Trong hồ sơ thực tập có Báo cáo & tài liệu (PDF/DOCX tối đa 10 MB) và Kết quả đánh giá (6 tiêu chí, điểm trung bình tự tính). Dùng student05@demo.edu.vn để xem dữ liệu đánh giá mẫu. Xem [Tài liệu và đánh giá](../docs/14-student-documents-evaluation.md).

Đã tiếp tục ngày 09/10/2026: Company có xem/sửa hồ sơ tại `/Company/Profile`, giữ nguyên trạng thái duyệt. Xem [Hồ sơ doanh nghiệp](../docs/15-company-profile.md). [Kết quả ban đầu](../KET_QUA_BAN_DAU.md) ghi lại mốc tối 08/10/2026.

Company đã có đăng/sửa/đóng vị trí và danh sách riêng tại `/Company/Positions`. Khi đã có đơn, khóa ngành và đợt thực tập. Xem [Quản lý vị trí](../docs/16-company-positions.md).

Company đã có danh sách ứng viên, hồ sơ và tải CV của đơn tại `/Company/Applications`. Xem [Ứng viên và CV](../docs/17-company-applicants.md). Đã bổ sung chuyển trạng thái, từ chối và duyệt tạo Internship; duyệt tự hủy các đơn chưa kết thúc cùng đợt theo xác nhận của người dùng. Xem [Xử lý ứng viên](../docs/18-company-decisions.md).

## Database và dữ liệu demo

Máy hiện tại dùng SQL Server mặc định tại localhost, Windows Authentication, database InternshipPortalDb. Connection String local nằm trong appsettings.Development.json, không chứa mật khẩu.

Hướng dẫn đầy đủ: [Giai đoạn 5](../docs/06-database-setup.md).

Từ thư mục InternshipPortal, chuẩn bị máy khác:

```powershell
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet ef database update
dotnet user-secrets set 'Seed:DemoPassword' '<mat-khau-demo-cua-ban>'
dotnet run --launch-profile http -- --seed-demo
```

Thay nội dung trong dấu <...> bằng mật khẩu demo dài ít nhất 10 ký tự, có chữ hoa, chữ thường, số và ký tự đặc biệt. Không dùng mật khẩu cá nhân thật. Seed tự kết thúc sau khi tạo dữ liệu; dùng lệnh run bình thường để mở web.

Mật khẩu trên máy hiện tại đã được tạo ngẫu nhiên và lưu trong User Secrets. Có thể xem tại terminal cá nhân bằng `dotnet user-secrets list`; không chia sẻ đầu ra này. Không commit mật khẩu.

Tài khoản demo: admin@demo.edu.vn, company@demo.vn, student@demo.edu.vn. Các tài khoản demo được tạo ban đầu dùng chung mật khẩu Seed:DemoPassword. Chạy seed lại giữ nguyên dữ liệu; đổi secret không tự đổi mật khẩu tài khoản đã tạo.

CV và báo cáo mẫu nằm trong wwwroot/uploads/demo, được bỏ qua bởi Git. Truy cập trực tiếp /uploads trả 404; action tải có phân quyền sẽ làm ở giai đoạn chức năng.

## Git checkpoint tùy chọn

Tự chạy `git status` để xem file thay đổi, rồi `git diff` để xem nội dung. Sau khi kiểm tra, từ thư mục gốc dùng `git add InternshipPortal docs PROJECT_REQUIREMENTS.md` để chọn code/tài liệu và `git commit -m "feat: add database and Identity authentication"` để lưu mốc. AI chưa commit hoặc push phần giai đoạn 5–6. Commit trước f9e0096 đã lưu giao diện đầy đủ.
# Cấu trúc Frontend / Backend

Giao diện ở `Frontend/Views`, `Frontend/Areas/*/Views`; CSS/JavaScript ở `Frontend/wwwroot`. Mã xử lý ở `Backend/Controllers`, `Backend/Areas/*/Controllers`, `Backend/Services`, `Backend/Models`, `Backend/Data`, `Backend/Migrations`, `Backend/Filters`, `Backend/Helpers` và `Backend/ViewModels`.

Đây vẫn là một ứng dụng ASP.NET Core MVC. `Program.cs`, project và cấu hình chạy giữ ở gốc. Namespace và database giữ nguyên. MVC đã cấu hình tìm view trong Frontend, WebRoot chuyển sang Frontend/wwwroot. File tải lên nằm ở `Frontend/wwwroot/uploads`, tải qua action có kiểm tra quyền và không được đưa vào bản publish tự động.
