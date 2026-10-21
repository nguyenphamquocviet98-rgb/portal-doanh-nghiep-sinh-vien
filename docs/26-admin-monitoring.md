# Admin giám sát vị trí, ứng tuyển, thực tập — 09/10/2026

## Kết quả

- `/Admin/Monitoring/Positions`: vị trí toàn hệ thống, gồm doanh nghiệp và đợt.
- `/Admin/Monitoring/Applications`: đơn ứng tuyển, sinh viên/mã, vị trí, doanh nghiệp và trạng thái.
- `/Admin/Monitoring/Internships`: hồ sơ thực tập, sinh viên/mã, doanh nghiệp, đợt và trạng thái.

Mỗi danh sách tìm tên vị trí/doanh nghiệp; ứng tuyển/thực tập tìm thêm tên/mã sinh viên. Phân trang 10 bản ghi, giới hạn số trang về khoảng hợp lệ. Các trang Position/Application/Internship theo ID hiển thị thông tin nghiệp vụ và liên kết sang hồ sơ sinh viên, doanh nghiệp, vị trí, đơn được tiếp nhận. Dashboard Admin có liên kết và các danh sách có menu chuyển mục.

Mốc này chỉ xem dữ liệu, không sửa hoặc xóa; hiển thị cả dữ liệu lịch sử thuộc doanh nghiệp ngừng hoạt động/chưa duyệt để nhà trường theo dõi. Chưa có tải CV/tài liệu, xem nhật ký hay đánh giá phía Admin trong mốc này.

Role Admin và ActiveAccountFilter bảo vệ mọi action, cache no-store. ID không tồn tại trả 404. Không đổi database/schema, không cần migration.

## File

- `InternshipPortal/ViewModels/AdminMonitoringViewModel.cs`
- `InternshipPortal/Areas/Admin/Controllers/MonitoringController.cs`
- `InternshipPortal/Areas/Admin/Views/Monitoring/Index.cshtml`
- `InternshipPortal/Areas/Admin/Views/Monitoring/Position.cshtml`
- `InternshipPortal/Areas/Admin/Views/Monitoring/Application.cshtml`
- `InternshipPortal/Areas/Admin/Views/Monitoring/Internship.cshtml`
- `InternshipPortal/Areas/Admin/Views/Dashboard/Index.cshtml`: liên kết.

## Kiểm tra thực tế

Build 0 lỗi, 0 cảnh báo; app và SQL Server hoạt động trên localhost:5086. Script `.preview/check-admin-monitoring.py` đạt: cả ba danh sách/chi tiết, đếm khớp database, giới hạn 10 dòng, tìm không có kết quả, số trang âm/quá lớn, tìm mã sinh viên, chặn Student/Company, khách chuyển đăng nhập, ID không tồn tại, cache no-store. Lỗi truy vấn EF khi tìm vị trí đã sửa bằng cách chỉ tìm các trường vị trí/doanh nghiệp; chạy lại kiểm tra đạt. Không thay đổi dữ liệu khi kiểm thử. Chưa kiểm tra trình duyệt thật/mobile hoặc tải lớn.

## Demo

Đăng nhập `admin@demo.edu.vn`, vào `http://localhost:5086/Admin/Monitoring/Positions`. Chuyển qua Ứng tuyển/Thực tập, tìm DEMO003 rồi mở chi tiết và liên kết hồ sơ sinh viên. Thử từ tài khoản Company hoặc Student phải bị từ chối.

Chạy lại từ thư mục gốc: `dotnet run --project InternshipPortal`; dừng bản đang dùng 5086 trước khi chạy lại.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc trước và Admin danh sách/chi tiết giám sát vị trí, ứng tuyển, thực tập.

Đang thực hiện: giai đoạn Admin, dừng tại mốc giám sát cơ bản.

Bước tiếp theo: Admin xem nhật ký, tài liệu và kết quả đánh giá thực tập. Chờ người dùng nói “tiếp tục”.
