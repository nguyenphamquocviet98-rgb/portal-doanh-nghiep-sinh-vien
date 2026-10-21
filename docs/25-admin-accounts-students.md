# Admin tài khoản/sinh viên — 09/10/2026

## Kết quả

`/Admin/Accounts`: tìm tên/email/mã sinh viên, lọc Student/Company/Admin, phân trang 10 tài khoản. `/Admin/Accounts/Details/{id}`: thông tin liên hệ, nhóm quyền, hoạt động; Student có mã, lớp, ngành, khoa; Company có tên doanh nghiệp. Dashboard Admin có liên kết.

Admin bật/tắt IsActive của tài khoản ngoài nhóm Admin. Không xóa hồ sơ/lịch sử, đổi role hoặc sửa thông tin học tập ở mốc này. Tài khoản Admin không được ngừng hoạt động qua chức năng này. Quyết định duyệt doanh nghiệp và trạng thái tài khoản là hai trường riêng, không tự thay đổi lẫn nhau.

Role Admin và ActiveAccountFilter bảo vệ action; POST có CSRF, nullable bool bắt buộc và phiên bản ConcurrencyStamp. Transaction Serializable kiểm tra role tài khoản đích và phiên bản trước khi lưu; stamp mới chống form cũ, xử lý xung đột/deadlock bằng thông báo tải lại. Tài khoản ngừng hoạt động bị ActiveAccountFilter đăng xuất khi yêu cầu tiếp theo; chức năng login hiện có chặn đăng nhập. Không cần migration.

## File

- `InternshipPortal/ViewModels/AdminAccountsViewModel.cs`
- `InternshipPortal/Areas/Admin/Controllers/AccountsController.cs`
- `InternshipPortal/Areas/Admin/Views/Accounts/Index.cshtml`
- `InternshipPortal/Areas/Admin/Views/Accounts/Details.cshtml`
- `InternshipPortal/Areas/Admin/Views/Dashboard/Index.cshtml`: liên kết.

## Kiểm tra thực tế

Build 0 lỗi, 0 cảnh báo; app/SQL Server chạy trên localhost:5086. `.preview/check-admin-accounts.py` đạt: chặn Company/Student GET và POST, khách chuyển login, CSRF, ID không tồn tại, tìm mã sinh viên và lọc role, hồ sơ học tập, no-store, bool sai, ngừng hoạt động, phiên cũ bị đăng xuất, login bị chặn, form cũ không kích hoạt lại, kích hoạt mới cho phép login, không tắt Admin.

Tài khoản demo student09 dùng tạm; IsActive/ConcurrencyStamp đã khôi phục chính xác trong finally. Không đổi hồ sơ học tập hoặc dữ liệu nghiệp vụ. Kiểm tra lại điều hướng Company trên bản mới đạt. Chưa kiểm tra tải lớn, thao tác đồng thời thực tế hoặc giao diện mobile của trang Admin.

## Demo

Mở `http://localhost:5086/Admin/Accounts`, đăng nhập `admin@demo.edu.vn`. Tìm DEMO009, xem hồ sơ, ngừng hoạt động và kiểm tra Student không còn truy cập được, sau đó kích hoạt lại. Không chọn tài khoản đang phục vụ demo khác để tránh gián đoạn thao tác.

Chạy lại từ thư mục gốc:

```powershell
dotnet run --project InternshipPortal
```

Dừng bản app cũ nếu cổng 5086 đang sử dụng. Nếu báo dữ liệu vừa thay đổi, tải lại trước khi thao tác lại.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc trước; Admin danh sách/chi tiết tài khoản và hồ sơ sinh viên, bật/tắt hoạt động tài khoản.

Đang thực hiện: giai đoạn Admin, dừng ở mốc tài khoản/sinh viên.

Bước tiếp theo: Admin giám sát vị trí/ứng tuyển/thực tập. Sửa điểm đã gửi, giao diện thông báo, dashboard/thống kê và kiểm thử toàn hệ thống vẫn còn. Chờ người dùng nói “tiếp tục”.
