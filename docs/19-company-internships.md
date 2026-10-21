# Company — danh sách thực tập và xem nhật ký

Ngày thực hiện: 09/10/2026.

## Kết quả

- `/Company/Internships`: danh sách hồ sơ thuộc doanh nghiệp, gồm mọi trạng thái; tìm theo tên/mã sinh viên, lọc trạng thái và phân trang 10 hồ sơ.
- `/Company/Internships/Details/{id}`: sinh viên, vị trí, đợt, thời gian, trạng thái, thông tin người hướng dẫn hiện có và nhật ký đã nộp.
- `/Company/Internships/Diary/{id}`: đọc nội dung nhật ký, không có nút sửa. Dùng lại view nhật ký Student với CanEdit mặc định false.
- Từ danh sách ứng viên có liên kết đến sinh viên thực tập; từ hồ sơ thực tập có liên kết về đúng đơn ứng tuyển/CV.

Mỗi action kiểm tra role Company và doanh nghiệp đang hoạt động/đã duyệt. Mỗi truy vấn hồ sơ/nhật ký tiếp tục kiểm tra chủ sở hữu và điều kiện hoạt động/duyệt. Hồ sơ của doanh nghiệp khác hoặc không tồn tại trả 404. Trang riêng tư đặt no-store.

Không sửa schema hoặc dữ liệu, không cần migration. Chưa thêm thao tác gán người hướng dẫn hay chuyển trạng thái ở mốc này.

## File

- `InternshipPortal/ViewModels/CompanyInternships/CompanyInternshipViewModels.cs`
- `InternshipPortal/Services/CompanyInternshipService.cs`
- `InternshipPortal/Areas/Company/Controllers/InternshipsController.cs`
- `InternshipPortal/Areas/Company/Views/Internships/Index.cshtml`
- `InternshipPortal/Areas/Company/Views/Internships/Details.cshtml`
- `InternshipPortal/Program.cs`: đăng ký service.
- `InternshipPortal/Areas/Company/Views/Applications/Index.cshtml`: liên kết điều hướng.

## Kiểm tra thực tế

Build thành công, 0 lỗi, 0 cảnh báo. Khởi động bản kiểm tra tại localhost:5088, kết nối SQL Server local thành công.

Script HTTP `.preview/check-company-internships.py` đã kiểm tra đăng nhập, role, Company chờ duyệt, phân tách danh sách giữa doanh nghiệp, hồ sơ/nhật ký ngoài quyền và ID không tồn tại, đọc nhật ký của doanh nghiệp, no-store, tìm mã sinh viên, danh sách rỗng, enum không hợp lệ, giới hạn số trang và trang ứng viên cũ. Tất cả đạt sau khi sửa thông báo trạng thái không hợp lệ sang tiếng Việt.

Chưa kiểm tra bố cục trên trình duyệt thật/điện thoại và phân trang trên tập dữ liệu lớn hơn 10 hồ sơ. Không tạo hoặc thay đổi dữ liệu demo khi kiểm tra.

## Chạy và demo

```powershell
dotnet run --project InternshipPortal
```

Nếu app đang chạy ở cổng mặc định, dừng terminal cũ trước khi chạy lại. Bản kiểm tra của mốc này đang ở `http://localhost:5088/Company/Internships`.

1. Đăng nhập `company@demo.vn`, mở danh sách thực tập.
2. Tìm `DEMO006`, mở chi tiết và nhật ký tuần đã nộp.
3. Bỏ bộ lọc, mở hồ sơ chưa bắt đầu để xem trường hợp chưa có nhật ký.
4. Thử ID hồ sơ/nhật ký doanh nghiệp khác: phải trả 404.

Nếu không thấy sinh viên, bỏ bộ lọc và kiểm tra đã có đơn được duyệt tạo Internship. Nếu bị chặn, kiểm tra role và trạng thái doanh nghiệp/tài khoản.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc Student và Company trước đó; danh sách, chi tiết thực tập và xem nhật ký phía Company.

Đang thực hiện: giai đoạn Company, dừng tại mốc đọc hồ sơ thực tập.

Bước tiếp theo: gán người hướng dẫn và cập nhật trạng thái thực tập; sau đó đánh giá sinh viên. Chờ người dùng nói “tiếp tục”.
