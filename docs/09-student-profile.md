# Giai đoạn 7.1 — Hồ sơ sinh viên và CV

## Mục tiêu và phạm vi

Cho Student xem/sửa hồ sơ của chính mình, tải lên và tải xuống CV. Giữ giao diện đã chọn. CV đã được người dùng chốt là PDF tối đa 10 MB.

GPA còn chờ làm rõ câu trả lời “tích hợp”: hỗ trợ hai thang 4/10 hay một thang. Chưa thêm cột hoặc quy tắc GPA; không sửa giá trị GPA hiện có. Đây là phần đầu của giai đoạn Student, chưa hoàn thành toàn bộ giai đoạn 7.

## File tạo/sửa

| File trong InternshipPortal | Chức năng |
|---|---|
| Areas/Student/Controllers/ProfileController.cs | Xem, sửa, upload, download; Authorize Student và AntiForgeryToken trên POST |
| Services/StudentProfileService.cs | Lấy hồ sơ theo user đăng nhập; cập nhật hồ sơ/tài khoản trong transaction; thay CV |
| Services/PdfUploadService.cs | Kiểm tra PDF, 10 MB, tên GUID, lưu file và kiểm tra đường dẫn riêng tư |
| ViewModels/StudentProfileViewModel.cs | Thông tin trang hồ sơ |
| ViewModels/EditStudentProfileViewModel.cs | Form chỉnh sửa với validation tiếng Việt |
| Areas/Student/Views/Profile/Index.cshtml | Thông tin và form CV |
| Areas/Student/Views/Profile/Edit.cshtml | Form thông tin cá nhân, khoa/ngành, kỹ năng và giới thiệu |
| Areas/Student/Views/Dashboard/Index.cshtml | Liên kết tới hồ sơ |
| Views/Shared/_Layout.cshtml | Menu Hồ sơ cho Student |
| Program.cs | Đăng ký Service |
| wwwroot/css/site.css | Bố cục hồ sơ, CV, textarea và responsive |

Không sửa Entity/DbContext/Migration ở phần đã làm; model vẫn khớp Migration trước.

## Hành vi

- UserId lấy từ phiên đăng nhập, không lấy từ form/query string.
- Sinh viên sửa họ tên, mã sinh viên, lớp, điện thoại, khoa/ngành, kỹ năng và giới thiệu.
- Email tài khoản hiển thị chỉ đọc; form không thể thay email, role, UserId hoặc CvPath.
- MSSV unique, khoa/ngành đang hoạt động và khớp nhau.
- Độ dài form khớp schema: họ tên 150, mã 30, lớp 50, kỹ năng 990, giới thiệu 1990.
- CV kiểm tra phần mở rộng PDF, dung lượng, header PDF và dấu kết thúc EOF. Đây là kiểm tra định dạng cơ bản, chưa phân tích toàn bộ cấu trúc PDF hoặc quét mã độc.
- Tên lưu file là GUID; không dùng tên file gửi từ trình duyệt.
- File mới lưu dưới wwwroot/uploads/cv; database chỉ lưu đường dẫn.
- Không xóa CV cũ khi thay vì đơn đã gửi có thể đang tham chiếu bản đó.
- Nếu file không hợp lệ hoặc lưu database thất bại, file mới được dọn và CV cũ giữ nguyên.
- Download chỉ lấy CV hiện tại của user đăng nhập; attachment PDF, nosniff và private/no-store.
- /uploads trực tiếp vẫn trả 404. Người khác không thể truyền đường dẫn để tải file.

## Cách chạy và sử dụng

App đang chạy tại http://localhost:5086. Đăng nhập Student, chọn “Hồ sơ” trên menu hoặc nút “Hồ sơ & CV của tôi” trong cổng.

Các URL:

- http://localhost:5086/Student/Profile
- http://localhost:5086/Student/Profile/Edit
- http://localhost:5086/Student/Profile/DownloadCv

Nếu cần khởi động lại, dừng terminal chạy app bằng Ctrl+C rồi từ thư mục gốc:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Mật khẩu demo vẫn lấy từ User Secrets theo tài liệu giai đoạn 6.

## Checklist người dùng

1. Hồ sơ hiển thị đúng sinh viên đăng nhập.
2. Sửa thông tin và lưu: trở về hồ sơ, thấy thông báo thành công và dữ liệu mới.
3. Chọn khoa khác: ngành được lọc theo khoa.
4. MSSV trùng hoặc nội dung quá dài: thấy lỗi, không ghi dữ liệu sai.
5. Upload PDF đúng dưới 10 MB: lưu thành công, tải được đúng file.
6. Upload file khác PDF hoặc quá 10 MB: báo lỗi, giữ CV trước.
7. Company/Admin mở trang Student/Profile: không được truy cập.

## Kiểm tra đã chạy trực tiếp

- Build: 0 warning, 0 error.
- Model không có thay đổi cần Migration.
- Chặn truy cập profile chưa đăng nhập/sai role.
- Lưu hồ sơ thành công; giả UserId, StudentId, Email, Role, CvPath từ form không thay được dữ liệu được bảo vệ hoặc hồ sơ khác.
- POST Edit/Upload thiếu anti-forgery token bị chặn.
- MSSV trùng, khoa/ngành không khớp, kỹ năng dài hơn 990 bị từ chối.
- PDF giả, sai phần mở rộng, thiếu EOF bị từ chối.
- PDF hợp lệ lưu tên GUID, tải xuống đúng nội dung; đường dẫn trực tiếp trả 404.
- File đúng 10 MB được nhận; 10 MB + 1 byte bị từ chối.
- Sau khi thay CV, đơn ứng tuyển thử vẫn trỏ tới CV cũ và file cũ còn tồn tại.
- Tài khoản, đơn và file kiểm thử tạm đã dọn; giữ 20 sinh viên, 6 doanh nghiệp, 21 đơn demo.
- Đã xem ảnh bố cục hồ sơ từ HTML của phiên demo ở desktop 1440 px và một cột 600 px. Chưa tự động kiểm tra viewport 390 px và tương tác dropdown khoa/ngành trong trình duyệt.

## Lỗi thường gặp

- Chưa có hồ sơ Student: kiểm tra role và hồ sơ tài khoản, không tự chọn ID của người khác.
- Không tải được CV demo sau khi chuyển máy: file uploads không nằm trong Git, cần chuyển file tương ứng hoặc seed trên database mới.
- Báo PDF không đầy đủ: xuất lại CV từ công cụ tạo PDF.
- Báo vượt 10 MB: giảm dung lượng rồi tải lại.
- File mới không lưu được: kiểm tra quyền ghi thư mục uploads; không mở công khai đường dẫn để xử lý.

## Git checkpoint

Chưa commit thay đổi phần này. Từ thư mục gốc, git status xem file; git diff xem nội dung. Sau khi kiểm tra có thể chọn InternshipPortal, docs và PROJECT_REQUIREMENTS.md bằng git add rồi commit với message đề xuất: feat: add student profile and private CV upload. Không tự push.

## Trạng thái dự án

Đã hoàn thành: hồ sơ cơ bản và CV trong giai đoạn 7.1; đã build/chạy/kiểm tra quyền và upload.

Đang thực hiện: chờ xác nhận cách tích hợp GPA; chưa hoàn thành toàn bộ giai đoạn 7.

Bước tiếp theo: bổ sung GPA sau khi thống nhất; sau đó danh sách/tìm kiếm vị trí cho Student theo lộ trình.
