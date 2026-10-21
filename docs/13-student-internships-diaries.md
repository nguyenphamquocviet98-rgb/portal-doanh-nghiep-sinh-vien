# Giai đoạn 7.4 — Theo dõi thực tập và nhật ký tuần

## Mục tiêu và kết quả

Student xem hồ sơ thực tập của mình, trạng thái, ngày thực tập và người hướng dẫn. Có danh sách nhật ký, xem chi tiết, tạo và sửa nhật ký tuần theo quy tắc đã được người dùng xác nhận.

Không thêm bảng/cột/Migration. Không cho Student đổi trạng thái hồ sơ thực tập hoặc người hướng dẫn. Chưa có nộp tài liệu ở phần này.

## Quy tắc đã chốt

- Chỉ tạo/sửa nhật ký khi InternshipStatus.InProgress.
- NotStarted, Completed và Failed chỉ xem lịch sử.
- Tuần 1 từ ngày bắt đầu thực tập, 7 ngày mỗi tuần, tuần cuối kết thúc ở ngày cuối kỳ.
- Một nhật ký mỗi tuần cho một hồ sơ.
- Ngày của nhật ký do server tính từ tuần, không nhận ngày tùy ý từ form.
- Kiểm tra trạng thái và quyền sở hữu khi lưu, không chỉ kiểm tra ở form GET.

## File tạo/sửa

| File trong InternshipPortal | Mục đích |
|---|---|
| Services/StudentInternshipService.cs | Truy vấn theo chủ sở hữu, tính tuần/ngày, tạo/sửa trong transaction |
| ViewModels/Internships/StudentInternshipViewModel.cs | Hồ sơ và chi tiết nhật ký |
| ViewModels/Internships/DiaryFormViewModel.cs | Form với giới hạn chuỗi khớp SQL |
| Areas/Student/Controllers/InternshipsController.cs | Index, Details, Diary, CreateDiary, EditDiary |
| Areas/Student/Views/Internships/Index.cshtml | Các hồ sơ của Student |
| Areas/Student/Views/Internships/Details.cshtml | Hồ sơ, số nhật ký/tuần và người hướng dẫn |
| Areas/Student/Views/Internships/Diary.cshtml | Nội dung tuần |
| Areas/Student/Views/Internships/DiaryForm.cshtml | Form tạo/sửa và trường chỉ đọc khi không đủ trạng thái |
| wwwroot/js/diary-form.js | Xem trước ngày khi đổi số tuần |
| wwwroot/css/site.css | CSS đồng bộ giao diện |
| Areas/Student/Views/Dashboard/Index.cshtml | Liên kết Thực tập & nhật ký |
| Program.cs | Đăng ký Service |

## Cách dùng

App đang chạy tại http://localhost:5086. Đăng nhập student03@demo.edu.vn để thử hồ sơ Đang thực tập; student@demo.edu.vn có hồ sơ Chưa bắt đầu nên chỉ xem.

1. Vào Cổng của tôi, chọn Thực tập & nhật ký, hoặc mở /Student/Internships.
2. Chọn hồ sơ để xem trạng thái, thời gian và người hướng dẫn.
3. Khi Đang thực tập, chọn Viết nhật ký.
4. Hệ thống gợi ý tuần đầu còn thiếu. Chọn tuần trong khoảng hiển thị; ngày tự tính theo lịch Việt Nam.
5. Nhập công việc, kết quả, khó khăn/ghi chú tùy chọn; lưu.
6. Mở nhật ký để xem và chỉnh sửa khi hồ sơ còn Đang thực tập.

Để chạy lại, dừng terminal bằng Ctrl+C rồi từ thư mục gốc:

```powershell
dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http
```

Các URL:

- /Student/Internships
- /Student/Internships/Details/{id}
- /Student/Internships/Diary/{id}
- /Student/Internships/CreateDiary?internshipId={id}
- /Student/Internships/EditDiary/{id}

## Chi tiết xử lý

- Authorize Student trên Controller, AntiForgeryToken trên hai POST.
- Mọi truy vấn hồ sơ/nhật ký có điều kiện Student.UserId bằng user đang đăng nhập.
- Khi sửa, InternshipId lấy từ bản ghi thật; không cho form chuyển nhật ký sang hồ sơ khác.
- Nội dung công việc/kết quả bắt buộc, tối đa 2990; khó khăn tối đa 2990; ghi chú tối đa 1990.
- Tổng tuần = làm tròn lên số ngày thực tập tính cả hai đầu chia 7, theo ngày tại UTC+7.
- Ngày bắt đầu/kết thúc nhật ký lưu UTC tương ứng 00:00 giờ Việt Nam của ngày đó.
- Không biến số nhật ký thành phần trăm đánh giá; chỉ hiển thị số đã nộp và số tuần của kỳ.
- Serializable transaction giữ điều kiện trạng thái trong lúc lưu; unique index chống ghi trùng tuần. Tranh chấp/deadlock được xử lý thành thông báo kiểm tra lại/thử lại.
- Không xóa nhật ký. Nội dung đọc từ database được Razor mã hóa khi hiển thị.

## Checklist đã kiểm tra trực tiếp

- Build 0 warning, 0 error; model không thay đổi cần Migration.
- Student chỉ thấy hồ sơ của mình; Company/Admin/chưa đăng nhập không vào cổng Student.
- Hồ sơ/nhật ký người khác trả 404, kể cả gọi POST tạo/sửa.
- Student chưa có hồ sơ thấy trang trống rõ ràng.
- Công việc trống, quá giới hạn, tuần ngoài khoảng bị từ chối.
- POST thiếu anti-forgery token bị chặn.
- Khoảng 01/10–10/10 có 2 tuần: 01/10–07/10 và 08/10–10/10. Ngày giả từ form không có hiệu lực.
- Tuần đầu còn thiếu được gợi ý; một tuần không tạo được hai nhật ký, kể cả gửi đồng thời.
- Sửa đúng nhật ký, ghi UpdatedAt; ID hồ sơ/nhật ký giả từ form không chuyển dữ liệu sang người khác.
- NotStarted/Completed/Failed không cho tạo/sửa; vẫn đọc được lịch sử.
- Hồ sơ đổi trạng thái sau khi mở form: POST bị chặn, dữ liệu giữ nguyên.
- Đã xem ảnh desktop của hồ sơ và form; chưa kiểm tra trực quan trên điện thoại thực tế hoặc tự động thao tác số tuần trong trình duyệt. JavaScript qua kiểm tra cú pháp và đã render ngày mặc định trong ảnh.
- Fixture thử đã dọn; giữ 20 sinh viên, 21 đơn, 8 hồ sơ và 6 nhật ký mẫu.

## Lỗi thường gặp

- Không thấy nút viết/sửa: hồ sơ chưa ở trạng thái Đang thực tập.
- Tuần đã có nhật ký: mở bản hiện tại để sửa, không tạo thêm.
- Ngày không chỉnh trực tiếp: đổi số tuần; ngày được tính thống nhất từ lịch thực tập.
- Form đang mở nhưng lưu bị từ chối: trạng thái có thể vừa kết thúc; tải lại hồ sơ để xem.
- ID không mở được: chỉ được truy cập dữ liệu thuộc user hiện tại.

## Git checkpoint

Chưa commit thay đổi. Từ thư mục gốc dùng git status và git diff để kiểm tra. Có thể git add InternshipPortal docs PROJECT_REQUIREMENTS.md rồi commit với message feat: add student internship tracking and weekly diaries. Không tự push; Git không backup dữ liệu SQL.

## Trạng thái dự án

Đã hoàn thành: giai đoạn 7.4 — theo dõi thực tập và nhật ký tuần.

Đang thực hiện: giai đoạn Student chưa hoàn tất; GPA vẫn chờ xác nhận.

Bước tiếp theo: nộp tài liệu và xem đánh giá cho Student; cần chốt loại/dung lượng và thời điểm nộp tài liệu trước phần liên quan.
