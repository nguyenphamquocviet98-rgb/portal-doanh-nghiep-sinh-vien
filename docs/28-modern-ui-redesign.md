# Thiết kế lại giao diện Cổng thông tin Thực tập Hiện đại — 16/10/2026

Theo yêu cầu và xác nhận từ người dùng, toàn bộ giao diện của hệ thống **Cổng thông tin Thực tập và Kết nối Doanh nghiệp** đã được thiết kế lại toàn diện theo chuẩn **Cổng thông tin Đại học hiện đại (Modern Academic University Design)** trên nền tảng **ASP.NET Core MVC (.NET 8)**.

---

## 1. Định hướng thẩm mỹ & Phong cách thiết kế mới

1. **Bảng màu chủ đạo (University Navy & Blue Palette)**:
   - **Xanh Navy Hoàng gia (`#1e3a8a`, `#0f172a`)**: Mang lại vẻ uy tín, trang trọng, chuẩn mực môi trường đại học.
   - **Xanh thương hiệu năng động (`#2563eb`, `#3b82f6`)**: Điểm nhấn hành động (buttons, links, active tabs).
   - **Nền sáng trang nhã (`#f8fafc`, `#ffffff`)**: Thay thế hoàn toàn tone nền kem báo chí cũ, mang lại cảm giác sạch sẽ, hiện đại và thân thiện cho sinh viên.
   - **Hệ màu trạng thái chuẩn**: 
     - Xanh lá (`#15803d` / nền `#dcfce7`) cho Đã duyệt, Đang tuyển, Hoàn thành.
     - Vàng cam (`#b45309` / nền `#fef3c7`) cho Chờ duyệt, Phỏng vấn.
     - Xanh dương (`#0369a1` / nền `#e0f2fe`) cho Đang thực tập, Xác thực.
     - Đỏ hồng (`#b91c1c` / nền `#fee2e2`) cho Từ chối, Hết hạn, Hủy đơn.

2. **Kiểu chữ & Typography**:
   - Sử dụng phông chữ **Plus Jakarta Sans / Inter** kết hợp hệ thống phông chữ hệ thống sắc nét.
   - Loại bỏ hoàn toàn phông Serif kiểu báo cũ, tiêu đề rõ ràng, ngắt dòng chuẩn tiếng Việt.

3. **Cấu trúc thẻ & Không gian (Card & Elevation)**:
   - Thẻ bo góc mềm mại (`border-radius: 12px – 16px – 24px`).
   - Đổ bóng nhẹ (`box-shadow: 0 4px 6px -1px rgba(15, 23, 42, 0.05)`).
   - Hiệu ứng hover nhấc thẻ nhẹ (`transform: translateY(-2px)`).

---

## 2. Các thành phần đã được nâng cấp

| File | Nội dung nâng cấp |
|---|---|
| `Frontend/wwwroot/css/site.css` | Viết lại toàn bộ hệ thống CSS tokens, utilities, cards, buttons, badges, tables, navigation và responsive. |
| `Frontend/Views/Shared/_Layout.cshtml` | Thanh điều hướng kính mờ (frosted glass navbar), biểu tượng huy hiệu đại học SVG (Crest), menu tài khoản có role badge, điều hướng theo vai trò và chân trang (footer) chuẩn trường ĐH. |
| `Frontend/Views/Home/Index.cshtml` | Trang chủ hiện đại: Hero banner kèm thanh tìm kiếm nhanh, thống kê 4 chỉ số nổi bật, 3 thẻ cổng thành viên (Sinh viên - Doanh nghiệp - Nhà trường) và quy trình thực tập 4 bước chuẩn hóa. |
| `Frontend/Views/Shared/_PositionList.cshtml` | Danh sách thẻ vị trí thực tập với biểu tượng doanh nghiệp, tag địa điểm, hình thức, chuyên ngành, số chỉ tiêu còn lại và hạn nộp nổi bật. |
| `Frontend/Views/Internships/Index.cshtml` | Trang tìm kiếm vị trí thực tập: Hộp bộ lọc đa tiêu chí chuyên nghiệp, đếm số kết quả, nút đặt lại bộ lọc và phân trang hiện đại. |
| `Frontend/Views/Internships/Details.cshtml` | Trang chi tiết vị trí bố cục 2 cột (mô tả, yêu cầu + bảng tóm tắt thời hạn & nút ứng tuyển nổi bật). |
| `Frontend/Views/Companies/Details.cshtml` | Trang hồ sơ doanh nghiệp công khai với huy hiệu xác nhận từ nhà trường và danh sách các vị trí đang mở tuyển. |
| `Frontend/Views/Account/Login.cshtml` | Giao diện đăng nhập hiện đại, có hướng dẫn chọn vai trò đăng ký và thông báo bảo mật. |
| `Frontend/Views/Account/Register.cshtml` | Trang chọn luồng đăng ký Sinh viên hoặc Doanh nghiệp với 2 thẻ trực quan lớn. |
| `Frontend/Views/Shared/_PortalHome.cshtml` | Thẻ chào mừng trên Dashboard cho các vai trò. |
| `Frontend/Views/Shared/_StudentNavigation.cshtml` | Thanh điều hướng tab chuyên biệt cho không gian Sinh viên (Bàn làm việc, Hồ sơ, CV mẫu, Đơn nộp, Thực tập). |
| `Frontend/Views/Shared/_AdminNavigation.cshtml` | Thanh điều hướng tab chuyên biệt cho Cổng Nhà trường (Tổng quan, Doanh nghiệp, Khoa/Ngành, Đợt thực tập, Sinh viên, Giám sát). |
| `Frontend/Areas/Company/Views/Shared/_CompanyNavigation.cshtml` | Thanh điều hướng tab chuẩn cho Cổng Doanh nghiệp. |
| `Frontend/Areas/Student/Views/Dashboard/Index.cshtml` | Bàn làm việc sinh viên với 4 thẻ chức năng nhanh trực quan. |
| `Frontend/Areas/Admin/Views/Dashboard/Index.cshtml` | Bàn làm việc quản trị nhà trường phân chia module rõ ràng. |
| `Frontend/Areas/Company/Views/Dashboard/Index.cshtml` | Bàn làm việc doanh nghiệp với 4 module tuyển dụng và quy trình 4 bước. |
| `Frontend/wwwroot/js/site.js` | Thêm hiệu ứng cuộn navbar mượt mà, tự động đóng menu mobile khi click anchor link. |

---

## 3. Kết quả kiểm tra kỹ thuật

1. **Build .NET**: Thành công, `0 Error(s), 0 Warning(s)`.
2. **Khởi động server**: Chạy thành công trên `http://localhost:5086`.
3. **Kiểm tra HTTP**:
   - `http://localhost:5086` -> HTTP 200.
   - `http://localhost:5086/Internships` -> HTTP 200.
   - `http://localhost:5086/Account/Login` -> HTTP 200.
   - `http://localhost:5086/Account/Register` -> HTTP 200.
   - `http://localhost:5086/Internships/Details/18` -> HTTP 200.
4. **Bảo toàn dữ liệu & nghiệp vụ**: Không thay đổi bất kỳ logic C#, Entity Framework Core hay Database schema nào.
