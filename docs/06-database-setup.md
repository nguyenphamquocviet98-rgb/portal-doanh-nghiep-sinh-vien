# Giai đoạn 5 — EF Core, SQL Server, Migration và dữ liệu mẫu

## Mục tiêu và kết quả

Tạo database InternshipPortalDb với đúng 12 bảng nghiệp vụ theo thiết kế đã duyệt và các bảng ASP.NET Core Identity. Có Migration đầu tiên và dữ liệu mẫu tiếng Việt để dùng ở các giai đoạn chức năng.

Máy hiện tại có SQL Server mặc định đang chạy tại localhost; không cần cài LocalDB. Dùng tài khoản Windows để kết nối. Không đổi tên database hoặc chuyển công nghệ.

## File tạo/sửa

| Đường dẫn tính từ thư mục dự án | Mục đích |
|---|---|
| InternshipPortal/Models/*.cs | 12 Entity nghiệp vụ, ApplicationUser, BaseEntity |
| InternshipPortal/Models/Enums/Statuses.cs | Enum và nhãn tiếng Việt |
| InternshipPortal/Data/ApplicationDbContext.cs | Quan hệ, kiểu dữ liệu, unique index, check constraint |
| InternshipPortal/Data/SeedData.cs | Tạo dữ liệu demo trong một transaction; chạy lại không nhân đôi |
| InternshipPortal/Migrations/*InitialCreate* | Migration đầu tiên |
| InternshipPortal/Migrations/ApplicationDbContextModelSnapshot.cs | Mô hình EF hiện tại |
| InternshipPortal/InternshipPortal.csproj | EF SQL Server, EF Design, Identity EF 8.0.31 và UserSecretsId |
| InternshipPortal/.config/dotnet-tools.json | Công cụ dotnet-ef 8.0.31 cài theo project |
| InternshipPortal/Program.cs | Đăng ký DbContext/IdentityCore, lệnh seed và chặn file riêng tư |
| InternshipPortal/appsettings.Development.json | Connection String local, giảm log truy vấn |
| InternshipPortal/README.md | Cách chạy và dữ liệu demo |
| docs/VerifyDatabase.sql | Kiểm tra ràng buộc bằng transaction và rollback |

Không sửa giao diện. Chưa tạo màn hình đăng nhập/đăng ký, chưa có thao tác ứng tuyển. IdentityCore được dùng để tạo tài khoản/role và băm mật khẩu, chưa cấu hình đăng nhập cookie.

## Cách chạy lại trên máy hiện tại

Database và seed đã được tạo, chỉ cần từ D:\Portal\code:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Mở http://localhost:5086. Nếu đang có bản chạy tại cổng này, dừng terminal của bản đó bằng Ctrl+C trước.

## Chuẩn bị database trên máy khác

Máy cần .NET SDK 8 và SQL Server/LocalDB. Nếu dùng instance khác, đặt ConnectionStrings:DefaultConnection qua User Secrets, giữ tên database InternshipPortalDb. Ví dụ LocalDB:

```powershell
dotnet user-secrets set 'ConnectionStrings:DefaultConnection' 'Server=(localdb)\MSSQLLocalDB;Database=InternshipPortalDb;Trusted_Connection=True;TrustServerCertificate=True'
```

Các lệnh bên dưới chạy từ thư mục InternshipPortal:

```powershell
dotnet restore
dotnet tool restore
dotnet build
```

Restore tải dependency và công cụ EF; build kiểm tra code. Sau đó:

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet ef database update
```

Lệnh EF áp dụng Migration còn thiếu. Không dùng EnsureCreated, không xóa database và không xóa Migration cũ.

Đặt mật khẩu demo trước khi seed:

```powershell
dotnet user-secrets set 'Seed:DemoPassword' '<mat-khau-demo-cua-ban>'
dotnet run --launch-profile http -- --seed-demo
```

Thay chuỗi <...> bằng mật khẩu cho demo, ít nhất 10 ký tự, có hoa/thường/số/ký tự đặc biệt. Không ghi mật khẩu vào code. Seed chỉ chạy khi gọi --seed-demo trong Development, không chạy tự động mỗi lần mở app.

Mật khẩu trên máy hiện tại được tạo ngẫu nhiên và lưu trong User Secrets, nằm ngoài repo. Để xem trên terminal cá nhân:

```powershell
dotnet user-secrets list
```

Đầu ra chứa secret, không chụp hoặc gửi công khai. Các tài khoản demo ban đầu dùng chung mật khẩu này. Nếu thay secret sau khi đã seed, mật khẩu tài khoản cũ vẫn giữ nguyên; seed không tự đặt lại mật khẩu.

## Dữ liệu đã tạo

| Dữ liệu | Số lượng |
|---|---:|
| Khoa | 3 |
| Ngành | 6 |
| Đợt thực tập | 1 |
| Doanh nghiệp | 6, trong đó 1 chờ duyệt |
| Vị trí | 18, có đang tuyển và đã đóng |
| Sinh viên | 20 |
| Đơn ứng tuyển | 21, đủ 7 trạng thái |
| Hồ sơ thực tập | 8, mỗi trạng thái có 2 hồ sơ |
| Nhật ký | 6 |
| Tài liệu | 4 |
| Đánh giá | 4 |
| Thông báo | 6 |

Tài khoản: admin@demo.edu.vn, company@demo.vn, student@demo.edu.vn. Ngoài ra có company02..06@demo.vn và student02..20@demo.edu.vn.

Dữ liệu doanh nghiệp/sinh viên là giả lập. Ngày được tạo tương đối với thời điểm seed để có tình huống hiện tại và lịch sử. GPA để trống vì chưa chốt thang điểm.

Có 20 file CV PDF và 4 báo cáo PDF mẫu hợp lệ với tên GUID. Chúng nằm trong wwwroot/uploads/demo, database chỉ lưu đường dẫn. File bị bỏ qua trong Git; chuyển sang máy khác cần seed lại trên database mới hoặc chuyển file riêng tư tương ứng cùng dữ liệu.

## Kiểm tra đã chạy thực tế

- Build: 0 warning, 0 error.
- InitialCreate đã áp dụng trên SQL Server; có lịch sử Migration.
- Đúng 12 bảng nghiệp vụ, cộng 7 bảng Identity và bảng lịch sử Migration.
- Model không có thay đổi chưa tạo Migration.
- Migration BoundStringLengths đã giới hạn tất cả cột chuỗi, cả Identity; không còn varchar(max)/nvarchar(max) trong các bảng ứng dụng.
- Các bảng nghiệp vụ đều có FK đi ra hoặc được bảng khác tham chiếu; không có bảng nghiệp vụ đứng riêng.
- Seed lần thứ hai không đổi số lượng dữ liệu hoặc tạo thêm file.
- Có đúng Admin, Company, Student và tài khoản được gán role.
- Không có FK cascade delete.
- Đã kiểm tra chống trùng đơn, hồ sơ cùng đợt, nhật ký tuần và đánh giá.
- Database từ chối điểm vượt 10, từ chối đơn thiếu lý do và FK không tồn tại.
- Các thao tác thử ràng buộc nằm trong transaction và đã rollback; không lưu bản ghi thử.
- Web khởi động, trang chủ và quyền riêng tư HTTP 200, giữ giao diện đầy đủ.
- URL trực tiếp tới file PDF thật trong uploads trả 404.

Chạy lại kiểm tra SQL từ thư mục gốc khi có dữ liệu demo:

```powershell
sqlcmd -S localhost -E -C -d InternshipPortalDb -b -i .\docs\VerifyDatabase.sql
```

Kết quả đúng: 7 dòng PASS và thông báo toàn bộ kiểm tra đã đạt. Script này chưa kiểm tra các nghiệp vụ liên bảng của Service hoặc quyền truy cập các cổng; thực hiện khi có chức năng tương ứng.

## Lỗi thường gặp

- Không kết nối được SQL Server: kiểm tra service, tên instance và cấu hình kết nối.
- Thiếu dotnet-ef: chạy dotnet tool restore từ InternshipPortal.
- Thiếu Seed:DemoPassword: đặt qua User Secrets, không ghi vào appsettings.
- Thiếu bảng: kiểm tra môi trường Development rồi chạy database update.
- Seed báo dữ liệu chưa đầy đủ: kiểm tra dữ liệu đã tồn tại; không xóa database để xử lý nhanh.
- Port 5086 đang được dùng: dừng bản app đang chạy trước khi mở bản mới.
- CV tải trực tiếp bị 404: đây là hành vi có chủ đích để bảo vệ dữ liệu; action tải theo quyền sẽ làm sau.

## Git checkpoint

Chưa commit phần giai đoạn 5. Từ thư mục gốc:

1. git status: xem các file mới/sửa.
2. git diff: xem nội dung sửa trong file đã theo dõi.
3. git add InternshipPortal docs: chọn code và tài liệu, không chọn file uploads bị ignore.
4. git commit -m "feat: add EF Core database and development seed": lưu mốc sau khi kiểm tra.

Git lưu mã nguồn và Migration, không tự backup dữ liệu SQL Server hoặc file uploads. Không push khi chưa được yêu cầu.

## Trạng thái dự án

Đã hoàn thành: giai đoạn 5 — EF Core, SQL Server, Migration và dữ liệu demo; build/database/khởi động đã kiểm tra.

Đang thực hiện: chờ người dùng kiểm tra và yêu cầu tiếp tục.

Bước tiếp theo: giai đoạn 6 — đăng nhập, đăng ký và phân quyền ba role bằng Identity; giữ phong cách giao diện đã thống nhất.
