# Chỉnh giao diện trang chủ

Theo yêu cầu người dùng, đổi giao diện sang phong cách biên tập: nền kem, chữ than, đỏ gạch và đồ họa bậc thang. Tiêu đề lớn kết hợp chữ serif; các vai trò chia bằng đường kẻ và số thứ tự thay cho thẻ bo tròn.

## File sửa

- InternshipPortal/Views/Home/Index.cshtml: bố cục trang bìa, SVG bậc thang, các vai trò và hành trình.
- InternshipPortal/Views/Shared/_Layout.cshtml: menu, logo chữ, footer và liên kết bỏ qua điều hướng.
- InternshipPortal/wwwroot/css/site.css: màu sắc, typography và responsive; CSS viết thành dòng dễ chỉnh.
- .gitignore: bỏ qua thư mục .preview chứa ảnh kiểm tra và hồ sơ trình duyệt.

Giữ ASP.NET Core MVC, Razor và Bootstrap 5. Giữ trang quyền riêng tư và các liên kết tới mục nội dung. Chưa thêm chức năng đăng nhập, ứng tuyển hoặc dữ liệu giả.

## Kiểm tra đã thực hiện

- Build thành công: 0 warning, 0 error.
- Chạy tại http://localhost:5086; trang chủ trả HTTP 200.
- Kiểm tra HTML có nội dung mới, SVG và các mục portals/journey.
- Chụp và xem bố cục bằng Edge headless ở 1440 px và 600 px. 600 px sử dụng bố cục một cột với menu thu gọn.
- Ảnh chụp yêu cầu cửa sổ 390 px bị cắt do giới hạn cửa sổ của trình duyệt headless; chưa xác minh chính xác viewport 390 px hoặc thao tác mở menu tự động.

Người dùng nhấn Ctrl+F5 tại localhost:5086 để xem bản mới; kiểm tra thêm trên điện thoại thực tế.

## Trạng thái

Hoàn thành chỉnh giao diện và kiểm tra build/HTTP, đã xem ảnh desktop và bố cục một cột. Chờ phản hồi giao diện. Giai đoạn database chưa bắt đầu.
