# Cập nhật Bộ màu Hệ thống sang Xanh Lá Cây & Ngọc Lục Bảo (Modern Academic Emerald) — 16/10/2026

Theo yêu cầu điều chỉnh nhận diện giao diện từ người dùng: thay đổi tone màu chủ đạo từ xanh navy sang **Xanh lá cây & Ngọc lục bảo học thuật (Modern Academic Emerald & Forest Green)**.

---

## 1. Triết lý Thiết kế & Bảng màu Mới

Màu xanh lá cây tượng trưng cho sự sinh sôi, phát triển nghề nghiệp, năng lượng tươi trẻ của sinh viên và niềm tin kết nối bền vững giữa nhà trường và doanh nghiệp.

### Hệ thống Token CSS (`Frontend/wwwroot/css/site.css`)

```css
:root {
    /* Bảng màu chủ đạo - Xanh Lá Cây Học Thuật & Ngọc Lục Bảo / Academic Emerald */
    --primary-950: #022c22;  /* Xanh rừng sâu thẳm, dùng cho Chân trang (Footer) sang trọng */
    --primary-900: #064e3b;  /* Deep Emerald - Dartmouth Green học thuật */
    --primary-800: #065f46;  /* Forest Green - Button hover, tiêu đề nổi bật */
    --primary-700: #047857;  /* Deep Emerald - Link và nút nhấn chính */
    --primary-600: #059669;  /* Vibrant Emerald - Màu thương hiệu cốt lõi, Huy hiệu (Crest) */
    --primary-500: #10b981;  /* Bright Leaf - Đường viền tương tác, điểm nhấn biểu đồ */
    --primary-400: #34d399;  /* Mint Green - Hiệu ứng chuyển màu và liên kết chân trang */
    --primary-200: #a7f3d0;  /* Pastel Sage - Đường viền thẻ và pill */
    --primary-100: #d1fae5;  /* Soft Mint - Nền trạng thái đang chọn */
    --primary-50:  #ecfdf5;  /* Ultra Light Mint - Nền hover danh mục, thanh điều hướng */
    
    /* Màu thương hiệu & Điểm nhấn */
    --accent:       #059669;
    --accent-hover: #047857;
    --accent-gold:  #f59e0b; /* Vàng hổ phách kết hợp truyền thống với xanh lá cây */
    --accent-cyan:  #0d9488; /* Xanh ngọc bích */
    --border-focus: #10b981;
}
```

---

## 2. Các Thành phần Được Cập nhật

| Thành phần | Chi tiết nâng cấp sang tone Xanh Lá Cây |
|---|---|
| **Logo & Huy hiệu trường (`.brand-crest`)** | Gradient xanh lục bảo sang trọng `linear-gradient(135deg, #065f46 0%, #059669 100%)`, bóng đổ xanh rêu học thuật. |
| **Thanh điều hướng (`.site-header`)** | Active tab nền bạc hà nhạt `#d1fae5`, chữ xanh ngọc `#047857`, hover nhẹ nhàng `#ecfdf5`. |
| **Hero Banner & Nút tìm kiếm** | Tiêu đề chuyển màu nổi bật `linear-gradient(135deg, #047857 0%, #10b981 100%)`, thanh chỉ thị chuyển từ xanh lá `#059669` sang xanh bạc hà `#34d399`. |
| **Nút hành động (`.action-link`)** | Nút gradient ngọc lục bảo `linear-gradient(135deg, #065f46 0%, #059669 100%)` với hiệu ứng đổ bóng `rgba(6, 95, 70, 0.25)`. |
| **Thẻ minh họa (`.hero-art`)** | Viền mờ gradient xanh ngọc tinh tế `rgba(5, 150, 105, 0.3)`. |
| **Biểu đồ thống kê Admin (`Chart.js`)** | Biểu đồ tròn tỷ lệ thực tập dùng xanh lục bảo `#059669`, biểu đồ cột theo ngành hiển thị màu xanh lá tươi `#10b981`. |
| **Chân trang (`.site-footer`)** | Nền xanh rừng thẳm `#022c22` kết hợp viền `#064e3b` và liên kết hover màu bạc hà sáng `#34d399`. |
| **Huy hiệu sinh viên & Thông báo** | Badge sinh viên, thông báo vị trí mới và các thẻ chức năng chuyển sang màu mint & emerald. |

---

## 3. Kết quả Kiểm tra Kỹ thuật

1. **Biên dịch .NET**: `dotnet build` đạt `0 Error(s), 0 Warning(s)`.
2. **Khởi động Server**: Đang chạy daemon tại `http://localhost:5086`.
3. **Phục vụ CSS tĩnh**: Trả về `HTTP 200 OK`, xác nhận các biến token `--primary-*` và màu xanh lá cây đã có hiệu lực lập tức.
