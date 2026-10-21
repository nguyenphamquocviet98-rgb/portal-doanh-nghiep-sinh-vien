# Hệ thống Truy cập Nhanh phục vụ Demo & Báo cáo (1-Click Demo Switcher)

## 1. Giới thiệu & Mục tiêu
Để phục vụ quá trình báo cáo đồ án tốt nghiệp, thuyết trình và chấm điểm của Thầy/Cô, Hội đồng mà không cần phải gõ tay email, mật khẩu hoặc liên tục đăng xuất/đăng nhập lại, hệ thống đã được tích hợp bộ công cụ **Truy cập nhanh Demo 1-Chạm (1-Click Demo Quick Switcher)**.

---

## 2. Các điểm truy cập Demo trên giao diện

### 2.1. Thanh điều khiển Demo nổi toàn cục (Global Floating Demo Bar)
- **Vị trí**: Nằm cố định ở góc dưới bên phải màn hình trên **tất cả các trang** (`position: fixed; bottom: 24px; right: 24px`).
- **Thành phần**:
  - **Huy hiệu trạng thái**: Hiển thị rõ đang ở chế độ khách vãng lai hay đang đăng nhập vai trò nào (kèm chấm xanh đang hoạt động).
  - **3 Nút chuyển vai trò 1-chạm**:
    1. 👑 **Nhà trường (Admin)**: Chuyển sang `admin@demo.edu.vn` và mở ngay Dashboard Giám sát & Biểu đồ Chart.js (`/Admin`).
    2. 🏢 **Doanh nghiệp (Company)**: Chuyển sang `company@demo.vn` (Cty Công nghệ An Việt) và mở ngay Bàn làm việc Doanh nghiệp (`/Company`).
    3. 🎓 **Sinh viên (Student)**: Chuyển sang `student@demo.edu.vn` (SV Nguyễn Minh Anh) và mở ngay Bàn làm việc Sinh viên (`/Student`).
  - **Nút Đăng xuất nhanh**: Thoát phiên làm việc tức thì về chế độ khách.
  - **Liên kết đi nhanh**: Trang chủ, Danh sách vị trí, Trang Login, Bàn làm việc.
  - **Nút Thu gọn / Mở rộng (– / +)**: Cho phép thu nhỏ thành viên thuốc nhỏ `⚡ Demo 1-Chạm` để không che khuất nội dung trang khi trình chiếu (lưu trạng thái vào `localStorage`).

### 2.2. Khối Đăng nhập Demo tại Trang Login (`/Account/Login`)
- Đặt ngay phía trên form nhập liệu truyền thống.
- Cung cấp 3 thẻ chọn lớn tương ứng với 3 vai trò kèm email mẫu. Nhấp chuột vào bất kỳ thẻ nào sẽ đăng nhập ngay lập tức.
- Giữ nguyên form đăng nhập thủ công bên dưới cho các trường hợp kiểm thử nghiệp vụ thông thường.

### 2.3. Khối Đăng nhập Demo tại Hero Trang chủ (`/`)
- Tại thẻ tiện ích đăng nhập bên phải của Hero section, tích hợp 3 nút truy cập nhanh dạng pill (`👑 Admin`, `🏢 Doanh nghiệp`, `🎓 Sinh viên`), cho phép người xem bắt đầu trải nghiệm ngay từ trang chủ.

---

## 3. Kiến trúc Backend & Xử lý Xác thực

### 3.1. Endpoint `AccountController.QuickLogin`
```csharp
[AllowAnonymous, HttpGet, HttpPost]
public async Task<IActionResult> QuickLogin(string? role, string? returnUrl = null)
```
- **Cơ chế xác thực**:
  - Tiếp nhận tham số `role` (`admin`, `company`, `student`).
  - Tìm kiếm người dùng mẫu trong cơ sở dữ liệu (`UserManager.FindByEmailAsync`).
  - Đăng xuất phiên hiện tại và đăng nhập người dùng mục tiêu thông qua `SignInManager.SignInAsync(user, isPersistent: true)`.
  - Tự động điều hướng đến đúng Area tương ứng (`/Admin`, `/Company`, `/Student`) hoặc `returnUrl` chỉ định.
  - Hỗ trợ cả phương thức `GET` (nhấp link trực tiếp) và `POST`.

---

## 4. Bảng tổng hợp Tài khoản Demo

| Vai trò | Email Demo | Mật khẩu Seed | Dashboard / Trang mặc định | Nghiệp vụ tiêu biểu |
|---|---|---|---|---|
| **Quản trị viên** | `admin@demo.edu.vn` | `Seed:DemoPassword` | `/Admin` | Biểu đồ Chart.js, duyệt doanh nghiệp, quản lý đợt thực tập |
| **Doanh nghiệp** | `company@demo.vn` | `Seed:DemoPassword` | `/Company` | Đăng tin tuyển dụng, xem hồ sơ, duyệt/từ chối đơn, đánh giá SV |
| **Sinh viên** | `student@demo.edu.vn` | `Seed:DemoPassword` | `/Student` | Ứng tuyển, xem trạng thái hồ sơ, viết nhật ký thực tập |

---

## 5. Tệp tin đã triển khai

| STT | Tệp tin | Nội dung |
|---|---|---|
| 1 | `Backend/Controllers/AccountController.cs` | Bổ sung action `QuickLogin` |
| 2 | `Frontend/Views/Shared/_DemoQuickBar.cshtml` | Partial view thanh điều khiển nổi toàn cục |
| 3 | `Frontend/Views/Shared/_Layout.cshtml` | Nhúng `_DemoQuickBar` vào layout chính |
| 4 | `Frontend/Views/Account/Login.cshtml` | Tích hợp khối đăng nhập nhanh 1-chạm |
| 5 | `Frontend/Views/Home/Index.cshtml` | Tích hợp nút truy cập demo tại Hero widget |
| 6 | `Frontend/wwwroot/css/site.css` | Toàn bộ style cho Demo Quick Bar, animations, responsive |
