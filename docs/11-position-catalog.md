# Giai đoạn 7.2 — Danh sách và tìm kiếm vị trí thực tập

## Mục tiêu và kết quả

Cho sinh viên và khách xem vị trí công khai, tìm kiếm/lọc, xem chi tiết vị trí và doanh nghiệp. Giữ giao diện nền kem, chữ than, đỏ gạch. Giai đoạn này chỉ đọc dữ liệu, chưa gửi đơn ứng tuyển.

## File tạo/sửa

| File trong InternshipPortal | Chức năng |
|---|---|
| Controllers/InternshipsController.cs | Danh sách, chi tiết và trang không tìm thấy |
| Controllers/CompaniesController.cs | Thông tin doanh nghiệp công khai |
| Services/PositionCatalogService.cs | Điều kiện công khai, bộ lọc, phân trang và số chỗ còn |
| ViewModels/Internships/*.cs | Form tìm kiếm và dữ liệu hiển thị |
| Helpers/DisplayExtensions.cs | Nhãn enum tiếng Việt và ngày/giờ Việt Nam UTC+7 |
| Views/Internships/Index.cshtml | Danh sách và bộ lọc |
| Views/Internships/Details.cshtml | Chi tiết vị trí |
| Views/Internships/Unavailable.cshtml | Trang 404 cho thông tin không công khai/không tồn tại |
| Views/Companies/Details.cshtml | Giới thiệu doanh nghiệp và các vị trí đang tuyển |
| Views/Shared/_PositionList.cshtml | Danh sách dùng chung |
| Views/Shared/_Layout.cshtml | Menu vị trí và breakpoint phù hợp độ rộng |
| Views/Home/Index.cshtml | Nút chính dẫn tới danh sách vị trí |
| Areas/Student/Views/Dashboard/Index.cshtml | Liên kết tìm vị trí từ cổng Student |
| Program.cs | Đăng ký Service; thông báo binding bằng tiếng Việt |
| wwwroot/css/site.css | CSS danh sách, bộ lọc và chi tiết |

Không thay Entity/DbContext hoặc tạo Migration.

## Quy tắc

- Vị trí công khai: Open, hạn ứng tuyển chưa qua, doanh nghiệp Approved, doanh nghiệp và tài khoản doanh nghiệp đang hoạt động.
- Dùng cùng điều kiện trên danh sách, chi tiết và trang doanh nghiệp, tránh mở được dữ liệu ẩn bằng URL.
- Tìm kiếm theo tên vị trí; lọc ngành, địa điểm, doanh nghiệp và WorkMode.
- Kết hợp bộ lọc bằng điều kiện AND.
- Form GET để URL có thể chia sẻ. Không có API riêng.
- Mỗi trang tối đa 9 vị trí, sắp xếp mới nhất rồi Id giảm dần để phân trang ổn định.
- Trang ngoài phạm vi được giới hạn về trang đầu/cuối, không bỏ qua kết quả hoặc gây overflow.
- Bộ lọc chỉ lấy lựa chọn từ các vị trí đang công khai.
- Còn chỗ = Quantity trừ số đơn Approved, tối thiểu 0; không lưu thêm cột đếm.
- Vị trí đủ chỉ tiêu vẫn có thể xem thông tin nếu còn Open/chưa hết hạn; điều kiện không được ứng tuyển khi đủ chỉ tiêu sẽ kiểm tra ở chức năng gửi đơn.
- Không tự thêm kiểm tra trạng thái đợt vì quy tắc này chưa chốt.
- Công khai chỉ thông tin doanh nghiệp phù hợp: tên, địa chỉ, lĩnh vực, giới thiệu, website; không đưa email đăng nhập, CV, hồ sơ hoặc danh sách sinh viên ra View.
- Liên kết website chỉ nhận http/https và mở với noopener/noreferrer.
- Ngày từ database hiểu là UTC; hiển thị giờ Việt Nam, không phụ thuộc timezone máy chạy.

## Cách chạy và dùng

App hiện đang chạy. Mở http://localhost:5086/Internships hoặc chọn Vị trí thực tập trong menu. Không cần đăng nhập để xem.

Để chạy lại từ thư mục gốc, dừng terminal đang chạy bằng Ctrl+C rồi:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

URL ví dụ:

- /Internships?Keyword=.NET
- /Internships?MajorId=1
- /Internships?Location=H%C3%A0%20N%E1%BB%99i
- /Internships?CompanyId=1&WorkMode=1
- /Internships/Details/1
- /Companies/Details/1

## Checklist người dùng

1. Danh sách có 14 vị trí công khai với dữ liệu demo hiện tại; các ngày có thể hết hạn về sau.
2. Tìm .NET: chỉ thấy tên vị trí chứa từ khóa.
3. Chọn nhiều bộ lọc rồi Tìm kiếm: kết quả thỏa tất cả điều kiện.
4. Trang sau giữ nguyên bộ lọc; không lặp vị trí giữa các trang.
5. Bỏ bộ lọc: trở về toàn bộ vị trí công khai.
6. Xem chi tiết: đủ mô tả, yêu cầu, thời gian, hạn nộp, chỉ tiêu và doanh nghiệp.
7. Bấm tên doanh nghiệp: thấy giới thiệu và vị trí đang tuyển của doanh nghiệp đó.
8. /Internships/Details/5 (đã đóng) và /Companies/Details/6 (chờ duyệt) trả trang 404.

## Kiểm thử đã thực hiện

- Build cuối: 0 warning, 0 error; app khởi động bình thường.
- Model không có thay đổi chưa tạo Migration.
- HTTP danh sách, tìm kiếm, chi tiết vị trí và doanh nghiệp đều 200 với dữ liệu hợp lệ.
- Số lượng tìm kiếm/lọc từng điều kiện và kết hợp khớp truy vấn SQL kiểm tra.
- Danh sách 14 vị trí; hai trang không lặp/mất bản ghi.
- Kiểm tra không có kết quả, trang âm, trang quá lớn, WorkMode sai, từ khóa quá 100 ký tự và giả trường kết quả của ViewModel.
- Kiểm tra điều kiện công khai bằng dữ liệu tạm: doanh nghiệp Pending, ngừng hoạt động, tài khoản ngừng hoạt động, vị trí hết hạn và đã đóng đều không mở được chi tiết.
- Doanh nghiệp chờ duyệt không có trang công khai; không lộ email đăng nhập trong trang doanh nghiệp.
- Các account/company/position kiểm thử tạm đã dọn; giữ 20 sinh viên, 6 doanh nghiệp, 18 vị trí và 21 đơn.
- Đã xem ảnh bố cục danh sách desktop 1440 px và một cột 600 px. Chưa tự động thao tác menu hoặc kiểm tra viewport 390 px.
- Validation tham số sai hiển thị thông báo tiếng Việt.

## Lỗi thường gặp

- Kết quả ít hơn dữ liệu trong SQL: vị trí đóng/hết hạn hoặc công ty chưa duyệt sẽ bị ẩn đúng nghiệp vụ.
- Chi tiết 404 dù biết Id: dữ liệu có thể không còn đủ điều kiện công khai.
- Không thấy lựa chọn ngành/doanh nghiệp trong bộ lọc: chưa có vị trí đang công khai tương ứng.
- Chưa thấy nút gửi đơn: chức năng ứng tuyển sẽ làm ở phần tiếp theo, không tạo nút giả.

## Git checkpoint

Chưa commit thay đổi. Từ thư mục gốc:

```powershell
git status
git diff
```

Sau khi kiểm tra có thể chọn code/tài liệu bằng git add InternshipPortal docs PROJECT_REQUIREMENTS.md, rồi lưu mốc với message đề xuất feat: add public internship catalog and search. Không tự push.

## Trạng thái dự án

Đã hoàn thành: giai đoạn 7.2 — danh sách, tìm kiếm/lọc, phân trang, chi tiết vị trí và doanh nghiệp.

Đang thực hiện: giai đoạn 7 chưa hoàn tất; GPA vẫn chờ xác nhận.

Bước tiếp theo: phần ứng tuyển của Student, kiểm tra CV/đơn trùng/hạn/chỉ tiêu và lịch sử; quy trình doanh nghiệp duyệt/tạo thực tập tiếp tục theo giai đoạn Company và nghiệp vụ chung.
