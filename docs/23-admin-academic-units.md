# Admin quản lý khoa/ngành — 09/10/2026

Đã thêm `/Admin/AcademicUnits` (khoa), `/Admin/AcademicUnits?major=true` (ngành) và form tạo/sửa. Dashboard Admin có liên kết. Mã bắt buộc tối đa 20 ký tự, tên bắt buộc tối đa 150 ký tự; mã bỏ khoảng trắng đầu/cuối và chuyển chữ hoa. Mã duy nhất trong từng bảng theo database hiện có. Ngành bắt buộc liên kết khoa tồn tại.

Có thể bật/tắt IsActive; không xóa cứng hoặc tự đổi trạng thái bản ghi liên quan. Danh sách khoa trên form ngành gồm cả khoa ngừng hoạt động, ghi rõ trạng thái, để quản trị sửa dữ liệu hiện có. Mốc này không thay quy tắc chọn khoa/ngành trong chức năng Student/Company.

Role Admin và ActiveAccountFilter bảo vệ action; POST có CSRF. Transaction Serializable, kiểm tra phiên bản UpdatedAt/CreatedAt và unique database chống ghi đè form cũ/mã trùng. Không cần migration. Danh sách hiện hiển thị toàn bộ, chưa có tìm kiếm/phân trang.

File tạo: ViewModels/AcademicUnitViewModel.cs, Services/AcademicUnitService.cs, Areas/Admin/Controllers/AcademicUnitsController.cs, Areas/Admin/Views/AcademicUnits/{Index,Edit}.cshtml trong InternshipPortal. Program.cs đăng ký service; Dashboard/Index thêm điều hướng.

Build: 0 lỗi, 0 cảnh báo. App/SQL Server chạy thành công trên 5092. Script `.preview/check-academic-units.py` đạt: role GET, CSRF POST, ID không tồn tại, mã/tên trống hoặc quá dài, tạo/sửa khoa/ngành, mã trùng, form cũ, khoa không tồn tại, liên kết khoa và trạng thái ngừng hoạt động. Dữ liệu thử riêng đã xóa, không sửa dữ liệu demo. Chưa kiểm tra trình duyệt thật/mobile hoặc tải lớn.

Bản mới nhất được chạy lại trên cổng mặc định 5086. Đăng nhập `admin@demo.edu.vn`, vào `/Admin/AcademicUnits`; thử thêm khoa, thêm ngành thuộc khoa, sửa tên hoặc bỏ chọn Đang hoạt động. Lưu trùng mã phải báo lỗi; mở hai form rồi lưu lần lượt phải chặn form cũ. Muốn tải lại thông tin, dùng liên kết trong form.

Chạy lại từ thư mục gốc:

```powershell
dotnet run --project InternshipPortal
```

Dừng app đang chiếm cổng 5086 trước khi chạy lại.

TRẠNG THÁI DỰ ÁN: đã hoàn thành mốc Admin khoa/ngành; đang ở giai đoạn Admin. Bước tiếp theo là quản lý đợt thực tập. Sửa điểm đánh giá đã gửi vẫn chưa triển khai. Dừng và chờ người dùng nói “tiếp tục”.
