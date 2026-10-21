# Giai đoạn 7.5 — Nộp tài liệu và xem đánh giá

## Phạm vi đã hoàn thành

Student nộp/tải tài liệu của hồ sơ thực tập mình và xem kết quả đánh giá. Giữ từng lần nộp riêng; không có xóa tài liệu hoặc sửa điểm. Đây là phần Student, chưa phải chức năng doanh nghiệp tạo đánh giá.

## Quy tắc đã xác nhận

- PDF/DOCX tối đa 10 MB mỗi file.
- InProgress, Completed và Failed được nộp; NotStarted chỉ xem.
- Tên gốc không quá 200 ký tự, chỉ dùng hiển thị; tên lưu GUID.
- File riêng tư dưới wwwroot/uploads/documents; database chỉ lưu đường dẫn/metadata.
- Không phục vụ trực tiếp uploads; Download kiểm tra Student và chủ sở hữu.
- Đánh giá có 6 tiêu chí từ 0–10; điểm trung bình tính từ các điểm, không nhận từ trình duyệt.

## File tạo/sửa

- Services/DocumentUploadService.cs: dung lượng, phần mở rộng, PDF/DOCX và tên GUID.
- Services/PdfUploadService.cs: dùng chung xử lý PDF cho CV/tài liệu, giữ quy tắc CV cũ.
- Services/StudentDocumentService.cs: quyền sở hữu/trạng thái, ghi metadata và tải file.
- ViewModels/Internships/StudentDocumentsViewModel.cs: trang danh sách và form upload.
- ViewModels/Internships/StudentEvaluationViewModel.cs: 6 tiêu chí và điểm trung bình chỉ đọc.
- Areas/Student/Controllers/DocumentsController.cs: Index, Upload POST, Download GET.
- Areas/Student/Controllers/InternshipsController.cs: thêm Evaluation GET.
- Services/StudentInternshipService.cs: đọc đánh giá theo chủ sở hữu và trạng thái kết thúc.
- Areas/Student/Views/Documents/Index.cshtml: tài liệu đã nộp và form.
- Areas/Student/Views/Internships/Evaluation.cshtml: kết quả/nhận xét hoặc trạng thái chưa có đánh giá.
- Areas/Student/Views/Internships/Details.cshtml: liên kết tài liệu/đánh giá.
- Program.cs, wwwroot/css/site.css, README.md: DI, giao diện và hướng dẫn.

Các đường dẫn trên tính từ InternshipPortal. Không thêm bảng, cột, package hay Migration.

## Kiểm tra file

- PDF: mở rộng, giới hạn, header và dấu EOF; không phân tích toàn bộ nội dung PDF.
- DOCX: chữ ký ZIP, các phần [Content_Types].xml và word/document.xml, loại Word và namespace. Không giải nén ra đường dẫn người dùng.
- Không nhận macro package; XML không đọc DTD/tài nguyên ngoài.
- Đã sửa trường hợp file giả DOCX quá ngắn gây lỗi seek trước đầu stream; giờ trả thông báo định dạng hợp lệ, không HTTP 500.
- Kiểm tra định dạng cơ bản, chưa có quét mã độc hoặc đọc toàn bộ ngữ nghĩa tài liệu.
- Nếu không lưu được metadata, file mới được dọn; giữ tài liệu cũ.

## Cách thử

1. Đăng nhập student03@demo.edu.vn.
2. Vào /Student/Internships, mở hồ sơ đang thực tập.
3. Chọn Báo cáo & tài liệu; chọn loại và file PDF/DOCX, nộp rồi tải lại.
4. Đăng nhập student05@demo.edu.vn để xem hồ sơ kết thúc, tài liệu mẫu và đánh giá.
5. Trong đánh giá, thấy sáu tiêu chí và điểm trung bình; không có form sửa điểm.

URL mẫu:

- /Student/Documents?internshipId=3
- /Student/Documents/Download/{id}
- /Student/Internships/Evaluation/5

Chạy lại từ thư mục gốc bằng dotnet run --project .\InternshipPortal\InternshipPortal.csproj --launch-profile http. Mật khẩu demo lưu ngoài repo trong User Secrets.

## Kiểm tra đã chạy trực tiếp

- Build cuối 0 warning, 0 error; model không có thay đổi cần Migration.
- PDF/DOCX hợp lệ lưu tên GUID, metadata từ file thật, tải lại đúng bytes và MIME.
- Chặn POST thiếu token; file giả, mở rộng sai, loại enum sai, tên quá 200 và file quá 10 MB.
- File đúng 10 MB được nhận, 10 MB + 1 byte bị chặn.
- NotStarted không được nộp; InProgress/Completed/Failed nộp được.
- Không xem/nộp/tải tài liệu thuộc người khác; đường dẫn trực tiếp uploads trả 404.
- Lịch sử giữ các lần nộp riêng.
- Đánh giá sáu tiêu chí, trung bình đúng 8,00 với dữ liệu mẫu; query AverageScore=999 không tác động.
- Student POST sửa điểm không có action (405); dữ liệu đánh giá giữ nguyên.
- Đánh giá người khác trả 404; hồ sơ chưa có đánh giá có thông báo rõ ràng.
- Kiểm tra lại CV cũ sau khi tách xử lý PDF dùng chung, toàn bộ kiểm tra profile/CV vẫn đạt.
- Fixture user/tài liệu/file đã dọn. Giữ 20 sinh viên, 6 doanh nghiệp, 18 vị trí, 21 đơn, 8 hồ sơ, 6 nhật ký, 4 tài liệu và 4 đánh giá.
- Chưa kiểm tra trực quan trang mới trên điện thoại thực tế; không tuyên bố kiểm tra giao diện đầy đủ.

## Trạng thái

Đã hoàn thành phần 7.5 đã viết và kiểm tra. Tạm dừng theo người dùng ngày 08/10/2026, không tự chuyển giai đoạn tiếp theo.

GPA vẫn cần xác nhận thang điểm. Phần Company/Admin, thông báo đầy đủ và báo cáo demo cuối cùng còn trong lộ trình. Các thay đổi mới chưa commit Git.
