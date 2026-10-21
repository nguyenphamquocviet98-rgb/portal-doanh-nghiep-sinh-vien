# Giai đoạn 7.3 — Gửi đơn, lịch sử và rút đơn

## Mục tiêu và kết quả

Sinh viên gửi đơn bằng CV hiện tại, xem lịch sử/trạng thái, tải CV trong từng đơn và rút đơn khi còn Pending. Kiểm tra role và quyền sở hữu ở server. Chưa xây phần doanh nghiệp duyệt đơn/tạo hồ sơ thực tập.

## File tạo/sửa

| File trong InternshipPortal | Chức năng |
|---|---|
| Services/ApplicationService.cs | Điều kiện gửi, transaction, lịch sử, quyền sở hữu, rút đơn và CV |
| Areas/Student/Controllers/ApplicationsController.cs | Index, Create GET/POST, Details, Withdraw POST, DownloadCv |
| ViewModels/Applications/*.cs | Form, danh sách và chi tiết đơn |
| Areas/Student/Views/Applications/Create.cshtml | Form với trạng thái đủ điều kiện và CV |
| Areas/Student/Views/Applications/Index.cshtml | Lịch sử, lọc trạng thái và phân trang |
| Areas/Student/Views/Applications/Details.cshtml | Đơn đã gửi, ghi chú, tải CV và xác nhận rút |
| Views/Internships/Details.cshtml | Nút ứng tuyển cho Student khi còn chỉ tiêu |
| Areas/Student/Views/Dashboard/Index.cshtml | Liên kết lịch sử |
| Views/Shared/_Layout.cshtml | Menu Ứng tuyển cho Student |
| Program.cs | Đăng ký ApplicationService |
| wwwroot/css/site.css | Giao diện trạng thái, form và lịch sử |
| README.md | Cách demo |

Không thêm bảng, cột hoặc Migration. Schema hiện tại đã có unique sinh viên/vị trí và sinh viên/đợt.

## Quy tắc thực hiện

- Action dùng Authorize Student; POST dùng AntiForgeryToken.
- StudentId/UserId/Status/CvPath do server xác định, không nhận từ form.
- Chỉ nhận đơn cho vị trí Open, chưa hết hạn, công ty Approved, công ty/tài khoản đang hoạt động.
- Một sinh viên một lần ứng tuyển mỗi vị trí, kể cả sau rút hoặc từ chối.
- Sinh viên đã được tiếp nhận trong đợt không gửi thêm, kể cả hồ sơ đã kết thúc. Kiểm tra cả Internship và đơn Approved để bảo vệ dữ liệu đã tiếp nhận.
- Chỉ tiêu tính theo số đơn Approved, không theo số đơn Pending.
- Phải có CV đang tồn tại trong thư mục riêng tư. CV chỉ lưu đường dẫn tại thời điểm gửi, giữ file cũ khi thay CV hồ sơ.
- CoverLetter tùy chọn, tối đa 1990 ký tự.
- Submit kiểm tra lại mọi điều kiện trong Serializable transaction và dùng unique index làm lớp bảo vệ cuối.
- Gửi đồng thời có thể gây tranh chấp SQL; lỗi deadlock được nhận diện qua các lớp exception EF, trả thông báo kiểm tra lịch sử/thử lại. Không tự bật retry cho toàn bộ transaction.
- Pending mới được Withdrawn. UPDATE có điều kiện kiểm tra lại Pending và chủ sở hữu ngay tại database, không chỉ dựa vào trạng thái trên trang.
- Rút giữ lịch sử, không xóa đơn, không cho gửi lại. Đổi trạng thái và tạo Notification trong cùng transaction.
- Rút đồng thời không tạo hai thông báo; có khóa chống trùng.
- Danh sách/chi tiết/download CV chỉ truy vấn đơn của user đang đăng nhập.
- Download dùng attachment PDF, private/no-store và nosniff; không phục vụ đường dẫn uploads trực tiếp.
- Lịch sử 10 đơn/trang, sắp mới nhất rồi Id giảm dần; giữ bộ lọc khi chuyển trang.

Quy tắc trạng thái đợt và bắt buộc ngành sinh viên trùng ngành vị trí chưa được chốt, nên không tự thêm điều kiện mới vào gửi đơn.

## Cách chạy và demo

App đang chạy tại http://localhost:5086. Nếu cần chạy lại, dừng terminal bằng Ctrl+C rồi từ thư mục gốc:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Để thử gửi mới, đăng nhập student09@demo.edu.vn bằng mật khẩu demo trong User Secrets. Sinh viên này có CV và chưa được tiếp nhận trong đợt. student@demo.edu.vn đã được duyệt/tạo hồ sơ thực tập, nên không gửi thêm trong cùng đợt.

1. Mở /Internships, chọn một vị trí chưa từng ứng tuyển.
2. Chọn Ứng tuyển vị trí này.
3. Kiểm tra CV, nhập lời giới thiệu, gửi.
4. Xem chi tiết hoặc menu Ứng tuyển tại /Student/Applications.
5. Đơn Pending: tích xác nhận rồi chọn Rút đơn ứng tuyển.
6. Xem đơn Withdrawn trong lịch sử; không thể gửi lại vị trí đó.

Các URL:

- /Student/Applications
- /Student/Applications?Status=0
- /Student/Applications/Create?positionId=1
- /Student/Applications/Details/{id}
- /Student/Applications/DownloadCv/{id}

## Checklist kiểm thử đã chạy

- Build cuối 0 warning, 0 error; model không thay đổi cần Migration.
- Chặn chưa đăng nhập/sai role.
- Chặn gửi khi không có CV; CoverLetter dài quá 1990; POST không có token.
- Gửi thành công tạo Pending, đúng chủ sở hữu và CV server dù form giả StudentId/UserId/Status/CvPath.
- Đơn trùng không tạo thêm; hai request đồng thời chỉ lưu một đơn. Đã sửa trường hợp EF bọc lỗi deadlock gây HTTP 500, chạy lại đạt một success và một thông báo.
- Đổi CV hồ sơ sau gửi: tải CV của đơn vẫn đúng nội dung cũ.
- Sinh viên khác không xem/tải/rút được đơn, đều trả 404.
- GET Withdraw không đổi dữ liệu; POST thiếu token bị chặn.
- Rút đồng thời từ Pending tạo một trạng thái Withdrawn và một Notification; đơn Viewed không rút được.
- Sau rút, POST gửi lại cùng vị trí bị chặn.
- Chặn vị trí đóng sau khi form đã mở, hết hạn, công ty chưa duyệt và đủ chỉ tiêu.
- Chặn sinh viên đã có hồ sơ Completed trong cùng đợt.
- Lịch sử, lọc trạng thái và 12 bản ghi qua hai trang không lặp/mất đơn.
- Ghi chú Rejected hiển thị đúng.
- Đã xem ảnh desktop của form gửi và lịch sử; chưa kiểm tra trực quan bằng điện thoại thực tế.
- Dữ liệu/file kiểm thử tạm đã dọn; giữ 20 sinh viên, 18 vị trí, 21 đơn, 8 hồ sơ, 6 thông báo demo.
- 7 kiểm tra ràng buộc SQL vẫn đạt.

## Lỗi thường gặp

- Form báo đã tiếp nhận: đúng quy tắc một lần trong một đợt; thử Student chưa tiếp nhận hoặc đăng ký tài khoản mới.
- Form báo đã ứng tuyển: kiểm tra lịch sử, kể cả đơn đã rút.
- CV có đường dẫn nhưng mất file trên máy chủ: upload lại CV trước khi gửi.
- Gửi hai lần: chỉ một đơn được lưu; request còn lại có thể báo tranh chấp, kiểm tra lịch sử trước khi thử lại.
- Đơn không rút được: có thể doanh nghiệp đã xem hoặc trạng thái vừa đổi.
- SQL log có deadlock khi kiểm thử đồng thời: exception được xử lý thành thông báo, không tự xóa database hoặc Migration.

## Git checkpoint

Chưa commit thay đổi mới. Từ thư mục gốc, git status để xem file, git diff để xem nội dung. Sau khi kiểm tra dùng git add InternshipPortal docs PROJECT_REQUIREMENTS.md, rồi commit message đề xuất feat: add student applications and withdrawal. Không tự push. Git không backup database/file uploads.

## Trạng thái dự án

Đã hoàn thành: giai đoạn 7.3 — gửi đơn, lịch sử, chi tiết/CV và rút đơn.

Đang thực hiện: giai đoạn Student chưa hoàn tất; GPA vẫn chờ xác nhận.

Bước tiếp theo: Student theo dõi hồ sơ thực tập, nhật ký và tài liệu; doanh nghiệp xử lý đơn theo phần Company/nghiệp vụ chung của lộ trình.
