# Tái cấu trúc Cổng thông tin theo Tiêu chuẩn Người thật & Bố cục Thực tế — 16/10/2026

Tài liệu này ghi lại chi tiết các thay đổi nhằm loại bỏ hoàn toàn cảm giác "AI tạo" (AI template/cliché/buzzwords) và đưa hệ thống về đúng **tiêu chuẩn cổng thông tin trường đại học thực tế** mà sinh viên, doanh nghiệp và nhà trường Việt Nam thường sử dụng.

---

## 1. Phân tích các yếu tố "Bị AI quá" đã được loại bỏ

| Yếu tố "bị AI" trước đây | Vấn đề | Thay thế bằng tiêu chuẩn Người thật |
|---|---|---|
| Slogan triết lý rườm rà: *"Khởi đầu thực tế. Vươn tầm tương lai."*, *"Học hỏi · Kết nối · Trưởng thành"*, *"Tiếp tục hành trình."* | Rập khuôn theo văn phong máy móc, sáo rỗng, không đúng tính chất học vụ. | Tiêu đề trực diện, chuẩn mực: **"Cổng Thông tin Thực tập & Tuyển dụng Sinh viên"**, **"Đăng ký tài khoản hệ thống"**, **"Danh mục Vị trí Thực tập"**. |
| Thẻ đồ họa giả lập giao diện (`.hero-art`) với các thẻ card giả như *"Hệ thống 24/7"*, *"Nhật ký số - Theo dõi chuẩn"* | Template AI thường vẽ giao diện giả ở góc màn hình thay vì dữ liệu thật. | Thay bằng **Khối tìm kiếm việc làm thực tế đa tiêu chí** và **2 Cổng tiện ích nhanh cho Sinh viên & Doanh nghiệp**. |
| Trang chủ không có dữ liệu thật | Chỉ hiển thị chữ chung chung và các số liệu giả lập tĩnh. | Đưa dữ liệu thực từ cơ sở dữ liệu lên trang chủ: **6 Vị trí thực tập mới nhất**, **6 Doanh nghiệp đối tác tiêu biểu**, **Đợt thực tập đang diễn ra**. |
| Đánh số thứ tự "01 / 02 / 03 / 04" trước mọi tiêu đề | Phong cách concept AI Dribbble không phù hợp web thực tế. | Sử dụng badge học thuật chuẩn và các thẻ phân loại mục đích sử dụng rõ ràng. |

---

## 2. Kiến trúc Trang chủ Thực tế Mới

### 2.1. Backend Cung cấp Dữ liệu Thực (`HomeController.cs`)
Controller `HomeController.Index` đã được nâng cấp để nạp trực tiếp dữ liệu từ Database vào `HomeIndexViewModel`:
- **`LatestPositions`**: 6 vị trí tuyển dụng thực tập mới nhất đang mở nộp đơn từ các công ty đã được nhà trường kiểm duyệt.
- **`FeaturedCompanies`**: 6 doanh nghiệp đối tác tiêu biểu kèm địa chỉ, lĩnh vực và số lượng vị trí đang tuyển.
- **`CurrentPeriod`**: Kế hoạch đợt thực tập tốt nghiệp hiện tại (mã đợt, ngày bắt đầu, ngày kết thúc).
- **`MajorOptions` & `LocationOptions`**: Nạp danh sách chuyên ngành và địa phương phục vụ bộ lọc tìm kiếm.
- **Chỉ số vận hành thật**: Tổng số vị trí đang mở, số doanh nghiệp liên kết, số sinh viên đã tiếp nhận.

### 2.2. Bố cục Giao diện Người thật (`Views/Home/Index.cshtml`)
1. **Thanh thông báo học vụ (Academic Notice Bar)**: Hiển thị đợt thực tập hiện tại, mốc thời gian đăng ký và liên kết xem lịch trình học vụ.
2. **Khung tìm kiếm việc làm chuyên nghiệp (Search Widget)**:
   - Ô nhập từ khóa (chức danh, công nghệ, công ty).
   - Chọn chuyên ngành đào tạo (CNTT, Kinh tế, Ngoại ngữ...).
   - Chọn địa điểm thực tập (TP.HCM, Hà Nội, Đà Nẵng, Remote...).
   - Nút hành động "Tìm việc ngay" và các tag gợi ý từ khóa phổ biến.
3. **Hai cổng điều hướng trực tiếp**:
   - **Dành cho Sinh viên**: Tra cứu vị trí tuyển dụng, soạn CV chuẩn A4, nộp đơn trực tuyến, viết nhật ký tuần.
   - **Dành cho Doanh nghiệp**: Đăng ký đối tác, đăng tin tuyển dụng miễn phí, sàng lọc ứng viên, chấm điểm 6 tiêu chí.
4. **Bảng tin "Vị trí thực tập mới nhất"**: Nhúng trực tiếp danh sách thẻ việc làm thực tế từ database với thông tin hạn nộp hồ sơ, số chỉ tiêu còn lại và nút nộp đơn.
5. **Lưới "Doanh nghiệp hợp tác tiêu biểu"**: Hiển thị hồ sơ, trụ sở và số cơ hội việc làm của đối tác.
6. **Lịch trình thực tập 4 giai đoạn chuẩn học vụ**: Chuẩn bị hồ sơ → Ứng tuyển & Tiếp nhận → Thực tập & Viết nhật ký → Đánh giá 6 tiêu chí & Báo cáo.
7. **Khu vực Tải biểu mẫu & Hướng dẫn học vụ**: Cung cấp các mẫu giấy giới thiệu, phiếu tiếp nhận, đề cương báo cáo tốt nghiệp.

---

## 3. Đồng bộ các Trang Chức năng Khác

- **Trang Đăng nhập (`Views/Account/Login.cshtml`)**: Thay thế câu chào AI bằng hướng dẫn đăng nhập học vụ rõ ràng, có phân luồng đăng ký tài khoản.
- **Trang Đăng ký (`Views/Account/Register.cshtml`)**: Phân biệt thẻ Sinh viên và Doanh nghiệp theo đúng nghiệp vụ trường học.
- **Trang Đăng ký Sinh viên & Doanh nghiệp**: Bỏ toàn bộ slogan quảng cáo, tập trung vào form điền thông tin học vụ và mã số sinh viên / mã số thuế.
- **Trang Chi tiết Vị trí (`Views/Internships/Details.cshtml`)**: Sử dụng tiêu đề chuẩn việc làm ("Mô tả công việc", "Yêu cầu ứng viên") thay cho các câu hỏi cảm thán AI.
