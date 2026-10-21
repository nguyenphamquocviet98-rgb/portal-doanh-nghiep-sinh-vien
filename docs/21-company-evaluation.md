# Company — đánh giá sinh viên

Ngày thực hiện: 09/10/2026.

## Kết quả

Từ hồ sơ thực tập, chọn **Đánh giá sinh viên** để mở `/Company/Internships/Evaluation/{id}`. Chỉ tạo đánh giá khi hồ sơ Hoàn thành hoặc Không hoàn thành. Hồ sơ đã có đánh giá hiển thị kết quả; mốc này không có chức năng sửa hoặc xóa đánh giá đã gửi.

Sáu tiêu chí theo mục 19 PROJECT_REQUIREMENTS: kiến thức chuyên môn, kỹ năng làm việc, thái độ, tinh thần trách nhiệm, khả năng giao tiếp, mức độ hoàn thành công việc. Mỗi điểm bắt buộc, từ 0–10, tối đa hai chữ số thập phân theo decimal(4,2) hiện có. Nhận xét bắt buộc, tối đa 2990 ký tự. Điểm trung bình tự tính từ sáu điểm, không nhận từ form.

Điểm từ ô HTML number được nhận theo dấu chấm thập phân, độc lập thiết lập vùng trên máy chủ. Binder chỉ áp dụng cho sáu trường điểm đánh giá. Không tự làm tròn điểm quá hai chữ số thập phân.

GET/POST kiểm tra Company đang hoạt động/đã duyệt, role và quyền sở hữu; POST yêu cầu CSRF. Service kiểm tra lại quyền, trạng thái hồ sơ và việc đã có đánh giá trong transaction Serializable. Unique InternshipId của Evaluations tiếp tục chống đánh giá trùng. Đánh giá và thông báo NewEvaluation lưu trong cùng transaction; nếu một phần lỗi, cả hai được hoàn tác. Thông báo có khóa `evaluation:{id}` và liên kết về `/Student/Internships/Evaluation/{id}`. Danh sách thông báo chưa triển khai ở mốc này.

Không đổi schema hoặc migration. Student dùng chức năng xem đánh giá hiện có; Company dùng lại nội dung kết quả đó.

## File tạo/sửa

- `InternshipPortal/ViewModels/CompanyInternships/CompanyEvaluationViewModel.cs`: sáu điểm, nhận xét và validation.
- `InternshipPortal/Helpers/EvaluationScoreModelBinder.cs`: nhận điểm thập phân HTML.
- `InternshipPortal/Services/CompanyInternshipService.cs`: đọc/tạo đánh giá và thông báo.
- `InternshipPortal/Areas/Company/Controllers/InternshipsController.cs`: GET/POST Evaluation.
- `InternshipPortal/Areas/Company/Views/Internships/Evaluation.cshtml`: nhập hoặc xem kết quả.
- `InternshipPortal/Areas/Company/Views/Internships/Details.cshtml`: liên kết đánh giá.

## Kiểm tra thực tế

Build đạt 0 lỗi, 0 cảnh báo. App chạy trên localhost:5090 với SQL Server local.

Script `.preview/check-company-evaluation.py` đã kiểm tra:

- Đăng nhập, role Student/Company chờ duyệt, GET/POST hồ sơ ngoài quyền hoặc không tồn tại, CSRF.
- Cả Chưa bắt đầu và Đang thực tập đều không được tạo đánh giá.
- Mỗi tiêu chí: thiếu điểm, số âm, quá 10, quá hai chữ số thập phân, dữ liệu không phải số bị chặn. Nhận xét trống/khoảng trắng/quá dài bị chặn.
- Điểm 0, 10, 8.25, 7.75, 6, 9 lưu đúng; trung bình 41/6 hiển thị đúng phía Company và Student. Nhận xét chứa HTML được mã hóa khi hiển thị.
- Cố tình tạo xung đột khóa thông báo: đánh giá không được lưu, chứng minh rollback.
- Hai yêu cầu tạo đồng thời: chỉ một đánh giá và một thông báo. Gửi lại không ghi đè điểm.
- Tạo đánh giá cho hồ sơ Không hoàn thành thành công.
- Xem đánh giá có sẵn và cache no-store phía Company.

Dữ liệu thử được tạo trên hồ sơ demo chưa có đánh giá, đã xóa riêng đánh giá/thông báo thử và khôi phục trạng thái ban đầu trong finally; các đánh giá demo có sẵn giữ nguyên.

Kiểm tra lại danh sách/chi tiết/nhật ký Company và cập nhật người hướng dẫn/trạng thái trên bản mới 5090: đạt. Chưa kiểm tra bố cục bằng trình duyệt thật/điện thoại hoặc tải lớn.

## Chạy và demo

```powershell
dotnet run --project InternshipPortal
```

Nếu app cũ đang dùng cổng mặc định, dừng terminal cũ trước khi chạy lại. Bản kiểm tra mốc này đang tại `http://localhost:5090/Company/Internships`.

1. Đăng nhập `company@demo.vn`, mở hồ sơ đã kết thúc và chọn đánh giá để xem dữ liệu mẫu.
2. Với hồ sơ chưa kết thúc, trang đánh giá phải báo chưa được đánh giá.
3. Muốn thử tạo mới, chuyển một hồ sơ chưa có đánh giá qua Đang thực tập rồi Hoàn thành/Không hoàn thành; nhập sáu điểm và nhận xét, gửi.
4. Đăng nhập Student tương ứng, mở kết quả đánh giá để xem sáu điểm, nhận xét và trung bình.
5. Tải lại hoặc gửi lại form cũ: không tạo đánh giá thứ hai.

Các thao tác demo tạo/cập nhật thực tế sẽ thay đổi dữ liệu trên máy. Nếu hồ sơ đã có đánh giá, trang chỉ hiển thị kết quả. Khi báo xung đột, tải lại để kiểm tra đã có đánh giá trước khi thử lại.

## TRẠNG THÁI DỰ ÁN

Đã hoàn thành: các mốc Student và Company trước đó; tạo/xem đánh giá Company, tự tính trung bình và lưu thông báo đánh giá mới.

Đang thực hiện: dừng tại mốc đánh giá sinh viên phía Company.

Bước tiếp theo: giai đoạn Admin, bắt đầu duyệt doanh nghiệp. Chờ người dùng nói “tiếp tục”.
