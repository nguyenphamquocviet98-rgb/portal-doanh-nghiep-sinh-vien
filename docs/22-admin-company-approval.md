# Admin — duyệt doanh nghiệp

Ngày thực hiện: 09/10/2026.

## Kết quả

`/Admin/Companies` có tìm tên/email, lọc trạng thái và phân trang 10 doanh nghiệp. `/Admin/Companies/Details/{id}` hiển thị hồ sơ và ghi chú xét duyệt. Dashboard Admin có liên kết đến danh sách.

Chỉ hồ sơ Chờ duyệt với doanh nghiệp/tài khoản đang hoạt động được xử lý. Admin duyệt hoặc từ chối; từ chối bắt buộc lý do, tối đa 990 ký tự. Mốc này chỉ xử lý lần đầu, chưa mở lại hồ sơ bị từ chối hoặc thu hồi quyết định duyệt.

POST có CSRF và role Admin. Service kiểm tra lại tài khoản Admin đang hoạt động/còn role, trạng thái doanh nghiệp và phiên bản UpdatedAt/CreatedAt trong transaction Serializable. Hồ sơ thay đổi hoặc đã xử lý yêu cầu tải lại. Quyết định và thông báo lưu cùng nhau, khóa thông báo `company-review:{id}` chống trùng. Duyệt dùng loại CompanyApproved; từ chối dùng Manual và lưu lý do. Thông báo dẫn về `/Company/Profile`; danh sách thông báo chưa được triển khai.

Không đổi schema, không cần migration. Duyệt cho phép doanh nghiệp đăng vị trí qua quy tắc Company hiện có. Từ chối hiển thị lý do trong hồ sơ và tiếp tục chặn tạo vị trí.

## File

- `InternshipPortal/ViewModels/AdminCompanies/AdminCompanyViewModels.cs`
- `InternshipPortal/Services/AdminCompanyService.cs`
- `InternshipPortal/Areas/Admin/Controllers/CompaniesController.cs`
- `InternshipPortal/Areas/Admin/Views/Companies/Index.cshtml`
- `InternshipPortal/Areas/Admin/Views/Companies/Details.cshtml`
- `InternshipPortal/Areas/Admin/Views/Dashboard/Index.cshtml`: liên kết.
- `InternshipPortal/Program.cs`: đăng ký service.

## Kiểm tra thực tế

Build đạt 0 lỗi, 0 cảnh báo. App chạy localhost:5091, SQL Server hoạt động. Script `.preview/check-admin-companies.py` kiểm tra quyền GET/POST, CSRF, bộ lọc, hồ sơ không tồn tại, no-store, trạng thái không hợp lệ, lý do từ chối, giới hạn ghi chú, phiên bản cũ, rollback khi thông báo trùng, quyết định đồng thời và gửi lại. Kiểm tra từ chối hiển thị lý do và chặn Create vị trí; duyệt mở Create và tạo thông báo đúng loại.

Hồ sơ `company06@demo.vn` dùng tạm kiểm thử: trạng thái, ghi chú, UpdatedAt đã khôi phục và so sánh đúng ban đầu; thông báo thử đã xóa trong finally. Không tạo vị trí mới. Chưa kiểm tra bố cục trên trình duyệt/điện thoại, tải lớn hay phân trang với trên 10 doanh nghiệp.

## Chạy và demo

```powershell
dotnet run --project InternshipPortal
```

Dừng app cũ nếu cổng mặc định đang được sử dụng. Bản kiểm tra hiện tại: `http://localhost:5091/Admin/Companies`.

1. Đăng nhập `admin@demo.edu.vn`, lọc Chờ duyệt và mở hồ sơ.
2. Duyệt hoặc nhập lý do rồi từ chối. Đây là thao tác thay đổi dữ liệu demo thực tế.
3. Đăng nhập doanh nghiệp tương ứng, xem trạng thái/lý do ở hồ sơ và thử tạo vị trí.
4. Gửi lại form cũ: không được xử lý thêm lần nữa.

Nếu hồ sơ thay đổi, tải lại thông tin trước khi xử lý. Hồ sơ đã duyệt/từ chối hoặc tài khoản ngừng hoạt động không có form quyết định.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc Student/Company trước đó; Admin danh sách, chi tiết và duyệt/từ chối doanh nghiệp.

Đang thực hiện: giai đoạn Admin, dừng ở mốc duyệt doanh nghiệp.

Bước tiếp theo: quản lý khoa/ngành phía Admin. Chờ người dùng nói “tiếp tục”.
