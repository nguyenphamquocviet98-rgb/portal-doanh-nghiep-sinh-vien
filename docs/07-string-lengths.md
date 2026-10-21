# Bổ sung quy tắc độ dài chuỗi SQL

## Yêu cầu

Mỗi bảng nghiệp vụ liên kết ít nhất một bảng khác. Không dùng varchar(max)/nvarchar(max); độ dài từ 220 trở lên giảm 10 một lần. Tên file từ 255 đổi thành 200 theo ví dụ người dùng.

Giữ nvarchar để lưu đầy đủ tiếng Việt. Không đổi tên Entity, số lượng bảng, role hoặc quan hệ.

## Thay đổi

| Nhóm | Giới hạn mới |
|---|---:|
| Tên file | 200 |
| Email, tên đăng nhập Identity | 246 |
| Địa chỉ, website | 290 |
| Khóa Identity và FK tương ứng | 440 |
| Đường dẫn CV, tài liệu, avatar | 490 |
| Kỹ năng, ghi chú duyệt doanh nghiệp | 990 |
| Giới thiệu, ghi chú, lời ứng tuyển, yêu cầu | 1990 |
| Mô tả vị trí, nhật ký, kết quả, nhận xét | 2990 |
| Password hash và stamp Identity | 200 |
| Số điện thoại | 30 |
| Claim value và token value | 1990 |

Các cột vốn ngắn hơn 220 như mã 20/30, tên 150/200 giữ giới hạn cũ.

Model annotations và cấu hình Identity nằm trong ApplicationDbContext được cập nhật đồng bộ. Không sửa mã nguồn SDK Identity.

## Migration

Thêm BoundStringLengths, giữ InitialCreate để bảo toàn lịch sử. Những chữ max trong Migration cũ hoặc oldType là mô tả schema trước đây, không phải kiểu cột hiện tại.

Migration kiểm tra độ dài thực tế của 55 cột trước khi sửa; nếu dữ liệu quá dài, báo tên bảng/cột và dừng. Không cắt dữ liệu. Các khóa Identity được tháo ràng buộc tạm thời, đổi độ dài, rồi tạo lại FK/PK đúng quan hệ trong transaction.

## Kết quả kiểm tra

- Build: 0 warning, 0 error.
- Migration áp dụng thành công; Model không còn thay đổi chưa tạo Migration.
- Đúng 12 bảng nghiệp vụ, 22 FK toàn ứng dụng; không có bảng nghiệp vụ đứng riêng.
- 0 cột varchar(max)/nvarchar(max) trong sys.tables của ứng dụng, bao gồm Identity.
- Các bảng hệ thống SQL Server có thể có max và không được chỉnh sửa.
- Dữ liệu mẫu giữ nguyên: 20 sinh viên, 6 doanh nghiệp, 18 vị trí, 21 đơn, 8 hồ sơ.
- 7 kiểm tra ràng buộc database vẫn đạt; chạy seed lại không nhân đôi.

## Trạng thái

Đã hoàn thành bổ sung quy tắc database. Chưa commit phần này. Bước tiếp theo vẫn là giai đoạn 6: đăng nhập, đăng ký và phân quyền.
