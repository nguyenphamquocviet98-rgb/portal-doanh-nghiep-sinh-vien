# Ứng viên và CV Company — 09/10/2026

## Kết quả

- `/Company/Applications`: danh sách đơn gửi vào các vị trí của doanh nghiệp, tìm tên/MSSV, lọc vị trí và trạng thái, phân trang 10 đơn.
- `/Company/Applications/Details/{id}`: thông tin sinh viên hiện tại, lời giới thiệu và ghi chú của đơn, trạng thái và ngày gửi.
- `/Company/Applications/DownloadCv/{id}`: tải bản CV theo thời điểm ứng tuyển, không lấy CV hiện tại trong hồ sơ Student.
- Liên kết từ Dashboard Company và từng vị trí trong trang quản lý vị trí.

Chỉ role Company với doanh nghiệp đã duyệt và tài khoản/hồ sơ đang hoạt động được truy cập. Cả danh sách, chi tiết và CV đều truy vấn theo tài khoản đăng nhập. Đơn/vị trí của doanh nghiệp khác trả 404. CV tải có `application/pdf`, attachment, `nosniff`, `private, no-store`; trang chứa thông tin sinh viên cũng không được cache.

Việc mở hồ sơ không tự chuyển đơn sang Đã xem. Đã bổ sung thao tác cập nhật trạng thái, từ chối và duyệt tạo Internship trong [Xử lý ứng viên](18-company-decisions.md). GPA vẫn chờ chốt thang điểm, không hiển thị số thiếu ngữ cảnh.

## File chính

Các đường dẫn code tính từ `InternshipPortal`:

- `ViewModels/CompanyApplications/CompanyApplicationViewModels.cs`: form lọc và dữ liệu hiển thị.
- `Services/CompanyApplicationService.cs`: truy vấn danh sách/hồ sơ/CV có quyền sở hữu.
- `Areas/Company/Controllers/ApplicationsController.cs`: role và điều kiện doanh nghiệp; action trả trang và file.
- `Areas/Company/Views/Applications/Index.cshtml`, `Details.cshtml`: danh sách và chi tiết.
- `Program.cs`: đăng ký service.
- Dashboard Company và danh sách Positions: thêm liên kết ứng viên.

Không thay đổi schema, không cần Migration.

## Kiểm tra

- Build thành công, 0 lỗi, 0 cảnh báo; bản kiểm tra khởi động trên cổng 5087 với SQL Server local.
- HTTP qua đăng nhập: Company được duyệt xem được; anonymous về login, Student và Company chờ duyệt bị chặn ở cả danh sách/chi tiết/CV.
- Danh sách chỉ có đơn thuộc doanh nghiệp; chi tiết/CV của doanh nghiệp khác hoặc ID không tồn tại trả 404. Lọc theo vị trí doanh nghiệp khác cũng trả 404.
- Lọc theo MSSV/vị trí/trạng thái trả đúng đơn đã biết; từ khóa không có kết quả hiển thị danh sách rỗng; enum sai báo lỗi.
- CV tải đúng byte của file ứng tuyển trong database; kiểm tra MIME, download và headers bảo vệ. Truy cập trực tiếp uploads trả 404.
- Xem hồ sơ không đổi trạng thái đơn. Kiểm tra chỉ đọc, không sửa dữ liệu demo.
- Chưa xác minh giao diện trên điện thoại thật, chưa tạo bộ dữ liệu lớn để kiểm tra nhiều trang.

## Chạy và demo

Từ `D:\Portal\code`, nếu cần chạy lại:

```powershell
dotnet run --project InternshipPortal
```

Nếu đã có phiên chạy ở 5086, dừng phiên cũ trước khi mở phiên khác.

1. Đăng nhập `company@demo.vn`, mở `http://localhost:5086/Company/Applications`.
2. Chọn một vị trí hoặc trạng thái; mở một hồ sơ ứng viên.
3. Kiểm tra thông tin sinh viên, lời giới thiệu và tải CV.
4. Từ trang quản lý vị trí, chọn Xem ứng viên để mở danh sách theo vị trí đó.

Nếu thiếu CV trên máy chủ, trang chi tiết báo không có file; cần chuyển cả uploads khi chuyển máy. Doanh nghiệp chờ duyệt không được xem ứng viên.

## Trạng thái dự án

Đã hoàn thành: các phần trước và danh sách ứng viên, xem hồ sơ, tải CV Company.

Đang thực hiện: giai đoạn Company; dừng ở mốc xem ứng viên.

Bước tiếp theo: xử lý trạng thái đơn. Trước khi duyệt tạo Internship cần người dùng chốt cách xử lý các đơn còn lại trong cùng đợt; chưa tự quyết định quy tắc này.
