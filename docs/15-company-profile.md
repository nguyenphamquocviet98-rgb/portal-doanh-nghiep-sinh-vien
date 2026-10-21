# Hồ sơ doanh nghiệp — 09/10/2026

Phần đầu giai đoạn Company: doanh nghiệp xem/sửa hồ sơ của mình tại `/Company/Profile`, form tại `/Company/Profile/Edit`.

## File chính

- `InternshipPortal/ViewModels/CompanyProfileViewModel.cs`: trường nhập và giới hạn khớp database.
- `InternshipPortal/Services/CompanyProfileService.cs`: lấy hồ sơ theo tài khoản đăng nhập; chỉ lưu các trường cho phép.
- `InternshipPortal/Areas/Company/Controllers/ProfileController.cs`: role Company, kiểm tra hồ sơ hoạt động và CSRF cho POST.
- `InternshipPortal/Areas/Company/Views/Profile/Index.cshtml`, `Edit.cshtml`: giao diện xem/sửa.
- `Program.cs`: đăng ký service; trang Dashboard Company có liên kết tới hồ sơ.

Không thay đổi database, không cần Migration. Doanh nghiệp chờ duyệt vẫn sửa được thông tin hồ sơ. Việc sửa không tự đổi trạng thái duyệt, ghi chú Admin hoặc chủ sở hữu.

## Chạy và demo

Nếu đang mở bản cũ, Ctrl+C ở terminal đó rồi chạy `dotnet run --project InternshipPortal` từ `D:\Portal\code`.

1. Đăng nhập `company@demo.vn`, mở cổng doanh nghiệp rồi chọn cập nhật hồ sơ.
2. Sửa giới thiệu, lưu và mở lại để kiểm tra.
3. Thử bỏ trống tên/địa chỉ hoặc nhập website không có http/https: không được lưu.
4. Đăng nhập `company06@demo.vn`: vẫn xem/sửa được, giữ trạng thái Chờ duyệt.
5. Đăng nhập Student: không truy cập được `/Company/Profile`.

Nếu website có giá trị không hợp lệ từ trước, hãy sửa thành URL http/https hoặc để trống. Nếu cổng 5086 bị chiếm, dừng phiên đang chạy trước khi mở lại.

## Kiểm tra đã thực hiện

- Build thành công, 0 lỗi, 0 cảnh báo; chạy bản kiểm tra cổng 5087 với SQL Server hiện tại.
- GET hồ sơ/form sau đăng nhập hoạt động, lưu tiếng Việt và đọc lại đúng.
- Website javascript và tên chỉ chứa khoảng trắng bị từ chối; POST thiếu CSRF trả 400.
- Gửi thêm UserId/ApprovalStatus/IsActive không thay đổi quyền sở hữu hoặc trạng thái duyệt.
- Company chờ duyệt truy cập được; người chưa đăng nhập chuyển tới login, Student bị từ chối 403.
- Đã khôi phục nội dung hồ sơ demo sau kiểm tra; UpdatedAt thay đổi do thao tác lưu.
- Chưa kiểm tra hình ảnh giao diện trên trình duyệt/điện thoại thực tế.

## Trạng thái dự án

Đã hoàn thành: phần Student trước đó và xem/sửa hồ sơ Company trong phạm vi trên.

Đang thực hiện: giai đoạn Company, dừng tại mốc hồ sơ để người dùng kiểm tra.

Bước tiếp theo: quản lý vị trí thực tập của doanh nghiệp (tạo/sửa/đóng, kiểm tra trạng thái duyệt và quyền sở hữu). GPA và quy tắc xử lý các đơn còn lại khi duyệt vẫn cần chốt trước phần liên quan.
