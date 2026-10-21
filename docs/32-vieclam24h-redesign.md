# Tài liệu Tái thiết kế Giao diện theo phong cách Vieclam24h & Hệ thống Logo

## 1. Mục tiêu và Định hướng Thiết kế
Giao diện Trang chủ (`/`) và Danh sách vị trí thực tập (`/Internships`) đã được tái thiết kế theo bố cục tiêu chuẩn của các nền tảng tuyển dụng hàng đầu Việt Nam (**Vieclam24h.vn**), kết hợp phong cách học thuật hiện đại:
- **Tông màu chủ đạo**: Xanh lá cây Forest & Emerald Green (`#064e3b`, `#059669`, `#10b981`, `#ecfdf5`).
- **Hệ thống nhận diện thương hiệu & Logo**:
  - Logo chính thức của Cổng Thông tin Thực tập (`/images/logo.svg`).
  - Hệ thống 6 logo thương hiệu cho các Doanh nghiệp đối tác (`/images/companies/company-{1..6}.svg`).
  - Banner cổ động sinh viên thực tập chất lượng cao (`/images/banner-student.jpg`).

---

## 2. Cấu trúc Bố cục Giao diện (Mô phỏng Vieclam24h)

### 2.1. Header & Thanh điều hướng
- **Logo Cổng thông tin**: Nằm nổi bật góc trái, biểu tượng mũ cử nhân & sao vàng trên nền gradient emerald, đi kèm slogan *"CỔNG THỰC TẬP — KẾT NỐI DOANH NGHIỆP"*.
- **Menu điều hướng**: Trang chủ, Vị trí thực tập, Doanh nghiệp đối tác, Biểu mẫu & Hướng dẫn, Quy chế.
- **Thanh tác vụ tài khoản**:
  - Nút **"Dành cho Nhà tuyển dụng"** (màu xanh emerald nổi bật, dẫn trực tiếp vào trang Đăng ký doanh nghiệp `/Account/RegisterCompany`).
  - Nút **"Đăng ký"** & **"Đăng nhập"** (hoặc menu tài khoản sinh viên/doanh nghiệp/admin nếu đã đăng nhập).

### 2.2. Khu vực Tìm kiếm & Thẻ Đăng nhập (Hero Section)
- **Tiêu đề số liệu thực tế**: *"670.000+ sinh viên đã kết nối thực tập thành công — Nhanh hơn. Chuẩn mực hơn."*
- **Khung tìm kiếm ngang đa trường**:
  - Ô nhập từ khóa vị trí (Software Engineer, Kế toán, Marketing, HR...).
  - Dropdown chọn Ngành đào tạo (CNTT, Kinh tế, Ngôn ngữ, Quản trị...).
  - Dropdown chọn Địa điểm (TP.HCM, Hà Nội, Đà Nẵng, Bình Dương...).
  - Nút bấm **"Tìm việc ngay"** lớn màu Emerald Green.
- **Quick Search Tags**: Thẻ gợi ý nhanh (*Thực tập IT, Kế toán sinh viên, Marketing part-time, Thực tập tại Bình Dương, Tuyển gấp không yêu cầu kinh nghiệm*).
- **Thẻ Đăng nhập nhanh bên phải (Login Suggestion Card)**:
  - Khuyến khích sinh viên: *"Đăng nhập để xem ngay vị trí thực tập phù hợp!"*.
  - Nút **"Đăng nhập ngay"** và nút **"Đăng nhập qua Google / Email trường"**.

### 2.3. Danh mục ngành nghề & Banner quảng bá
- **Cột trái (Category Sidebar)**: Danh sách ngành thực tập trọng điểm kèm icon (Bán hàng - Quản lý khách hàng, Marketing & Truyền thông, CNTT & Phần mềm, Tài chính - Ngân hàng, Kế toán - Kiểm toán, Ngôn ngữ & Dịch thuật...).
- **Cột phải (Promotional Banner)**: Banner hình ảnh sinh viên năng động kèm slogan *"Tìm đúng nơi thực tập — Tự tin bước vào doanh nghiệp"*.

### 2.4. Khối Vị trí Thực tập Tuyển Gấp (`🔥 Việc làm tuyển gấp`)
- Bố cục **lưới 3 cột** trực quan, tối ưu mật độ thông tin:
  - **Logo doanh nghiệp**: Khung vuông 52x52px bo góc tinh tế góc trái.
  - **Tiêu đề vị trí**: Link đậm, hover đổi màu emerald.
  - **Tên doanh nghiệp**: Có icon tòa nhà, dẫn sang trang hồ sơ doanh nghiệp.
  - **Phụ cấp thực tập**: Hiển thị dạng badge nổi bật (vd: `3.000.000 - 5.000.000 đ/tháng` hoặc `Thỏa thuận`).
  - **Huy hiệu "HOT"**: Biểu tượng ngọn lửa đỏ cam thu hút sự chú ý.
  - **Hạn ứng tuyển**: Đếm ngược trực quan (*Còn X ngày*).
  - **Nút Lưu tin**: Biểu tượng trái tim tiện lợi.

### 2.5. Doanh nghiệp Đối tác Tiêu biểu
- Lưới các doanh nghiệp đối tác với logo sắc nét, số lượng vị trí đang tuyển và ngành nghề hoạt động.

### 2.6. Quy trình 4 bước Thực tập Chuẩn Học thuật
1. Khảo sát & Ứng tuyển
2. Doanh nghiệp Phê duyệt
3. Báo cáo & Giảng viên Đánh giá
4. Hoàn tất & Công nhận Tín chỉ

---

## 3. Danh mục Tệp tin & Mã nguồn Cập nhật

| STT | Đường dẫn | Nội dung & Vai trò |
|---|---|---|
| 1 | `Frontend/wwwroot/images/logo.svg` | Vector logo chính thức cho Cổng thông tin |
| 2 | `Frontend/wwwroot/images/companies/company-{1..6}.svg` | Hệ thống 6 logo thương hiệu cho các công ty đối tác |
| 3 | `Frontend/wwwroot/images/banner-student.jpg` | Hình ảnh banner sinh viên chất lượng cao |
| 4 | `Frontend/Views/Home/Index.cshtml` | Cấu trúc trang chủ theo giao diện Vieclam24h có logo |
| 5 | `Frontend/Views/Shared/_Layout.cshtml` | Header tích hợp logo mới và nút Nhà tuyển dụng |
| 6 | `Frontend/Views/Shared/_PositionList.cshtml` | Tích hợp logo doanh nghiệp vào danh sách vị trí `/Internships` |
| 7 | `Backend/ViewModels/Internships/PositionCardViewModel.cs` | Bổ sung `LogoUrl`, `DaysRemaining`, `AllowanceText` |
| 8 | `Backend/ViewModels/Home/HomeIndexViewModel.cs` | Bổ sung `LogoUrl` cho doanh nghiệp đối tác |
| 9 | `Frontend/wwwroot/css/site.css` | CSS styling cho Vieclam24h layout, logo grid, search bar, card, navbar spacing & colors |

---

## 5. Tinh chỉnh Phân bổ Bố cục & Màu sắc Header (Navbar Spacing & Color Harmony)
Theo phản hồi về việc các khối màu bị dính chùm và lệch bố cục sang bên phải:
1. **Phân bổ không gian ("Giãn cách bố cục")**:
   - **Bên trái (Cạnh Logo)**: Đặt cụm Menu điều hướng (`Trang chủ`, `Vị trí thực tập`, `Doanh nghiệp đối tác`, `Quy trình & Biểu mẫu`) ngay cạnh Logo Cổng thông tin với khoảng cách thoáng đãng (`ms-lg-4`, `gap: 8px-12px`).
   - **Khoảng giữa**: Giữ khoảng thở tự nhiên (`me-auto`), giúp thanh điều hướng cân đối hai bên.
   - **Bên phải**: Đẩy cụm nút thao tác tài khoản (`Đăng nhập`, `Đăng ký`, `Nhà tuyển dụng`) về phía mép phải (`ms-auto`).
2. **Hài hòa màu sắc ("Phân bổ màu hợp lý")**:
   - **Menu Item Đang chọn (`is-active`)**: Bỏ khối nền xanh mint đóng hộp thô cứng (`#d1fae5`). Thay bằng thiết kế bo tròn nhẹ nhàng (`background: #ecfdf5; border: 1px solid #a7f3d0; color: #065f46; font-weight: 700`), tiệp màu với hệ thống nhận diện.
   - **Nút "Đăng nhập"**: Chuyển sang dạng text link thanh lịch (`color: #334155; font-weight: 600`), hover nhẹ nhàng.
   - **Nút "Đăng ký"**: Nút chính Emerald Green (`#059669`), bo tròn dạng viên thuốc (pill), đổ bóng nhẹ.
   - **Nút "Nhà tuyển dụng"**: Loại bỏ hoàn toàn khối đen xì (`btn-dark` `#1e293b`). Thay thế bằng khối badge cao cấp chuẩn nhận diện: Nền xanh nhạt `#ecfdf5`, viền xanh ngọc `#a7f3d0`, chữ xanh rêu `#065f46`, icon cặp tài liệu màu xanh ngọc.
   - Không còn hiện tượng 4 khối màu đối chọi nhau dồn cục vào một góc.

---

## 6. Kiểm tra & Vận hành
- Dự án biên dịch thành công 100% (`dotnet build` 0 Warning, 0 Error).
- Dev Server chạy tại: **`http://localhost:5086/`**.
- Mọi tài nguyên ảnh và SVG trả về mã `200 OK`.
