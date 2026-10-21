# Chia Frontend / Backend — 09/10/2026

Đã chuyển toàn bộ Views và Areas/*/Views sang Frontend; wwwroot (kể cả file tải lên hiện có) sang Frontend/wwwroot. Controllers, Areas/*/Controllers, Services, Models, Data, Migrations, Filters, Helpers, ViewModels sang Backend. Program.cs/project/configuration giữ ở gốc làm điểm khởi động chung.

Namespace không đổi, không thay schema hoặc dữ liệu SQL. Đường dẫn Razor tuyệt đối đã cập nhật; RazorViewEngine tìm view theo thư mục mới, WebRootPath là Frontend/wwwroot. URL trang/CSS/JS và đường dẫn file lưu trong database không đổi. Project copy tài nguyên Frontend vào output/publish, loại uploads khỏi đóng gói.

Build và publish thành công, 0 lỗi/0 cảnh báo. Chạy thực tế trên localhost:5086, kiểm tra HTTP Company toàn bộ menu/form/xem đánh giá, Admin giám sát và các trang quản trị, Student hồ sơ/CV/ứng tuyển/thực tập, trang công khai, CSS/JS/Bootstrap và tải CV riêng tư: đạt. Uploads vẫn bị chặn truy cập trực tiếp. Publish chứa CSS/JS/lib và không chứa uploads. Chưa kiểm tra toàn bộ POST nghiệp vụ lại hoặc trực quan mọi trang.

Các đường dẫn trong tài liệu cũ được viết trước khi chuyển cấu trúc: thêm Backend vào các đường dẫn mã C#, Frontend vào các đường dẫn Views/wwwroot. Script kiểm tra cũ thao tác trực tiếp wwwroot cần đổi sang Frontend/wwwroot trước khi sử dụng; không chạy seed lại để di chuyển file.

Chạy vẫn bằng `dotnet run --project InternshipPortal` từ thư mục gốc. App mới nhất đang chạy trên 5086. Đây là một ứng dụng MVC được phân chia thư mục, chưa tách thành API/frontend riêng.
