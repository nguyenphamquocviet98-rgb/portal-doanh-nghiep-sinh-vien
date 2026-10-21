# Hệ thống Thông báo (Notification) & Biểu đồ Thống kê Admin (Chart.js) — 16/10/2026

Tài liệu này ghi lại chi tiết kiến trúc, mã nguồn triển khai và kết quả kiểm thử của 2 tính năng quan trọng theo yêu cầu:
1. **Hệ thống Thông báo (Notification)**: Biểu tượng chuông trên Header, tự động tạo thông báo theo 4 sự kiện nghiệp vụ, trung tâm thông báo và bộ đếm chưa đọc.
2. **Biểu đồ thống kê Admin (Chart.js)**: Tích hợp thư viện biểu đồ vào Dashboard Quản trị trường học để trực quan hóa tỷ lệ tiếp nhận thực tập, số lượng đơn theo ngành và trạng thái thực tập.

---

## 1. Hệ thống Thông báo (Notification System)

### 1.1. Cấu trúc Dữ liệu & ViewModel

- **Model Thực thể (`Notification.cs`)**:
  - `Id`: Khóa chính.
  - `UserId`: Mã định danh người nhận thông báo (`AspNetUsers.Id`).
  - `Type`: Phân loại thông báo (`NotificationType`).
  - `Title`: Tiêu đề thông báo ngắn gọn.
  - `Message`: Nội dung chi tiết.
  - `TargetUrl`: Đường dẫn điều hướng khi người dùng nhấp vào thông báo.
  - `IsRead`: Trạng thái đã đọc / chưa đọc (mặc định `false`).
  - `CreatedAt`: Thời điểm phát sinh thông báo (UTC).

- **Loại thông báo (`NotificationType`)**:
  - `ApplicationApproved`: Doanh nghiệp duyệt đơn ứng tuyển (gửi sinh viên).
  - `ApplicationRejected`: Doanh nghiệp từ chối đơn ứng tuyển (gửi sinh viên).
  - `NewPosition`: Vị trí thực tập mới mở tuyển (gửi sinh viên theo ngành hoặc toàn trường).
  - `CompanyApproved`: Ban quản lý trường duyệt hồ sơ doanh nghiệp (gửi đại diện doanh nghiệp).
  - `NewEvaluation`: Doanh nghiệp hoàn thành phiếu đánh giá thực tập (gửi sinh viên).
  - `System`: Thông báo hệ thống chung.

- **ViewModels (`NotificationViewModels.cs`)**:
  - `NotificationItemViewModel`: Chứa thông tin hiển thị của từng thông báo, định dạng thời gian thân thiện (`TimeAgo`: "Vừa xong", "5 phút trước", "2 ngày trước").
  - `NotificationListViewModel`: Hỗ trợ phân trang, lọc theo trạng thái `OnlyUnread`, hiển thị tổng số và số lượng chưa đọc.

---

### 1.2. Tự động kích hoạt thông báo (Auto-generation Triggers)

Hệ thống tự động tạo và gửi thông báo theo đúng 4 kịch bản nghiệp vụ yêu cầu:

| STT | Nghiệp vụ | Service kích hoạt | Người nhận | Nội dung thông báo |
|---|---|---|---|---|
| 1 | **Doanh nghiệp duyệt đơn** | `CompanyApplicationService.cs` (`AcceptAsync`) | Sinh viên nộp đơn | "Đơn ứng tuyển được tiếp nhận: Vị trí {Position.Title} tại {Company.Name}" |
| 2 | **Doanh nghiệp từ chối đơn** | `CompanyApplicationService.cs` (`RejectAsync`) | Sinh viên nộp đơn | "Kết quả ứng tuyển: Đơn ứng tuyển vị trí {Position.Title} chưa phù hợp" |
| 3 | **Đăng vị trí thực tập mới** | `CompanyPositionService.cs` (`CreateAsync`) | Sinh viên cùng chuyên ngành | "Vị trí thực tập mới: {Position.Title} tại {Company.Name}" |
| 4 | **Admin duyệt tài khoản doanh nghiệp** | `AdminCompanyService.cs` (`ApproveAsync`) | Đại diện Doanh nghiệp | "Tài khoản doanh nghiệp đã được phê duyệt bởi Ban Quản lý Nhà trường" |
| 5 | **Có đánh giá mới** | `CompanyInternshipService.cs` (`CreateEvaluationAsync`) | Sinh viên thực tập | "Bạn có đánh giá thực tập mới từ {Company.Name} (Điểm số: {Score}/10)" |

---

### 1.3. Giao diện & Trải nghiệm Người dùng

1. **Header Navigation (`_Layout.cshtml`)**:
   - Biểu tượng quả chuông SVG sắc nét, sang trọng.
   - Huy hiệu đỏ đếm số lượng thông báo chưa đọc (`#notif-badge`), tự động ẩn khi không có thông báo chưa đọc và hiển thị `99+` nếu vượt quá 99.
   - Dropdown xem nhanh 5 thông báo mới nhất kèm thời gian và liên kết điều hướng trực tiếp.
   - Nút liên kết "Xem tất cả thông báo" mở toàn màn hình trung tâm thông báo.

2. **Trung tâm Thông báo (`Frontend/Views/Notifications/Index.cshtml`)**:
   - Thẻ tiêu đề lớn với số đếm trực quan.
   - Nút hành động nhanh "Đánh dấu tất cả đã đọc" (CSRF protected).
   - Bộ lọc tab: `Tất cả (n)` và `Chưa đọc (n)`.
   - Phân biệt trực quan qua màu sắc và biểu tượng của từng loại thông báo.
   - Hỗ trợ phân trang tự động nếu số thông báo vượt quá 15 bản ghi/trang.

3. **Cập nhật dữ liệu ngầm (`site.js`)**:
   - Sử dụng Fetch API nhẹ nhàng, an toàn:
     - `GET /Notifications/UnreadCount`: Cập nhật số đếm badge khi tải trang.
     - `GET /Notifications/Recent`: Tải danh sách xem nhanh khi người dùng click vào quả chuông.

---

## 2. Biểu đồ thống kê Admin (Chart.js)

### 2.1. Backend Tổng hợp Dữ liệu (`DashboardController.cs`)

Controller tổng hợp dữ liệu thực tế từ cơ sở dữ liệu và chuyển giao qua `AdminDashboardViewModel`:
- **Tổng quan chỉ số**: Tổng số sinh viên, doanh nghiệp đối tác, vị trí tuyển dụng, đơn ứng tuyển.
- **Tỷ lệ tiếp nhận thực tập (`Placement Rate`)**:
  - Sinh viên đã có nơi thực tập (`AcceptedStudentsCount`).
  - Sinh viên chưa có nơi thực tập (`NotAcceptedStudentsCount`).
- **Phân bố hồ sơ ứng tuyển theo ngành học (`Applications by Major`)**:
  - Nhóm theo mã ngành (`MajorCode`) và tên ngành (`MajorName`), đếm tổng số hồ sơ đã nộp.
- **Phân bố trạng thái thực tập (`Internship Status Distribution`)**:
  - Chưa bắt đầu (`Planned`).
  - Đang thực tập (`InProgress`).
  - Hoàn thành (`Completed`).
  - Không đạt (`Terminated`).

---

### 2.2. Tích hợp Thư viện Biểu đồ (`Frontend/wwwroot/lib/chart.js/chart.min.js`)

- Do môi trường dự án vận hành khép kín (offline-first, no external CDN), hệ thống đã xây dựng thư viện đồ họa HTML5 Canvas tương thích chuẩn API của Chart.js:
  - Hỗ trợ đầy đủ `type: 'doughnut'` và `type: 'bar'`.
  - Hỗ trợ cấu hình `colors`, `legend`, `tooltips`, `cutout` (donut inner radius).
  - Vẽ mượt mà trên Canvas độ phân giải cao (`devicePixelRatio`), hiển thị nhãn trục, vạch chia giá trị và chú thích màu chuẩn xác.

---

### 2.3. Bố cục Dashboard Quản trị (`Areas/Admin/Views/Dashboard/Index.cshtml`)

1. **Hàng chỉ số tổng quan (Metric Cards)**: 4 thẻ lớn hiển thị Sinh viên, Doanh nghiệp, Vị trí tuyển, Đơn ứng tuyển.
2. **Hàng trực quan hóa dữ liệu (Visual Analytics)**:
   - **Biểu đồ tròn 1 (Doughnut)**: *Tỷ lệ sinh viên có nơi thực tập* (Xanh lục `#16a34a` đại diện cho Đã tiếp nhận, Xám `#cbd5e1` cho Chưa tiếp nhận).
   - **Biểu đồ cột (Bar Chart)**: *Số lượng đơn ứng tuyển theo ngành* (Cột màu Navy `#2563eb`, trục tung hiển thị số lượng hồ sơ).
   - **Biểu đồ tròn 2 (Doughnut)**: *Phân bố trạng thái thực tập* (Vàng cam: Chưa bắt đầu, Xanh dương: Đang thực tập, Xanh lá: Hoàn thành, Đỏ: Không đạt).
3. **Khu vực quản lý nhanh**: Các khối truy cập danh mục đợt thực tập, duyệt doanh nghiệp, giám sát thực tập và cài đặt hệ thống.

---

## 3. Kết quả Kiểm thử & Nghiệm thu

| Hạng mục kiểm tra | Phương thức | Kết quả | Trạng thái |
|---|---|---|---|
| Build Solution | `dotnet build` | 0 Error(s), 0 Warning(s) | Đạt |
| Phục vụ file tĩnh Chart.js | `GET /lib/chart.js/chart.min.js` | HTTP 200 OK, JavaScript hợp lệ | Đạt |
| Biểu đồ Dashboard Admin | `GET /Admin/Dashboard` (Session Admin) | HTTP 200 OK, 3 canvas chart hiển thị | Đạt |
| Chuông thông báo trên Header | `GET /` (Session Authenticated) | Có `notif-dropdown-btn`, `notif-badge` | Đạt |
| Đếm thông báo chưa đọc | `GET /Notifications/UnreadCount` | HTTP 200 OK, JSON `{"unreadCount": n}` | Đạt |
| Danh sách thông báo gần đây | `GET /Notifications/Recent` | HTTP 200 OK, JSON items đầy đủ | Đạt |
| Trang trung tâm thông báo | `GET /Notifications` | HTTP 200 OK, giao diện phân loại rõ ràng | Đạt |
| Đánh dấu đã đọc | `POST /Notifications/MarkAsRead/{id}` | HTTP 200/302, unread giảm về 0 | Đạt |
| Đánh dấu tất cả đã đọc | `POST /Notifications/MarkAllAsRead` | HTTP 302, flash message thành công | Đạt |
