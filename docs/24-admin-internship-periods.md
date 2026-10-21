# Admin quản lý đợt thực tập — 09/10/2026

Tiếp nối mốc 23 khoa/ngành theo lộ trình Admin trong PROJECT_REQUIREMENTS.md.

## Kết quả

`/Admin/Periods`: danh sách đợt, liên kết tạo mới và chỉnh sửa. `/Admin/Periods/Edit/{id}`: mã (30 ký tự), tên (150 ký tự), ngày bắt đầu/kết thúc và enum Chưa mở/Đang mở/Đã đóng. Dashboard Admin có liên kết.

Mã/tên/ngày/trạng thái bắt buộc. Ngày kết thúc không sớm hơn ngày bắt đầu, ngày từ năm 1900. Mã bỏ khoảng trắng đầu/cuối và chuyển chữ hoa; unique database chống trùng. Ngày nhập theo Việt Nam được lưu UTC tại 00:00 Việt Nam, mở form chuyển lại đúng ngày. Không xóa đợt; chỉnh trạng thái giữ các vị trí/hồ sơ liên quan. Mốc này cho Admin chọn lại trạng thái, không tự đổi trạng thái theo thời gian.

Đóng đợt loại nó khỏi lựa chọn khi tạo vị trí mới theo CompanyPositionService hiện có. Không tự đóng vị trí, hủy đơn hoặc thay ngày thực tập trong hồ sơ đã tạo. Sửa ngày đợt chỉ sửa đợt, không đồng bộ ngày sang các bản ghi khác.

Role Admin, ActiveAccountFilter và CSRF bảo vệ thao tác. Transaction Serializable và phiên bản UpdatedAt/CreatedAt chặn form cũ; xung đột unique/deadlock trả thông báo tải lại. Không cần migration.

## File tạo/sửa

- `InternshipPortal/ViewModels/PeriodFormViewModel.cs`
- `InternshipPortal/Areas/Admin/Controllers/PeriodsController.cs`
- `InternshipPortal/Areas/Admin/Views/Periods/Index.cshtml`
- `InternshipPortal/Areas/Admin/Views/Periods/Edit.cshtml`
- `InternshipPortal/Areas/Admin/Views/Dashboard/Index.cshtml`: điều hướng.

## Kiểm tra đã chạy

Build 0 lỗi, 0 cảnh báo. App/SQL Server hoạt động, bản mới chạy trên localhost:5086.

`.preview/check-periods.py` đạt: GET/POST chặn Company/Student; khách chuyển đăng nhập; CSRF; ID không tồn tại; mã/tên trống/quá dài; thiếu ngày; ngày ngược; enum sai; tạo và sửa đợt; mã trùng; form cũ; chuyển trạng thái; ngày lưu/đọc lại đúng; đợt đóng không xuất hiện trong form Company tạo vị trí. Đợt thử riêng đã xóa, không đổi dữ liệu demo.

Chưa kiểm tra trực quan trình duyệt/điện thoại hoặc tải lớn. Danh sách chưa có tìm kiếm/phân trang.

## Chạy và demo

```powershell
dotnet run --project InternshipPortal
```

Dừng app đang dùng cổng 5086 trước khi chạy lại. Bản mới hiện tại: `http://localhost:5086/Admin/Periods`. Đăng nhập `admin@demo.edu.vn`.

1. Chọn Thêm đợt, nhập mã/tên và khoảng ngày hợp lệ, chọn Chưa mở rồi lưu.
2. Sửa đợt sang Đang mở hoặc Đã đóng.
3. Thử mã đã có hoặc ngày kết thúc sớm hơn bắt đầu: phải báo lỗi.
4. Mở hai form sửa cùng đợt, lưu một form rồi lưu form cũ: phải yêu cầu tải lại.

Nếu bị báo xung đột, tải lại thông tin mới nhất trước khi nhập lại.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc trước, Admin khoa/ngành và quản lý đợt thực tập.

Đang thực hiện: giai đoạn Admin, dừng ở mốc đợt thực tập.

Bước tiếp theo: quản lý tài khoản/sinh viên phía Admin. Chức năng sửa điểm đã gửi vẫn chưa triển khai. Chờ người dùng nói “tiếp tục”.
