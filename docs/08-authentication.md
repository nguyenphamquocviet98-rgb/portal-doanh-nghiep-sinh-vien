# Giai đoạn 6 — Identity, đăng nhập, đăng ký và role

## Mục tiêu và kết quả

Có đăng nhập/đăng xuất, đăng ký Student và Company, chuyển đúng cổng và kiểm tra truy cập theo role. Giữ giao diện nền kem, chữ than, đỏ gạch đã thống nhất.

Không thêm bảng, không sửa schema. Các cổng hiện hiển thị thông tin ban đầu; chưa có quản lý hồ sơ, vị trí hoặc ứng tuyển đầy đủ.

## File tạo/sửa

| File trong InternshipPortal | Chức năng |
|---|---|
| Program.cs | Identity và cookie; Authentication trước Authorization; đăng ký Service/filter |
| Controllers/AccountController.cs | Login, Register, RegisterStudent, RegisterCompany, Portal, Logout, AccessDenied |
| Services/AccountService.cs | Kiểm tra ngành/MSSV/email; tạo tài khoản + role + hồ sơ trong một transaction |
| Services/VietnameseIdentityErrorDescriber.cs | Lỗi Identity bằng tiếng Việt |
| Filters/ActiveAccountFilter.cs | Kiểm tra tài khoản đang hoạt động, sign out khi bị vô hiệu hóa/khóa |
| ViewModels/Account/*.cs | Form và validation tiếng Việt, độ dài khớp cột SQL |
| ViewModels/PortalViewModel.cs | Thông tin trang đầu từng cổng |
| Views/Account/*.cshtml | Các trang tài khoản và form dùng chung |
| Areas/{Admin,Company,Student}/Controllers/DashboardController.cs | Authorize theo role; dữ liệu theo user đăng nhập |
| Areas/{Admin,Company,Student}/Views/* | Razor Views dùng layout chung |
| Views/Shared/_PortalHome.cshtml | Nội dung đầu cổng |
| Views/Shared/_Layout.cshtml | Menu đăng nhập/đăng ký hoặc cổng của tôi/đăng xuất |
| wwwroot/css/site.css | CSS cho form và cổng |
| wwwroot/js/register-student.js | Lọc ngành theo khoa trên form |
| README.md | Hướng dẫn cập nhật |

## Hành vi

- Student đăng ký bằng họ tên, email, điện thoại tùy chọn, MSSV, lớp, khoa/ngành và mật khẩu.
- Company đăng ký thông tin doanh nghiệp và người liên hệ; trạng thái luôn Pending.
- Role được chọn trong Service theo action đăng ký; không nhận role Admin từ form.
- Khoa/ngành được kiểm tra ở server, ngoài việc lọc bằng JavaScript.
- Email và MSSV trùng bị từ chối; unique index bảo vệ cả đăng ký đồng thời.
- Đăng ký thành công trở về trang Login với thông báo tiếng Việt.
- Login dùng Identity để kiểm tra hash; không so sánh mật khẩu dạng rõ trong database.
- Năm lần sai mật khẩu khóa tài khoản 15 phút.
- Giữ đăng nhập tạo cookie lưu lâu hơn phiên trình duyệt; cookie HttpOnly, thời hạn 8 giờ và sliding expiration.
- Không yêu cầu email xác nhận ở phạm vi hiện tại vì chưa xây chức năng gửi email.
- Tài khoản bị IsActive=false hoặc bị lockout không được dùng cổng; phiên cũ cũng được chặn ở lần truy cập MVC tiếp theo.
- Logout chỉ dùng POST, có Authorize và AntiForgeryToken.
- Login/Register là các action công khai cần thiết, vẫn kiểm tra AntiForgeryToken trên POST.
- ReturnUrl chỉ chấp nhận đường dẫn nội bộ.
- Sai role chuyển tới trang 403; chưa đăng nhập chuyển tới Login.
- Student/Company dashboard lấy UserId từ phiên, không nhận ID hồ sơ từ URL để chọn dữ liệu.
- Company Pending/Rejected xem thông báo trạng thái. Đăng vị trí sẽ chỉ được triển khai cùng kiểm tra đã duyệt ở giai đoạn Company.

## Đường dẫn để thử

| Trang | URL |
|---|---|
| Đăng nhập | http://localhost:5086/Account/Login |
| Chọn loại đăng ký | http://localhost:5086/Account/Register |
| Đăng ký sinh viên | http://localhost:5086/Account/RegisterStudent |
| Đăng ký doanh nghiệp | http://localhost:5086/Account/RegisterCompany |
| Cổng theo tài khoản | http://localhost:5086/Account/Portal |
| Sinh viên | http://localhost:5086/Student/Dashboard |
| Doanh nghiệp | http://localhost:5086/Company/Dashboard |
| Nhà trường | http://localhost:5086/Admin/Dashboard |

Tài khoản demo: student@demo.edu.vn, company@demo.vn, admin@demo.edu.vn. company06@demo.vn là doanh nghiệp chờ duyệt.

Mật khẩu hiện tại lưu ngoài repo. Từ D:\Portal\code, xem trên terminal cá nhân:

```powershell
dotnet user-secrets list --project .\InternshipPortal\InternshipPortal.csproj
```

Dùng giá trị Seed:DemoPassword. Không chia sẻ đầu ra có mật khẩu. Không đặt mật khẩu trong README hoặc commit.

## Chạy lại

App hiện đang chạy. Nếu cần chạy lại, dừng terminal bằng Ctrl+C rồi:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Lệnh run sẽ build trước khi mở web. Không cần Migration mới vì giai đoạn này không thay đổi schema.

## Checklist kiểm tra thủ công

1. Mở Login, đăng nhập bằng Student: vào đúng cổng và thấy MSSV/ngành của mình.
2. Đăng xuất, đăng nhập Company: thấy đúng doanh nghiệp và trạng thái.
3. Đăng nhập Admin: vào cổng nhà trường.
4. Dùng Student mở /Admin/Dashboard: thấy trang không có quyền.
5. Chưa đăng nhập mở một cổng: được đưa về Login.
6. Đăng ký Student mới: chọn khoa/ngành, nhập mật khẩu đủ mạnh; đăng nhập được sau đăng ký.
7. Thử trùng email/MSSV: thấy lỗi tiếng Việt, không tạo thêm tài khoản.
8. Đăng ký Company mới: trạng thái Chờ duyệt, không được tự chọn role Admin.
9. Xem trang trên điện thoại: form xếp một cột; khoa/ngành/lớp chia cột phù hợp chiều rộng.

## Kiểm thử đã chạy trực tiếp

- Build cuối: 0 warning, 0 error.
- Các trang công khai HTTP 200; ba cổng chặn người chưa đăng nhập.
- Login và chuyển cổng đúng cho cả ba tài khoản demo.
- Sáu trường hợp truy cập chéo role đều bị chặn; trang AccessDenied trả 403.
- Login/Register/Logout POST thiếu anti-forgery token trả 400.
- Logout xóa phiên; truy cập cổng sau đó trở về Login.
- Mật khẩu sai bị từ chối; ReturnUrl ngoài hệ thống không được chuyển hướng.
- Đăng ký Student/Company thật bằng dữ liệu kiểm thử tạm thành công, đăng nhập được.
- Role=Admin và ApprovalStatus=Approved giả từ form không có hiệu lực.
- Chọn khoa/ngành không khớp, mật khẩu yếu, họ tên quá dài, email/MSSV trùng bị từ chối.
- Hai request đăng ký Company đồng thời cùng email: đúng một thành công, một báo lỗi; không còn hồ sơ/tài khoản dư.
- Tài khoản bị ngừng hoạt động bị chặn trong phiên đang đăng nhập; thử đúng mật khẩu cũng không đăng nhập được.
- Năm lần mật khẩu sai khóa tài khoản kiểm thử; mật khẩu đúng vẫn bị chặn khi còn khóa.
- Tài khoản kiểm thử tạm đã dọn theo ID/email riêng; giữ 20 sinh viên, 6 doanh nghiệp và dữ liệu demo.
- 7 kiểm tra database vẫn đạt; không còn cột chuỗi max; model không có thay đổi cần Migration.
- Đã xem ảnh Edge headless trang Login ở 1440 px và form Student ở 600 px. Chưa tự động xác minh viewport 390 px hoặc tương tác dropdown bằng trình duyệt; cần kiểm tra thêm trên điện thoại thực tế.

## Lỗi thường gặp

- Đăng nhập thất bại với demo: kiểm tra dùng đúng Seed:DemoPassword; đổi User Secrets sau seed không tự đổi mật khẩu tài khoản cũ.
- Sai 5 lần bị khóa: đợi 15 phút; không xóa tài khoản/database để xử lý.
- Form báo mã sinh viên trùng: dùng MSSV khác, kể cả email khác.
- Không thấy ngành: chọn khoa và kiểm tra danh mục đang hoạt động.
- SQLCMD sửa dữ liệu báo QUOTED_IDENTIFIER: dùng tùy chọn -I khi thao tác bảng có filtered index.
- Đăng ký đồng thời trùng có thể tạo log lỗi unique index của EF; lỗi được xử lý thành thông báo form, transaction rollback và không trả 500.
- Cổng 5086 bị chiếm: dừng bản đang chạy trước khi chạy lại.

## Git checkpoint tùy chọn

Các thay đổi giai đoạn 5–6 chưa commit. Từ thư mục gốc:

```powershell
git status
git diff
```

Hai lệnh xem file/nội dung thay đổi, không lưu mốc. Sau khi kiểm tra:

```powershell
git add InternshipPortal docs PROJECT_REQUIREMENTS.md
git commit -m "feat: add database and Identity authentication"
```

Add chọn code/tài liệu; commit lưu phiên bản tại máy. Không tự push lên GitHub. Git không lưu dữ liệu SQL Server hoặc file uploads bị ignore.

## Trạng thái dự án

Đã hoàn thành: giai đoạn 6 — đăng nhập, đăng ký, đăng xuất, role và trang đầu ba cổng; đã kiểm tra build, database, HTTP và quyền cơ bản.

Đang thực hiện: chờ người dùng kiểm tra giao diện và luồng tài khoản.

Bước tiếp theo: giai đoạn 7 — chức năng Student, bắt đầu hồ sơ cá nhân và CV; cần chốt thang GPA và giới hạn upload trước phần liên quan.
