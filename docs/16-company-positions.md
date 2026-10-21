# Quản lý vị trí Company — 09/10/2026

## Kết quả

Company có danh sách riêng tại `/Company/Positions`, tìm theo tên/mã, lọc trạng thái và phân trang 10 vị trí. Doanh nghiệp đã duyệt, hồ sơ và tài khoản đang hoạt động mới được tạo/sửa/đóng vị trí. Doanh nghiệp chờ duyệt có thể xem danh sách nhưng không được thao tác.

- Đăng mới tại `/Company/Positions/Create`: mã tự sinh, trạng thái Đang tuyển, gán doanh nghiệp từ tài khoản đăng nhập.
- Sửa tại `/Company/Positions/Edit/{id}`: dữ liệu và quyền sở hữu được kiểm tra lại khi lưu.
- Khi đã có bất kỳ đơn ứng tuyển nào, ngành và đợt thực tập bị khóa theo xác nhận của người dùng. Nếu có đơn gửi trong lúc form đang mở, kiểm tra khi lưu vẫn chặn thay đổi.
- Chỉ tiêu không được nhỏ hơn số đơn Đã duyệt. Kiểm tra/lưu trong transaction Serializable tương thích với luồng gửi đơn.
- Chọn ngành đang hoạt động và đợt chưa đóng cho vị trí mới hoặc khi đổi các trường này. Khi giữ nguyên, các giá trị cũ vẫn hiển thị dù đã ngừng hoạt động/đóng đợt.
- Hạn mới phải ở tương lai; nhập giờ Việt Nam, lưu UTC. Hạn cũ đã hết có thể giữ nguyên khi sửa nội dung. Ngày kết thúc không trước ngày bắt đầu.
- Đóng qua POST có CSRF và xác nhận trên giao diện. Không xóa đơn đã gửi; trang công khai ngừng hiển thị vị trí. Sửa vị trí đã đóng không tự mở lại.

Không thay đổi schema hoặc Migration. Thông báo về vị trí mới cho sinh viên sẽ làm trong phần thông báo; phần này chưa triển khai gửi thông báo.

## File tạo/sửa

- `ViewModels/CompanyPositions/PositionFormViewModel.cs`: form, danh sách và giới hạn đầu vào.
- `Services/CompanyPositionService.cs`: truy vấn theo chủ sở hữu, nghiệp vụ tạo/sửa/đóng.
- `Areas/Company/Controllers/PositionsController.cs`: các action với role Company, CSRF và xử lý kết quả.
- `Areas/Company/Views/Positions/Index.cshtml`, `Form.cshtml`: danh sách và form chung.
- `wwwroot/js/company-positions.js`: xác nhận đóng vị trí.
- `Program.cs`: đăng ký service; Dashboard Company có liên kết quản lý vị trí.
- `PROJECT_REQUIREMENTS.md`: ghi lại quy tắc khóa ngành/đợt đã xác nhận.

Các đường dẫn code trên tính từ `InternshipPortal`, trừ tài liệu yêu cầu ở thư mục gốc.

## Kiểm tra đã thực hiện

- Build: 0 lỗi, 0 cảnh báo. Khởi động bản kiểm tra cổng 5087, dùng database SQL Server hiện tại.
- HTTP qua phiên đăng nhập: Company tạo/sửa được, người chưa đăng nhập chuyển về login, Student bị từ chối 403, Company chờ duyệt bị chặn GET/POST tạo.
- Đầu vào trống, chỉ tiêu 0, enum sai, ngày đảo ngược, hạn quá khứ, ngành/đợt không tồn tại: không lưu.
- POST thiếu CSRF trả 400; GET đóng trả 405.
- Gửi giả CompanyId/Code/Status không chiếm dữ liệu hoặc đổi trạng thái.
- GET/POST sửa và POST đóng vị trí của doanh nghiệp khác trả 404.
- Đăng mới xuất hiện trong chi tiết công khai; hạn Việt Nam được đổi sang UTC đúng.
- Có đơn ứng tuyển: không đổi ngành/đợt, không giảm chỉ tiêu dưới số đã duyệt.
- Đóng: trang công khai trả 404, các đơn giữ nguyên; sửa vị trí đã đóng vẫn đóng.
- Fixture vị trí và đơn thử nghiệm đã được dọn. Không sửa dữ liệu nghiệp vụ demo sẵn có.
- Chưa kiểm tra hình ảnh trên điện thoại/trình duyệt thật, chưa thử tải đồng thời nhiều người dùng.

## Chạy và demo

Nếu đang mở bản cũ, Ctrl+C tại terminal đó, từ `D:\Portal\code` chạy:

```powershell
dotnet run --project InternshipPortal
```

1. Đăng nhập `company@demo.vn`, mở `http://localhost:5086/Company/Positions` hoặc chọn liên kết trong cổng doanh nghiệp.
2. Đăng vị trí mới với hạn tương lai. Mở `/Internships` và tìm theo tên vừa nhập.
3. Quay lại Company, sửa nội dung; kiểm tra lưu được.
4. Mở vị trí demo đã có đơn: ngành và đợt hiển thị khóa.
5. Đóng vị trí vừa tạo, xác nhận, kiểm tra không còn ở danh sách công khai.
6. Thử tài khoản `company06@demo.vn`: có thông báo chờ duyệt và không có nút đăng mới.

Lỗi thường gặp: hạn nhập phải theo giờ Việt Nam và nằm trong tương lai; ngành phải đang hoạt động; đợt mới chọn không được đóng; chỉ tiêu phải đủ cho số đã duyệt. Nếu cổng bị chiếm, dừng phiên cũ trước khi chạy lại.

## Trạng thái dự án

Đã hoàn thành: Student, hồ sơ Company và quản lý vị trí Company trong phạm vi kiểm tra trên.

Đang thực hiện: giai đoạn Company; dừng tại mốc quản lý vị trí để người dùng kiểm tra.

Bước tiếp theo: quản lý ứng viên và CV. Trước phần duyệt tạo Internship cần chốt cách xử lý các đơn còn lại của sinh viên trong cùng đợt. GPA chưa thay đổi.
