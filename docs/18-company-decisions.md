# Xử lý ứng viên Company — 09/10/2026

## Kết quả và quy tắc

Trang chi tiết ứng viên có nút xử lý theo trạng thái thực tế:

- Chờ duyệt → Đã xem → Đã phỏng vấn → Đã duyệt, không bỏ qua bước hoặc chuyển ngược.
- Từ chối từ một trong ba trạng thái chưa kết thúc, bắt buộc nhập lý do tối đa 1990 ký tự.
- Đã duyệt/Từ chối/Đã rút/Đã hủy không được xử lý lại.
- Form gửi cả trạng thái đã nhìn thấy; nếu trạng thái vừa đổi, yêu cầu tải lại hồ sơ.
- Mọi thao tác là POST có CSRF, kiểm tra role Company, quyền sở hữu và doanh nghiệp đang hoạt động/đã duyệt tại thời điểm xử lý.

Khi duyệt, transaction Serializable thực hiện cùng nhau:

1. Kiểm tra tài khoản sinh viên đang hoạt động, chưa được tiếp nhận trong cùng đợt và vị trí còn chỉ tiêu.
2. Đổi đơn sang Đã duyệt, tạo Internship Chưa bắt đầu với ngày bắt đầu/kết thúc từ vị trí.
3. Theo xác nhận của người dùng, tự hủy các đơn Chờ duyệt/Đã xem/Đã phỏng vấn còn lại của sinh viên trong cùng đợt, kể cả thuộc doanh nghiệp khác; ghi lý do và giữ lịch sử.
4. Tạo thông báo cho đơn duyệt và từng đơn bị hủy.

Các đơn đã kết thúc và thuộc đợt khác giữ nguyên. Các bước chuyển Đã xem/Đã phỏng vấn/Từ chối cũng tạo thông báo cho sinh viên. Thông báo được lưu trong database; giao diện danh sách thông báo chưa triển khai.

Hạn ứng tuyển và việc đóng vị trí giới hạn gửi đơn mới; đơn đã gửi vẫn được xử lý. Duyệt không yêu cầu hạn ứng tuyển còn tương lai. Vẫn tuân thủ unique Student/Period hiện có, không tạo hồ sơ thứ hai dù hồ sơ cũ đã kết thúc.

## File tạo/sửa

Các đường dẫn tính từ `InternshipPortal`:

- `ViewModels/CompanyApplications/UpdateApplicationViewModel.cs`: trạng thái dự kiến, trạng thái mới, ghi chú.
- `CompanyApplicationDetailsViewModel` thêm dữ liệu form để giữ lý do khi báo lỗi.
- `Services/CompanyApplicationService.cs`: transaction, quy tắc chuyển trạng thái, Internship, tự hủy và thông báo.
- `Areas/Company/Controllers/ApplicationsController.cs`: POST UpdateStatus, reload trạng thái thực tế khi có lỗi.
- `Areas/Company/Views/Applications/Details.cshtml`: nút xử lý và lý do từ chối.
- `wwwroot/js/company-applications.js`: xác nhận duyệt/từ chối.
- `PROJECT_REQUIREMENTS.md` ở thư mục gốc: ghi lại quy tắc hủy đã xác nhận.

Không thay đổi schema, không cần Migration. Giữ UI hiện tại; hoàn thiện UI/UX ở giai đoạn sau theo yêu cầu người dùng.

## Kiểm tra đã thực hiện

- Build thành công, 0 lỗi, 0 cảnh báo; chạy bản kiểm tra trên 5087 với SQL Server local.
- Kiểm tra lại danh sách/hồ sơ/CV và quyền truy cập của giai đoạn trước.
- POST thiếu CSRF trả 400, GET UpdateStatus trả 405; Student/Company chờ duyệt bị chặn, đơn của doanh nghiệp khác trả 404.
- Chuyển đúng thứ tự, chặn bỏ bước/chuyển ngược/form cũ; từ chối không có lý do bị chặn; đơn kết thúc không được xử lý tiếp.
- Duyệt tạo đúng một Internship Chưa bắt đầu, tự hủy đơn cùng đợt của doanh nghiệp khác, giữ đơn khác đợt/đã rút.
- Kiểm tra thông báo đúng loại và khóa chống trùng.
- Cố tình gây lỗi unique khi tạo thông báo: đơn duyệt, hồ sơ mới và hủy đơn khác đều rollback; sau bỏ lỗi fixture, duyệt thành công.
- Hai yêu cầu duyệt đồng thời vào vị trí một chỉ tiêu: chỉ một đơn được duyệt, một hồ sơ được tạo; yêu cầu còn lại bị chặn hoặc nhận thông báo xung đột để thử lại.
- Hai doanh nghiệp đồng thời duyệt cùng sinh viên/cùng đợt: chỉ có một hồ sơ, đơn còn lại bị hủy.
- Chặn duyệt sinh viên đã được tiếp nhận trong cùng đợt.
- Toàn bộ đợt/vị trí/đơn/hồ sơ/thông báo fixture đã được dọn; dữ liệu demo có sẵn giữ nguyên.

Chưa thử tải lớn hoặc toàn bộ thao tác trên trình duyệt thật/điện thoại. SQL deadlock được xử lý bằng thông báo tải lại, không tự lặp thao tác duyệt.

## Demo

App tại `http://localhost:5086`. Nếu cần chạy lại từ `D:\Portal\code`:

```powershell
dotnet run --project InternshipPortal
```

1. Đăng nhập Company, mở danh sách ứng viên và một đơn Chờ duyệt.
2. Đánh dấu Đã xem, sau đó Đã phỏng vấn.
3. Duyệt: xác nhận, kiểm tra trạng thái mới; đăng nhập Student để xem hồ sơ Chưa bắt đầu và các đơn cùng đợt đã hủy.
4. Với đơn khác chưa kết thúc, thử từ chối thiếu lý do rồi nhập lý do hợp lệ.

Các thao tác demo này thay đổi dữ liệu thật trên máy phát triển. Nếu sinh viên đã có hồ sơ cùng đợt hoặc vị trí đủ chỉ tiêu, hệ thống sẽ báo lý do và không duyệt thêm. Khi có thao tác đồng thời, tải lại hồ sơ trước khi thử lại.

## Trạng thái dự án

Đã hoàn thành: Student, hồ sơ/vị trí/ứng viên Company và xử lý trạng thái đơn, duyệt tạo Internship, tự hủy đơn cùng đợt.

Đang thực hiện: giai đoạn Company; dừng tại mốc xử lý ứng viên.

Bước tiếp theo: danh sách sinh viên thực tập tại doanh nghiệp, gán người hướng dẫn và cập nhật quá trình thực tập; sau đó đánh giá sinh viên. GPA và UI/UX giữ lại cho phần liên quan về sau.
