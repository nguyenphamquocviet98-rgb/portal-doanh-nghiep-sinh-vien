# Giai đoạn 4 — Tạo dự án ASP.NET Core MVC

## Mục tiêu và kết quả

Tạo project .NET 8 trong InternshipPortal, giữ nguyên dự án Next.js. Có trang chủ tiếng Việt, Razor Views, Bootstrap 5, routing cho Area và thư mục chuẩn bị ba cổng.

## File tạo hoặc sửa

- InternshipPortal/InternshipPortal.csproj và các file template MVC.
- InternshipPortal/Program.cs: thêm route Area; HTTPS khi triển khai, HTTP cho demo Development.
- InternshipPortal/Views/Home/Index.cshtml: thay giao diện mẫu bằng trang chủ đề tài.
- InternshipPortal/Views/Home/Privacy.cshtml: nội dung tiếng Việt ban đầu.
- InternshipPortal/Views/Shared/_Layout.cshtml: ngôn ngữ, menu, tiêu đề, footer.
- InternshipPortal/Views/Shared/Error.cshtml: thông báo lỗi tiếng Việt.
- InternshipPortal/README.md: cách chạy, kiểm tra và Git checkpoint.
- Các thư mục Areas, Data, Services, ViewModels có .gitkeep.
- .gitignore: bỏ qua bin, obj, dữ liệu Visual Studio và uploads.

Không thay schema Prisma, chưa thêm Entity, EF Core, Identity hoặc Migration. Không có form thao tác dữ liệu nên chưa có kiểm thử quyền người dùng.

## Các bước đã thực hiện

1. Kiểm tra SDK: máy có .NET 8.0.424.
2. Tạo project bằng dotnet new mvc với framework net8.0.
3. Đọc file template trước khi chỉnh giao diện và routing.
4. Restore thành công.
5. Build sau sửa cuối: thành công, 0 warning, 0 error.
6. Chạy profile http tại http://localhost:5086 trong Development.
7. Kiểm tra HTTP trang chủ, quyền riêng tư, trang lỗi và tài nguyên CSS/Bootstrap.

## Kiểm tra trực tiếp

Trang chủ, /Home/Privacy, /Home/Error, /css/site.css, Bootstrap CSS và Bootstrap JavaScript đều trả HTTP 200. Nội dung HTML trang chủ được kiểm tra có tiêu đề tiếng Việt, ba đối tượng sử dụng và lang=vi.

Chưa kiểm tra giao diện bằng trình duyệt tự động; người dùng có thể mở trang và thu nhỏ cửa sổ để kiểm tra menu và bố cục. Chưa có database hoặc đăng nhập ở giai đoạn này.

## Chạy lại và lỗi thường gặp

Xem InternshipPortal/README.md. Cổng 5086 bị chiếm: dừng bản đang chạy hoặc dùng cổng khác. Không tìm thấy dotnet: cài SDK .NET 8 và mở lại terminal. Đang ở sai thư mục: dùng --project trỏ đúng InternshipPortal.csproj.

## Trạng thái dự án

Đã hoàn thành: khung ASP.NET Core MVC giai đoạn 4, build và chạy kiểm tra HTTP.

Đang thực hiện: chờ người dùng kiểm tra trang chủ.

Bước tiếp theo: giai đoạn 5 — EF Core, SQL Server, Migration và Seed Data; kiểm tra SQL Server/LocalDB và chốt các giá trị cần thiết trước khi tạo Entity liên quan.
