# Company — người hướng dẫn và trạng thái thực tập

Ngày thực hiện: 09/10/2026.

## Kết quả và nghiệp vụ

Từ `/Company/Internships/Details/{id}`, chọn **Cập nhật người hướng dẫn và trạng thái** để mở `/Company/Internships/Edit/{id}`.

- Tên người hướng dẫn bắt buộc, tối đa 150 ký tự. Email tùy chọn, đúng định dạng, tối đa 246 ký tự. Điện thoại tùy chọn, đúng định dạng, tối đa 30 ký tự. Lưu dữ liệu đã bỏ khoảng trắng đầu/cuối.
- Có thể giữ nguyên trạng thái để chỉ sửa người hướng dẫn, kể cả hồ sơ đã kết thúc.
- Chuyển trạng thái đúng mục 16 của PROJECT_REQUIREMENTS: Chưa bắt đầu → Đang thực tập → Hoàn thành hoặc Không hoàn thành. Không bỏ bước, chuyển ngược hoặc đổi kết quả đã kết thúc.
- Mốc này không đổi ngày thực tập, không thêm điều kiện ngày hoặc đánh giá cho việc chuyển trạng thái. Chưa tạo thông báo khi đổi người hướng dẫn/trạng thái.
- Form mang phiên bản từ UpdatedAt/CreatedAt. Nếu hồ sơ vừa được lưu bởi yêu cầu khác, phải tải lại form; không tự thay phiên bản trong form lỗi và ghi đè thay đổi mới.
- POST kiểm tra CSRF, role Company, hoạt động/duyệt doanh nghiệp và quyền sở hữu. Transaction Serializable kiểm tra lại quyền, phiên bản, luồng trạng thái và lưu người hướng dẫn/trạng thái cùng nhau. Xung đột deadlock trả thông báo tải lại.
- Student xem được thông tin mới qua chức năng hiện có. Kết thúc thực tập khóa tạo/sửa nhật ký ở cả giao diện và service Student hiện có.

Không đổi schema, không cần migration.

## File tạo/sửa

- `InternshipPortal/ViewModels/CompanyInternships/EditCompanyInternshipViewModel.cs`: form và kết quả cập nhật.
- `InternshipPortal/Services/CompanyInternshipService.cs`: lấy form, kiểm tra và lưu.
- `InternshipPortal/Areas/Company/Controllers/InternshipsController.cs`: GET/POST Edit.
- `InternshipPortal/Areas/Company/Views/Internships/Edit.cshtml`: form người hướng dẫn và trạng thái được phép.
- `InternshipPortal/Areas/Company/Views/Internships/Details.cshtml`: liên kết cập nhật và thông báo thành công.

## Kiểm tra đã chạy

Build: 0 lỗi, 0 cảnh báo. App khởi động trên localhost:5089, truy vấn/lưu SQL Server local thành công.

Script `.preview/check-company-progress.py` kiểm tra:

- GET/POST bị chặn với Student, Company chờ duyệt, hồ sơ ngoài doanh nghiệp và ID không tồn tại; POST thiếu CSRF trả 400.
- Tên trống/quá dài, email/điện thoại sai, enum sai, thiếu phiên bản bị chặn.
- Chặn bỏ bước và chuyển ngược; bắt đầu thực tập lưu thành công; Student thấy người hướng dẫn và quyền viết nhật ký.
- Form cũ không ghi đè người hướng dẫn mới.
- Hai yêu cầu Hoàn thành/Không hoàn thành đồng thời: chỉ một thành công; kết quả đã kết thúc không thể chuyển lại.
- Kiểm tra riêng cả hai nhánh Hoàn thành và Không hoàn thành; POST Student tạo nhật ký bị chặn và số nhật ký không đổi.
- Có thể sửa người hướng dẫn giữ nguyên trạng thái đã kết thúc.

Hồ sơ demo được dùng tạm để kiểm tra; Status, SupervisorName, SupervisorEmail, SupervisorPhone và UpdatedAt đã được khôi phục và so sánh đúng dữ liệu ban đầu trong finally. Không tạo nhật ký mới hoặc thông báo.

Chạy lại script danh sách/chi tiết/nhật ký Company trên bản mới 5089: đạt. Chưa kiểm tra trực quan trên trình duyệt/điện thoại hoặc tải lớn.

## Chạy và demo

```powershell
dotnet run --project InternshipPortal
```

Nếu cổng mặc định đang được app cũ sử dụng, dừng terminal cũ trước khi chạy lại. Bản kiểm tra đang chạy tại `http://localhost:5089/Company/Internships`.

1. Đăng nhập `company@demo.vn`, mở hồ sơ Chưa bắt đầu và chọn cập nhật.
2. Nhập người hướng dẫn, giữ trạng thái hiện tại để chỉ lưu người hướng dẫn; hoặc chọn Đang thực tập.
3. Đăng nhập Student tương ứng để xem thông tin mới và quyền viết nhật ký.
4. Khi muốn kết thúc, chọn Hoàn thành hoặc Không hoàn thành; thao tác này thay đổi dữ liệu demo thực tế và không thể đổi kết quả từ giao diện.
5. Mở hai form cùng hồ sơ, lưu một form rồi lưu form còn lại: form cũ phải yêu cầu tải lại.

Nếu bị báo hồ sơ vừa thay đổi, chọn **Tải lại thông tin mới nhất** và nhập lại thay đổi dựa trên dữ liệu mới. Không bỏ qua thông báo này bằng cách sửa phiên bản form.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc Student/Company trước đó; gán/cập nhật người hướng dẫn và chuyển trạng thái thực tập.

Đang thực hiện: giai đoạn Company, dừng tại mốc cập nhật quá trình thực tập.

Bước tiếp theo: đánh giá sinh viên phía Company. Chờ người dùng nói “tiếp tục”.
