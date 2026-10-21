# Kết quả ban đầu — Cổng thông tin Thực tập và Kết nối Doanh nghiệp

Ngày chốt: **08/10/2026, giờ Việt Nam**.

**Cập nhật 09/10/2026:** đã triển khai và kiểm tra [Hồ sơ doanh nghiệp](docs/15-company-profile.md), [Quản lý vị trí Company](docs/16-company-positions.md), [Ứng viên/CV Company](docs/17-company-applicants.md) và [Xử lý ứng viên](docs/18-company-decisions.md). Người dùng đã xác nhận tự hủy các đơn chưa kết thúc cùng đợt khi duyệt. Các nội dung bên dưới giữ lại mốc tối 08/10. Điểm tiếp tục mới: quản lý thực tập và người hướng dẫn Company; UI/UX để giai đoạn sau.

Trạng thái: **Tạm dừng theo yêu cầu người dùng**. Không tự triển khai thêm cho đến khi người dùng yêu cầu tiếp tục.

## 1. Bản đang chạy

- Project: D:\Portal\code\InternshipPortal.
- Website: http://localhost:5086.
- Công nghệ: ASP.NET Core MVC .NET 8, C#, Razor Views, EF Core 8, SQL Server, Identity, Bootstrap 5.
- Database: InternshipPortalDb, SQL Server localhost, Windows Authentication.
- Giao diện đã chọn: nền kem, chữ than, đỏ gạch; mẫu CV riêng nền trắng/xanh đậm.
- Next.js cũ trong thư mục gốc được giữ để tham khảo, không phải bản .NET hiện tại.

## 2. Phần đã hoàn thành và kiểm tra

| Phần | Kết quả hiện tại |
|---|---|
| Yêu cầu và thiết kế | PROJECT_REQUIREMENTS.md, ERD và tài liệu theo từng phần |
| Khung MVC | Trang chủ, layout, menu, routing Area và trang lỗi ban đầu |
| Database | 12 bảng nghiệp vụ, Identity, Migration, FK/unique/check constraint |
| Giới hạn SQL | Các cột chuỗi có độ dài cụ thể, không có max trong bảng ứng dụng; mỗi bảng nghiệp vụ có liên kết |
| Dữ liệu demo | Khoa/ngành/đợt, doanh nghiệp, sinh viên, vị trí, đơn, thực tập, nhật ký/tài liệu/đánh giá |
| Tài khoản | Đăng ký Student/Company, đăng nhập/đăng xuất, 3 role và trang đầu các cổng |
| Hồ sơ Student | Xem/sửa thông tin của mình, kiểm tra khoa/ngành và MSSV |
| CV | Upload/tải PDF 10 MB, tên GUID, bảo vệ file và giữ CV trong đơn đã gửi |
| Mẫu CV | Soạn hai cột, chọn ảnh, xem trước và in/lưu PDF A4; phần soạn thêm chưa tự lưu lâu dài |
| Vị trí công khai | Tìm kiếm, 4 bộ lọc, phân trang, chi tiết vị trí/doanh nghiệp |
| Ứng tuyển Student | Gửi đơn, lịch sử/trạng thái, tải CV của đơn, rút Pending; chặn trùng/hết hạn/đủ chỉ tiêu |
| Theo dõi thực tập | Hồ sơ của mình, trạng thái, thời gian và người hướng dẫn |
| Nhật ký tuần | Tạo/sửa khi InProgress, tuần từ ngày bắt đầu; trạng thái khác chỉ đọc |
| Tài liệu | PDF/DOCX 10 MB, nộp khi đang/kết thúc, lưu từng lần, tải theo quyền |
| Xem đánh giá | Sáu tiêu chí, trung bình tự tính, nhận xét, chỉ xem dữ liệu của mình |

Company và Admin hiện có **khung đăng nhập/phân quyền và trang đầu**, chưa có đầy đủ chức năng quản lý.

## 3. Kết quả kiểm thử đã có

- Build cuối: **thành công, 0 lỗi, 0 cảnh báo**.
- Website khởi động được; trang chủ/danh sách công khai HTTP 200.
- EF model khớp Migration, không còn thay đổi schema chưa tạo Migration.
- Kiểm tra SQL chống trùng đơn, hồ sơ cùng đợt, nhật ký tuần, đánh giá; giới hạn điểm và FK đều đạt.
- Đã kiểm tra role, chủ sở hữu, CSRF, overposting, URL chuyển hướng, tài khoản ngừng hoạt động và khóa sau sai mật khẩu nhiều lần.
- Gửi đơn/nhật ký đồng thời không tạo trùng; xử lý trường hợp SQL deadlock trả thông báo.
- File thật/giả, tên/dung lượng, tải đúng nội dung và MIME, bảo vệ uploads đã được kiểm tra.
- Test fixture đã dọn, dữ liệu demo giữ nguyên.
- Đã xem ảnh giao diện một số trang desktop/600 px. Chưa có bộ kiểm thử trình duyệt hoàn chỉnh, chưa xác minh toàn bộ trên điện thoại thực tế.

## 4. Dữ liệu hiện có

| Dữ liệu | Số lượng |
|---|---:|
| Bảng nghiệp vụ | 12 |
| Sinh viên | 20 |
| Doanh nghiệp | 6 |
| Vị trí | 18, hiện 14 vị trí công khai |
| Đơn ứng tuyển | 21 |
| Hồ sơ thực tập | 8 |
| Nhật ký | 6 |
| Tài liệu | 4 |
| Đánh giá | 4 |

Vị trí công khai có thể giảm khi hết hạn về sau; con số 14 là tại thời điểm chốt.

## 5. Tài khoản để demo

| Tài khoản | Cách dùng |
|---|---|
| admin@demo.edu.vn | Xem khung cổng nhà trường |
| company@demo.vn | Xem khung cổng doanh nghiệp được duyệt |
| company06@demo.vn | Doanh nghiệp chờ duyệt |
| student@demo.edu.vn | Hồ sơ/CV, đơn đã duyệt, thực tập Chưa bắt đầu |
| student09@demo.edu.vn | Thử tìm và gửi đơn mới |
| student03@demo.edu.vn | Thực tập đang diễn ra, viết/sửa nhật ký, nộp tài liệu |
| student05@demo.edu.vn | Xem tài liệu và đánh giá mẫu của hồ sơ kết thúc |

Các tài khoản demo ban đầu dùng mật khẩu lưu tại Seed:DemoPassword. Xem trên terminal cá nhân từ thư mục gốc:

```powershell
dotnet user-secrets list --project .\InternshipPortal\InternshipPortal.csproj
```

Không đưa mật khẩu vào mã nguồn hoặc Git. Nội dung hồ sơ/doanh nghiệp mẫu là dữ liệu giả lập.

## 6. Chạy lại khi tiếp tục

Từ D:\Portal\code:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Nếu app đã chạy, không mở thêm bản trên cùng cổng. Dừng terminal cũ bằng Ctrl+C rồi chạy lại nếu cần.

Các trang nên xem:

- / — trang chủ.
- /Internships — vị trí công khai.
- /Account/Login — đăng nhập.
- /Student/Profile — hồ sơ/CV.
- /Student/Profile/Cv — mẫu CV.
- /Student/Applications — lịch sử ứng tuyển.
- /Student/Internships — theo dõi thực tập.

Trong hồ sơ thực tập có liên kết tới nhật ký, tài liệu và đánh giá.

## 7. Phần chưa hoàn thành

- GPA: chưa chốt hỗ trợ thang 4, thang 10 hay cả hai; chưa thêm trường/thay quy tắc GPA.
- Company: sửa hồ sơ, tạo/sửa/đóng vị trí, xử lý ứng viên, duyệt/từ chối, gán hướng dẫn, cập nhật thực tập và tạo đánh giá.
- Admin: duyệt doanh nghiệp, quản lý khoa/ngành/đợt, tài khoản và giám sát toàn hệ thống.
- Luồng duyệt tạo Internship/hủy các đơn còn lại/thông báo đầy đủ theo nghiệp vụ chung.
- Danh sách thông báo, đánh dấu đã đọc, nhắc hạn và gửi thông báo.
- Dashboard/thống kê Chart.js.
- Hoàn thiện trang lỗi toàn hệ thống, kiểm thử tích hợp toàn bộ luồng, giao diện mobile và tài liệu bảo vệ/demo cuối.

Chưa xem hệ thống là đồ án hoàn chỉnh. Điểm dừng hiện tại: phần Student chính đã triển khai đến tài liệu/xem đánh giá, trước khi làm chức năng Company/Admin.

## 8. Git và lưu trữ

- Repo Git đã khởi tạo tại D:\Portal\code.
- Commit f9e0096 lưu giao diện đầy đủ ban đầu.
- Bản giao diện rút gọn đã cất trong stash.
- Các thay đổi mới của database/Identity/Student/tài liệu **đã lưu trên ổ đĩa, chưa commit và chưa push**.
- Git không tự lưu database SQL hoặc các file uploads đang bị ignore. Muốn chuyển máy cần chuyển/backup dữ liệu và file riêng tư tương ứng.

Nếu người dùng muốn lưu mốc mới, từ thư mục gốc:

```powershell
git status
git diff
git add InternshipPortal docs PROJECT_REQUIREMENTS.md KET_QUA_BAN_DAU.md
git commit -m "feat: complete initial student portal"
```

Status/diff xem thay đổi; add chọn file; commit lưu mốc trên máy. AI chưa tự chạy add/commit/push cho mốc này.

## 9. Tài liệu chi tiết

- PROJECT_REQUIREMENTS.md — yêu cầu và quy tắc đã xác nhận.
- docs/03-database-design.md — ERD/database.
- docs/04-mvc-project.md — khung MVC.
- docs/06-database-setup.md — database/seed.
- docs/07-string-lengths.md — giới hạn chuỗi.
- docs/08-authentication.md — tài khoản/role.
- docs/09-student-profile.md — hồ sơ/CV.
- docs/10-cv-template.md — mẫu CV.
- docs/11-position-catalog.md — tìm vị trí.
- docs/12-student-applications.md — đơn ứng tuyển.
- docs/13-student-internships-diaries.md — nhật ký.
- docs/14-student-documents-evaluation.md — tài liệu/đánh giá.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các phần nêu ở mục 2 đã được kiểm tra trong phạm vi ghi rõ.

Đang thực hiện: **tạm dừng theo người dùng ngày 08/10/2026**.

Bước tiếp theo khi người dùng yêu cầu tiếp tục: chốt GPA/phần nghiệp vụ còn mở rồi triển khai Company theo lộ trình; không tự bắt đầu trong lúc tạm dừng.
