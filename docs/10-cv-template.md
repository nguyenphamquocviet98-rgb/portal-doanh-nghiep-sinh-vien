# Mẫu CV hai cột theo ảnh tham khảo

## Mục tiêu

Thêm mẫu CV chuyên nghiệp theo bố cục người dùng gửi: ảnh và thông tin giới thiệu ở đầu; học vấn, dự án/kinh nghiệm ở cột trái; liên hệ, kỹ năng, chứng chỉ, người giới thiệu và sở thích ở cột phải. Điểm nhấn xanh đậm, nền trắng, chữ dễ đọc.

Đây là chức năng soạn mẫu và lưu PDF qua trình duyệt. Không thay định dạng hoặc nội dung PDF đã upload trước đó. Không thêm bảng, cột, role hoặc Migration. GPA vẫn chờ xác nhận riêng.

## File tạo/sửa

- InternshipPortal/Areas/Student/Controllers/ProfileController.cs: thêm GET Cv, lấy hồ sơ theo user đăng nhập.
- InternshipPortal/Areas/Student/Views/Profile/Cv.cshtml: form soạn và bản xem trước.
- InternshipPortal/Areas/Student/Views/Profile/Index.cshtml: liên kết Soạn CV theo mẫu hai cột.
- InternshipPortal/Views/Shared/_Layout.cshtml: hỗ trợ CSS riêng qua section Styles.
- InternshipPortal/wwwroot/css/cv-template.css: hai cột, bố cục A4, responsive và chế độ in.
- InternshipPortal/wwwroot/js/cv-template.js: cập nhật bản xem trước, ảnh cục bộ, ẩn mục trống và in.
- InternshipPortal/README.md: cách dùng.

## Cách dùng

1. Đăng nhập Student.
2. Mở http://localhost:5086/Student/Profile/Cv hoặc vào Hồ sơ rồi chọn Soạn CV theo mẫu hai cột.
3. Kiểm tra họ tên, email, ngành, kỹ năng và giới thiệu đã lấy từ hồ sơ.
4. Bổ sung vị trí ứng tuyển, tên trường, dự án/kinh nghiệm, chứng chỉ và liên hệ.
5. Chọn ảnh JPG/PNG nếu muốn. Ảnh chỉ dùng tại trang soạn, không upload vào server; tối đa 10 MB.
6. Chọn In / Lưu PDF. Trong hộp thoại trình duyệt chọn Lưu thành PDF, A4, tắt header/footer trình duyệt. Nếu màu nền chưa hiện, bật in đồ họa nền.
7. Lưu file rồi trở về Hồ sơ để upload PDF, giới hạn 10 MB.

Các nội dung soạn thêm chưa được lưu lâu dài trong database. Trang nhắc lưu PDF trước khi rời; thay đổi tại mẫu không tự sửa hồ sơ hoặc CV đã tải lên. Có cảnh báo khi rời trang sau chỉnh nội dung.

## Hành vi

- Role Student và quyền sở hữu lấy từ phiên; truyền studentId khác không đổi hồ sơ.
- Tự điền dữ liệu thật trong hồ sơ, không tự bịa kinh nghiệm/chứng chỉ.
- Mục tùy chọn trống được ẩn khỏi bản xem trước và bản in.
- Nội dung form đưa vào bản xem trước bằng textContent, không chạy HTML người dùng nhập.
- Không ảnh: hiển thị chữ viết tắt tên; có ảnh: căn ảnh chân dung trong khung.
- In chỉ có CV, không có form, menu hoặc footer website.
- Nội dung dài có thể cần nhiều trang; nên rút gọn để CV thực tập nằm trong một trang.

## Kiểm tra đã thực hiện

- Build 0 warning, 0 error.
- Chặn truy cập chưa đăng nhập hoặc sai role.
- GET Cv HTTP 200 cho Student, trả đúng tên chủ hồ sơ dù có studentId khác.
- Liên kết tại hồ sơ và CSS/JS tải thành công.
- JavaScript qua kiểm tra cú pháp.
- Đã xem ảnh trang soạn với dữ liệu nhập mẫu cục bộ; kinh nghiệm/chứng chỉ hiển thị, mục trống được ẩn.
- Edge đã xuất PDF A4 một trang, dung lượng khoảng 51 KB với nội dung kiểm tra.
- Đã xem bố cục theo CSS in: không còn menu/form/footer website. Chưa tự động thao tác hộp thoại in hoặc chọn ảnh của người dùng.
- EF không có thay đổi mô hình; không tạo Migration.

## Lỗi thường gặp

- Chưa đăng nhập: được chuyển về Login.
- Nội dung mới mất sau tải lại: cần lưu PDF trước khi rời trang; phần soạn thêm chưa tự lưu lâu dài.
- PDF có địa chỉ website/ngày ở đầu hoặc cuối: tắt tùy chọn header/footer của trình duyệt.
- CV qua hai trang: rút gọn mục tiêu, kinh nghiệm và phần phụ.
- Ảnh không hiện: dùng JPG/PNG hợp lệ dưới 10 MB.

## Trạng thái dự án

Đã hoàn thành: mẫu CV hai cột, bản xem trước và CSS in PDF.

Đang thực hiện: chờ người dùng thử mẫu; GPA chưa chốt.

Bước tiếp theo: tiếp tục phần Student theo lộ trình sau phản hồi; chưa commit các thay đổi mới.
