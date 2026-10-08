# Cổng thông tin Thực tập và Kết nối Doanh nghiệp

Giai đoạn 4: khung ASP.NET Core MVC .NET 8, Razor Views và Bootstrap 5. Chưa có database hoặc đăng nhập.

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
- Areas/Admin, Areas/Company, Areas/Student: thư mục chuẩn bị, chưa có chức năng.
- Data, Services, ViewModels: chuẩn bị cho giai đoạn tiếp theo.

## Checklist

- Trang chủ hiển thị tiếng Việt và ba thẻ đối tượng sử dụng.
- Menu Quyền riêng tư truy cập được.
- CSS và Bootstrap tải được.
- Khi thu nhỏ cửa sổ, kiểm tra thẻ xếp dọc và menu thu gọn.

Chưa có Identity hoặc EF Core nên chưa kiểm tra phân quyền hay database.

## Git checkpoint tùy chọn

Tự chạy `git status` để xem file thay đổi, rồi `git diff` để xem nội dung. Chỉ khi đã kiểm tra, chạy `git add .` để chọn file và `git commit -m "feat: initialize ASP.NET Core MVC project"` để lưu mốc. Xem kỹ danh sách trước khi add vì thư mục còn dự án Next.js. AI chưa commit hoặc push.
