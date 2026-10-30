-- ==========================================================================================
-- ĐỒ ÁN MÔN HỌC: CÔNG NGHỆ PORTAL
-- ĐỀ TÀI: CỔNG THÔNG TIN THỰC TẬP VÀ KẾT NỐI DOANH NGHIỆP
-- SINH VIÊN: NGUYỄN PHẠM QUỐC VIỆT - MSSV: 1250070030 - LỚP: 12_ĐH_TMĐT
--
-- QUY TẮC THIẾT KẾ CƠ SỞ DỮ LIỆU:
-- 1. Chuẩn hóa 3NF, cấu trúc tinh gọn (13 bảng cốt lõi, loại bỏ các bảng rác kỹ thuật).
-- 2. TUYỆT ĐỐI KHÔNG DÙNG KIỂU DỮ LIỆU MAX (Toàn bộ nvarchar/varchar đều giới hạn cụ thể).
-- 3. Quy tắc độ dài -5 đến -10 đơn vị: nvarchar(45), (95), (140), (190), (240), (490), (990), (1990), (2990).
-- 4. Ràng buộc toàn vẹn dữ liệu chặt chẽ (PK, FK, UNIQUE, CHECK, DEFAULT, COMPUTED COLUMN).
-- ==========================================================================================

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLyThucTap')
BEGIN
    CREATE DATABASE [QuanLyThucTap];
END
GO

USE [QuanLyThucTap];
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- ==========================================================================================
-- XÓA BẢNG CŨ THEO THỨ TỰ RÀNG BUỘC (NẾU ĐÃ TỒN TẠI) ĐỂ CHẠY LẠI KHÔNG BỊ XUNG ĐỘT
-- ==========================================================================================
IF OBJECT_ID('dbo.DanhGiaThucTap', 'U') IS NOT NULL DROP TABLE dbo.DanhGiaThucTap;
IF OBJECT_ID('dbo.TaiLieuThucTap', 'U') IS NOT NULL DROP TABLE dbo.TaiLieuThucTap;
IF OBJECT_ID('dbo.NhatKyThucTap', 'U') IS NOT NULL DROP TABLE dbo.NhatKyThucTap;
IF OBJECT_ID('dbo.HoSoThucTap', 'U') IS NOT NULL DROP TABLE dbo.HoSoThucTap;
IF OBJECT_ID('dbo.DonUngTuyen', 'U') IS NOT NULL DROP TABLE dbo.DonUngTuyen;
IF OBJECT_ID('dbo.ViTriThucTap', 'U') IS NOT NULL DROP TABLE dbo.ViTriThucTap;
IF OBJECT_ID('dbo.DoanhNghiep', 'U') IS NOT NULL DROP TABLE dbo.DoanhNghiep;
IF OBJECT_ID('dbo.SinhVien', 'U') IS NOT NULL DROP TABLE dbo.SinhVien;
IF OBJECT_ID('dbo.ThongBao', 'U') IS NOT NULL DROP TABLE dbo.ThongBao;
IF OBJECT_ID('dbo.DotThucTap', 'U') IS NOT NULL DROP TABLE dbo.DotThucTap;
IF OBJECT_ID('dbo.Nganh', 'U') IS NOT NULL DROP TABLE dbo.Nganh;
IF OBJECT_ID('dbo.Khoa', 'U') IS NOT NULL DROP TABLE dbo.Khoa;
IF OBJECT_ID('dbo.TaiKhoan', 'U') IS NOT NULL DROP TABLE dbo.TaiKhoan;
GO

-- ==========================================================================================
-- 1. BẢNG TÀI KHOẢN (NGƯỜI DÙNG HỆ THỐNG: NHÀ TRƯỜNG, DOANH NGHIỆP, SINH VIÊN)
-- ==========================================================================================
CREATE TABLE dbo.TaiKhoan (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(190) NOT NULL UNIQUE,
    MatKhauHash VARCHAR(195) NOT NULL,
    HoTen NVARCHAR(95) NOT NULL,
    SoDienThoai VARCHAR(15) NULL,
    VaiTro NVARCHAR(20) NOT NULL CHECK (VaiTro IN (N'Admin', N'Company', N'Student')),
    AnhDaiDien NVARCHAR(490) NULL,
    TrangThai BIT NOT NULL DEFAULT 1, -- 1: Đang hoạt động, 0: Khóa
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME NULL
);
GO

-- ==========================================================================================
-- 2. BẢNG KHOA
-- ==========================================================================================
CREATE TABLE dbo.Khoa (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaKhoa VARCHAR(18) NOT NULL UNIQUE,
    TenKhoa NVARCHAR(140) NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- ==========================================================================================
-- 3. BẢNG NGÀNH ĐÀO TẠO
-- ==========================================================================================
CREATE TABLE dbo.Nganh (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    KhoaId INT NOT NULL,
    MaNganh VARCHAR(18) NOT NULL UNIQUE,
    TenNganh NVARCHAR(140) NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Nganh_Khoa FOREIGN KEY (KhoaId) REFERENCES dbo.Khoa(Id)
);
GO

-- ==========================================================================================
-- 4. BẢNG ĐỢT THỰC TẬP
-- ==========================================================================================
CREATE TABLE dbo.DotThucTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MaDot VARCHAR(28) NOT NULL UNIQUE,
    TenDot NVARCHAR(140) NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    HanNopHoSo DATE NOT NULL,
    TrangThai INT NOT NULL DEFAULT 1 CHECK (TrangThai IN (0, 1, 2)), -- 0: Sắp mở, 1: Đang mở, 2: Đã đóng
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT CK_DotThucTap_Ngay CHECK (NgayBatDau <= NgayKetThuc AND HanNopHoSo <= NgayKetThuc)
);
GO

-- ==========================================================================================
-- 5. BẢNG HỒ SƠ SINH VIÊN
-- ==========================================================================================
CREATE TABLE dbo.SinhVien (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaiKhoanId INT NOT NULL UNIQUE,
    NganhId INT NOT NULL,
    MaSinhVien VARCHAR(18) NOT NULL UNIQUE,
    Lop VARCHAR(28) NULL,
    NgaySinh DATE NULL,
    GioiTinh NVARCHAR(10) NULL CHECK (GioiTinh IN (N'Nam', N'Nữ', N'Khác')),
    DiemGPA DECIMAL(3,2) NULL CHECK (DiemGPA >= 0 AND DiemGPA <= 4.00),
    DiaChi NVARCHAR(240) NULL,
    KyNang NVARCHAR(990) NULL,
    MucTieuNgheNghiep NVARCHAR(990) NULL,
    DuongDanCV NVARCHAR(490) NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SinhVien_TaiKhoan FOREIGN KEY (TaiKhoanId) REFERENCES dbo.TaiKhoan(Id) ON DELETE CASCADE,
    CONSTRAINT FK_SinhVien_Nganh FOREIGN KEY (NganhId) REFERENCES dbo.Nganh(Id)
);
GO

-- ==========================================================================================
-- 6. BẢNG HỒ SƠ DOANH NGHIỆP
-- ==========================================================================================
CREATE TABLE dbo.DoanhNghiep (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaiKhoanId INT NOT NULL UNIQUE,
    MaSoThue VARCHAR(28) NOT NULL UNIQUE,
    TenDoanhNghiep NVARCHAR(190) NOT NULL,
    Website VARCHAR(190) NULL,
    DiaChi NVARCHAR(240) NOT NULL,
    NguoiLienHe NVARCHAR(95) NULL,
    SdtLienHe VARCHAR(15) NULL,
    LogoUrl NVARCHAR(490) NULL,
    GioiThieu NVARCHAR(2990) NULL,
    TrangThaiDuyet INT NOT NULL DEFAULT 0 CHECK (TrangThaiDuyet IN (0, 1, 2)), -- 0: Chờ duyệt, 1: Đã duyệt, 2: Từ chối
    GhiChuDuyet NVARCHAR(990) NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DoanhNghiep_TaiKhoan FOREIGN KEY (TaiKhoanId) REFERENCES dbo.TaiKhoan(Id) ON DELETE CASCADE
);
GO

-- ==========================================================================================
-- 7. BẢNG VỊ TRÍ THỰC TẬP TUYỂN DỤNG
-- ==========================================================================================
CREATE TABLE dbo.ViTriThucTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DoanhNghiepId INT NOT NULL,
    DotThucTapId INT NOT NULL,
    NganhId INT NOT NULL,
    MaViTri VARCHAR(28) NOT NULL UNIQUE,
    TieuDe NVARCHAR(190) NOT NULL,
    DiaDiem NVARCHAR(190) NOT NULL,
    HinhThucLamViec INT NOT NULL DEFAULT 0 CHECK (HinhThucLamViec IN (0, 1, 2)), -- 0: Tại văn phòng, 1: Từ xa, 2: Kết hợp
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    MoTaCongViec NVARCHAR(2990) NOT NULL,
    YeuCauUngVien NVARCHAR(1990) NOT NULL,
    QuyenLoi NVARCHAR(1990) NULL,
    MucLuongThoaThuan NVARCHAR(95) NULL,
    HanNopHoSo DATETIME NOT NULL,
    TrangThai INT NOT NULL DEFAULT 1 CHECK (TrangThai IN (0, 1)), -- 1: Đang mở, 0: Đã đóng
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ViTri_DoanhNghiep FOREIGN KEY (DoanhNghiepId) REFERENCES dbo.DoanhNghiep(Id),
    CONSTRAINT FK_ViTri_DotThucTap FOREIGN KEY (DotThucTapId) REFERENCES dbo.DotThucTap(Id),
    CONSTRAINT FK_ViTri_Nganh FOREIGN KEY (NganhId) REFERENCES dbo.Nganh(Id)
);
GO

-- ==========================================================================================
-- 8. BẢNG ĐƠN ỨNG TUYỂN
-- ==========================================================================================
CREATE TABLE dbo.DonUngTuyen (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SinhVienId INT NOT NULL,
    ViTriId INT NOT NULL,
    DuongDanCV NVARCHAR(490) NOT NULL,
    ThuGioiThieu NVARCHAR(1990) NULL,
    TrangThai INT NOT NULL DEFAULT 0 CHECK (TrangThai IN (0, 1, 2, 3)), -- 0: Chờ duyệt, 1: Phỏng vấn, 2: Tiếp nhận, 3: Từ chối
    GhiChuPhanHoi NVARCHAR(990) NULL,
    NgayNop DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Don_SinhVien FOREIGN KEY (SinhVienId) REFERENCES dbo.SinhVien(Id),
    CONSTRAINT FK_Don_ViTri FOREIGN KEY (ViTriId) REFERENCES dbo.ViTriThucTap(Id),
    CONSTRAINT UQ_SinhVien_ViTri UNIQUE (SinhVienId, ViTriId)
);
GO

-- ==========================================================================================
-- 9. BẢNG HỒ SƠ THỰC TẬP CHÍNH THỨC
-- ==========================================================================================
CREATE TABLE dbo.HoSoThucTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DonUngTuyenId INT NOT NULL UNIQUE,
    SinhVienId INT NOT NULL,
    DotThucTapId INT NOT NULL,
    NguoiHuongDan NVARCHAR(95) NULL,
    EmailHuongDan VARCHAR(190) NULL,
    SdtHuongDan VARCHAR(15) NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    TrangThai INT NOT NULL DEFAULT 1 CHECK (TrangThai IN (0, 1, 2, 3)), -- 0: Chưa bắt đầu, 1: Đang thực tập, 2: Hoàn thành, 3: Không đạt
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_HoSo_Don FOREIGN KEY (DonUngTuyenId) REFERENCES dbo.DonUngTuyen(Id),
    CONSTRAINT FK_HoSo_SinhVien FOREIGN KEY (SinhVienId) REFERENCES dbo.SinhVien(Id),
    CONSTRAINT FK_HoSo_Dot FOREIGN KEY (DotThucTapId) REFERENCES dbo.DotThucTap(Id),
    CONSTRAINT CK_HoSoThucTap_Ngay CHECK (NgayBatDau <= NgayKetThuc)
);
GO

-- ==========================================================================================
-- 10. BẢNG NHẬT KÝ THỰC TẬP THEO TUẦN
-- ==========================================================================================
CREATE TABLE dbo.NhatKyThucTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoSoThucTapId INT NOT NULL,
    TuanSo INT NOT NULL CHECK (TuanSo >= 1 AND TuanSo <= 52),
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    NoiDungCongViec NVARCHAR(2990) NOT NULL,
    KetQuaDatDuoc NVARCHAR(2990) NOT NULL,
    KhoKhan NVARCHAR(1990) NULL,
    GhiChu NVARCHAR(990) NULL,
    NgayNop DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_NhatKy_HoSo FOREIGN KEY (HoSoThucTapId) REFERENCES dbo.HoSoThucTap(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_NhatKy_Tuan UNIQUE (HoSoThucTapId, TuanSo)
);
GO

-- ==========================================================================================
-- 11. BẢNG TÀI LIỆU & BÁO CÁO THỰC TẬP
-- ==========================================================================================
CREATE TABLE dbo.TaiLieuThucTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoSoThucTapId INT NOT NULL,
    LoaiTaiLieu NVARCHAR(95) NOT NULL, -- Đề cương thực tập, Báo cáo tiến độ, Báo cáo tổng kết
    TenFile NVARCHAR(190) NOT NULL,
    DuongDanFile NVARCHAR(490) NOT NULL,
    KichThuocBytes INT NOT NULL,
    NgayNop DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TaiLieu_HoSo FOREIGN KEY (HoSoThucTapId) REFERENCES dbo.HoSoThucTap(Id) ON DELETE CASCADE
);
GO

-- ==========================================================================================
-- 12. BẢNG ĐÁNH GIÁ KẾT QUẢ THỰC TẬP (6 TIÊU CHÍ CHUẨN)
-- ==========================================================================================
CREATE TABLE dbo.DanhGiaThucTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoSoThucTapId INT NOT NULL UNIQUE,
    DiemChuyenMon DECIMAL(4,2) NOT NULL CHECK (DiemChuyenMon >= 0 AND DiemChuyenMon <= 10.00),
    DiemKyNang DECIMAL(4,2) NOT NULL CHECK (DiemKyNang >= 0 AND DiemKyNang <= 10.00),
    DiemThaiDo DECIMAL(4,2) NOT NULL CHECK (DiemThaiDo >= 0 AND DiemThaiDo <= 10.00),
    DiemTrachNhiem DECIMAL(4,2) NOT NULL CHECK (DiemTrachNhiem >= 0 AND DiemTrachNhiem <= 10.00),
    DiemGiaoTiep DECIMAL(4,2) NOT NULL CHECK (DiemGiaoTiep >= 0 AND DiemGiaoTiep <= 10.00),
    DiemHoanThanh DECIMAL(4,2) NOT NULL CHECK (DiemHoanThanh >= 0 AND DiemHoanThanh <= 10.00),
    -- Cột tính toán tự động điểm trung bình cộng 6 tiêu chí
    DiemTrungBinh AS CAST((DiemChuyenMon + DiemKyNang + DiemThaiDo + DiemTrachNhiem + DiemGiaoTiep + DiemHoanThanh) / 6.0 AS DECIMAL(4,2)),
    NhanXet NVARCHAR(2990) NOT NULL,
    NgayDanhGia DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_DanhGia_HoSo FOREIGN KEY (HoSoThucTapId) REFERENCES dbo.HoSoThucTap(Id) ON DELETE CASCADE
);
GO

-- ==========================================================================================
-- 13. BẢNG THÔNG BÁO HỆ THỐNG
-- ==========================================================================================
CREATE TABLE dbo.ThongBao (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaiKhoanId INT NOT NULL,
    TieuDe NVARCHAR(190) NOT NULL,
    NoiDung NVARCHAR(990) NOT NULL,
    LoaiThongBao NVARCHAR(45) NOT NULL,
    DuongDanLienKet NVARCHAR(490) NULL,
    DaDoc BIT NOT NULL DEFAULT 0,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ThongBao_TaiKhoan FOREIGN KEY (TaiKhoanId) REFERENCES dbo.TaiKhoan(Id) ON DELETE CASCADE
);
GO

-- ==========================================================================================
-- CÁC CHỈ MỤC TỐI ƯU TRUY VẤN (INDEXES)
-- ==========================================================================================
CREATE INDEX IX_SinhVien_MaSV ON dbo.SinhVien(MaSinhVien);
CREATE INDEX IX_DoanhNghiep_MST ON dbo.DoanhNghiep(MaSoThue);
CREATE INDEX IX_ViTri_Dot_TrangThai ON dbo.ViTriThucTap(DotThucTapId, TrangThai);
CREATE INDEX IX_DonUngTuyen_TrangThai ON dbo.DonUngTuyen(TrangThai);
CREATE INDEX IX_HoSoThucTap_TrangThai ON dbo.HoSoThucTap(TrangThai);
CREATE INDEX IX_ThongBao_TaiKhoan_DaDoc ON dbo.ThongBao(TaiKhoanId, DaDoc);
GO

-- ==========================================================================================
-- DỮ LIỆU MẪU BAN ĐẦU (SEED DATA TIẾNG VIỆT)
-- ==========================================================================================
BEGIN TRANSACTION;

-- 1. Nạp Tài khoản mẫu (Mật khẩu hash demo)
INSERT INTO dbo.TaiKhoan (Email, MatKhauHash, HoTen, SoDienThoai, VaiTro)
VALUES 
('admin@portal.edu.vn', 'AQAAAAIAAYagAAAAEPw==', N'Ban Quản trị Nhà trường', '02838940393', N'Admin'),
('contact@fptsoftware.com', 'AQAAAAIAAYagAAAAEPw==', N'FPT Software TP.HCM', '02873007575', N'Company'),
('tuyendung@vng.com.vn', 'AQAAAAIAAYagAAAAEPw==', N'VNG Corporation', '02839623888', N'Company'),
('nhansu@viettel.com.vn', 'AQAAAAIAAYagAAAAEPw==', N'Viettel Telecom Miền Nam', '02862961961', N'Company'),
('viet.nguyen@sinhvien.edu.vn', 'AQAAAAIAAYagAAAAEPw==', N'Nguyễn Phạm Quốc Việt', '0912345678', N'Student'),
('minh.anh@sinhvien.edu.vn', 'AQAAAAIAAYagAAAAEPw==', N'Trần Minh Anh', '0987654321', N'Student'),
('hoang.nam@sinhvien.edu.vn', 'AQAAAAIAAYagAAAAEPw==', N'Lê Hoàng Nam', '0903123456', N'Student');

-- 2. Nạp Khoa
INSERT INTO dbo.Khoa (MaKhoa, TenKhoa)
VALUES 
('CNTT', N'Khoa Công nghệ Thông tin'),
('KT', N'Khoa Kinh tế & Thương mại điện tử'),
('NN', N'Khoa Ngoại ngữ & Hợp tác quốc tế');

-- 3. Nạp Ngành đào tạo
INSERT INTO dbo.Nganh (KhoaId, MaNganh, TenNganh)
VALUES 
(1, 'KTPM', N'Kỹ thuật Phần mềm'),
(1, 'HTTT', N'Hệ thống Thông tin'),
(1, 'KHMT', N'Khoa học Máy tính'),
(2, 'TMDT', N'Thương mại Điện tử'),
(2, 'QTKD', N'Quản trị Kinh doanh'),
(3, 'NNA', N'Ngôn ngữ Anh Doanh nghiệp');

-- 4. Nạp Đợt thực tập
INSERT INTO dbo.DotThucTap (MaDot, TenDot, NgayBatDau, NgayKetThuc, HanNopHoSo, TrangThai)
VALUES 
('DOT-2026-HK1', N'Đợt thực tập Doanh nghiệp Học kỳ 1 (2025 - 2026)', '2026-09-01', '2026-12-30', '2026-09-20', 1),
('DOT-2026-HK2', N'Đợt thực tập Tốt nghiệp Học kỳ 2 (2025 - 2026)', '2027-02-15', '2027-06-15', '2027-02-28', 0);

-- 5. Nạp Hồ sơ Sinh viên
INSERT INTO dbo.SinhVien (TaiKhoanId, NganhId, MaSinhVien, Lop, NgaySinh, GioiTinh, DiemGPA, DiaChi, KyNang, MucTieuNgheNghiep, DuongDanCV)
VALUES 
(5, 4, '1250070030', '12_ĐH_TMĐT', '2004-05-15', N'Nam', 3.65, N'Quận 1, TP. Hồ Chí Minh', N'C#, ASP.NET Core, SQL Server, HTML5, CSS3, JavaScript, React', N'Trở thành Kỹ sư phát triển phần mềm Full-Stack chuyên sâu về Web Portal', '/uploads/cv/1250070030_CV.pdf'),
(6, 1, '1250070088', '12_ĐH_KTPM', '2004-08-20', N'Nữ', 3.42, N'Quận Bình Thạnh, TP. Hồ Chí Minh', N'Java Spring Boot, SQL, React, Git, RESTful API', N'Phát triển ứng dụng Web quy mô lớn và hệ thống phân tán', '/uploads/cv/1250070088_CV.pdf'),
(7, 2, '1250070099', '12_ĐH_HTTT', '2004-11-10', N'Nam', 3.20, N'TP. Thủ Đức, TP. Hồ Chí Minh', N'Data Analysis, PowerBI, SQL Server, Python, ERP', N'Chuyên viên phân tích hệ thống nghiệp vụ (Business Analyst)', '/uploads/cv/1250070099_CV.pdf');

-- 6. Nạp Hồ sơ Doanh nghiệp
INSERT INTO dbo.DoanhNghiep (TaiKhoanId, MaSoThue, TenDoanhNghiep, Website, DiaChi, NguoiLienHe, SdtLienHe, LogoUrl, GioiThieu, TrangThaiDuyet)
VALUES 
(2, '0101258672', N'Công ty Cổ phần Phần mềm FPT (FPT Software)', 'https://fpt-software.com', N'Khu Công Nghệ Cao, TP. Thủ Đức, TP.HCM', N'Nguyễn Thu Hằng', '02873007575', '/img/fpt-software.png', N'Doanh nghiệp công nghệ hàng đầu Việt Nam cung cấp dịch vụ CNTT toàn cầu.', 1),
(3, '0303885068', N'Công ty Cổ phần VNG (VNG Corporation)', 'https://vng.com.vn', N'VNG Campus, Khu chế xuất Tân Thuận, Quận 7, TP.HCM', N'Lê Minh Tuấn', '02839623888', '/img/vng.png', N'Kỳ lân công nghệ đầu tiên tại Việt Nam với các sản phẩm Zalo, Zing, VNG Games.', 1),
(4, '0100109106', N'Tổng Công ty Viễn thông Viettel', 'https://vietteltelecom.vn', N'285 Cách Mạng Tháng 8, Quận 10, TP.HCM', N'Phạm Quang Huy', '02862961961', '/img/viettel.png', N'Nhà mạng viễn thông và giải pháp số công nghệ hàng đầu khu vực.', 1);

-- 7. Nạp Vị trí tuyển dụng
INSERT INTO dbo.ViTriThucTap (DoanhNghiepId, DotThucTapId, NganhId, MaViTri, TieuDe, DiaDiem, HinhThucLamViec, SoLuong, MoTaCongViec, YeuCauUngVien, QuyenLoi, MucLuongThoaThuan, HanNopHoSo, TrangThai)
VALUES 
(1, 1, 1, 'FPT-NET-2026', N'Thực tập sinh Lập trình .NET / C# Web Portal', N'Khu Công nghệ Cao, TP. Thủ Đức', 0, 5, N'Tham gia phát triển hệ thống Enterprise Web Portal với nền tảng ASP.NET Core, EF Core và SQL Server.', N'Nắm vững OOP, kiến thức C#, cơ sở dữ liệu SQL Server. Tinh thần chủ động học hỏi.', N'Hỗ trợ phụ cấp 4 - 6 triệu/tháng, cơ hội ký hợp đồng chính thức sau kỳ thực tập.', N'4.000.000 - 6.000.000 VNĐ', '2026-10-30', 1),
(1, 1, 4, 'FPT-TMDT-2026', N'Thực tập sinh Vận hành & Phân tích Sàn TMĐT', N'Khu Công nghệ Cao, TP. Thủ Đức', 2, 3, N'Tham gia tối ưu trải nghiệm người dùng, phân tích hành vi khách hàng trên cổng thương mại điện tử.', N'Sinh viên ngành Thương mại điện tử hoặc Kinh tế. Thành thạo phân tích dữ liệu cơ bản.', N'Phụ cấp thực tập hàng tháng, làm việc trong môi trường đa quốc gia năng động.', N'Thỏa thuận', '2026-10-25', 1),
(2, 1, 1, 'VNG-FRONTEND-2026', N'Frontend Developer Intern (React / JavaScript)', N'VNG Campus, Quận 7', 0, 4, N'Thiết kế và xây dựng giao diện người dùng Responsive cho các cổng dịch vụ số hàng triệu người dùng.', N'Thành thạo HTML5, CSS3, JavaScript ES6+, tư duy thẩm mỹ UI/UX tốt.', N'Phụ cấp cạnh tranh, hỗ trợ ăn trưa tại Campus, phòng Gym hiện đại miễn phí.', N'5.000.000 - 7.000.000 VNĐ', '2026-10-31', 1),
(3, 1, 2, 'VIETTEL-BA-2026', N'Thực tập sinh Phân tích nghiệp vụ hệ thống (BA Intern)', N'Quận 10, TP.HCM', 0, 2, N'Khảo sát yêu cầu người dùng, viết tài liệu đặc tả chức năng và vẽ sơ đồ luồng dữ liệu Use Case.', N'Tư duy logic tốt, khả năng giao tiếp, tài liệu hóa hệ thống rõ ràng.', N'Trực tiếp tham gia dự án thực tế quy mô lớn của Viettel, có phụ cấp thực tập.', N'4.500.000 VNĐ', '2026-10-28', 1);

-- 8. Nạp Đơn ứng tuyển mẫu
INSERT INTO dbo.DonUngTuyen (SinhVienId, ViTriId, DuongDanCV, ThuGioiThieu, TrangThai, GhiChuPhanHoi)
VALUES 
(1, 1, '/uploads/cv/1250070030_CV.pdf', N'Em mong muốn được rèn luyện kỹ năng phát triển phần mềm ASP.NET Core tại FPT Software.', 2, N'Hồ sơ đạt yêu cầu, tiếp nhận vào đợt thực tập.'),
(2, 3, '/uploads/cv/1250070088_CV.pdf', N'Em có nền tảng tốt về React và mong muốn đóng góp cho các sản phẩm tại VNG.', 1, N'Mời phỏng vấn trực tiếp vào 09h00 sáng thứ Tư.'),
(3, 4, '/uploads/cv/1250070099_CV.pdf', N'Em yêu thích công việc phân tích hệ thống nghiệp vụ CNTT.', 0, NULL);

-- 9. Nạp Hồ sơ thực tập chính thức (Sinh viên Nguyễn Phạm Quốc Việt tại FPT Software)
INSERT INTO dbo.HoSoThucTap (DonUngTuyenId, SinhVienId, DotThucTapId, NguoiHuongDan, EmailHuongDan, SdtHuongDan, NgayBatDau, NgayKetThuc, TrangThai)
VALUES 
(1, 1, 1, N'Trần Văn Đức (Technical Lead)', 'duc.tran@fpt.com', '0918889999', '2026-09-01', '2026-11-30', 2); -- 2: Hoàn thành

-- 10. Nạp Nhật ký tuần mẫu
INSERT INTO dbo.NhatKyThucTap (HoSoThucTapId, TuanSo, NgayBatDau, NgayKetThuc, NoiDungCongViec, KetQuaDatDuoc, KhoKhan, GhiChu)
VALUES 
(1, 1, '2026-09-01', '2026-09-07', N'Tìm hiểu kiến trúc dự án Portal, cài đặt môi trường .NET 8, SQL Server và Git workflow.', N'Hoàn thành setup môi trường và chạy thử nghiệm thành công nhánh code chính.', N'Làm quen với quy chuẩn đặt tên biến và clean code của công ty.', N'Mentor hướng dẫn tận tình.'),
(1, 2, '2026-09-08', '2026-09-14', N'Thiết kế sơ đồ CSDL quan hệ, tạo các bảng danh mục và viết migration trong EF Core.', N'Hoàn thành 13 bảng cơ sở dữ liệu đúng chuẩn ràng buộc toàn vẹn.', NULL, N'Được Technical Lead khen cấu trúc bảng logic.'),
(1, 3, '2026-09-15', '2026-09-21', N'Xây dựng các chức năng CRUD cho phân hệ Quản trị và phân quyền bảo mật Identity.', N'API hoạt động trơn tru, xử lý xong các validation bắt lỗi nhập liệu.', NULL, N'Chuẩn bị nộp đề cương thực tập.');

-- 11. Nạp Tài liệu thực tập
INSERT INTO dbo.TaiLieuThucTap (HoSoThucTapId, LoaiTaiLieu, TenFile, DuongDanFile, KichThuocBytes)
VALUES 
(1, N'Đề cương thực tập tốt nghiệp', N'DeCuong_ThucTap_1250070030.pdf', '/uploads/docs/DeCuong_1250070030.pdf', 1048576),
(1, N'Báo cáo tiến độ giữa kỳ', N'BaoCao_TienDo_1250070030.docx', '/uploads/docs/BaoCao_1250070030.docx', 2097152);

-- 12. Nạp Đánh giá kết quả thực tập (6 tiêu chí)
INSERT INTO dbo.DanhGiaThucTap (HoSoThucTapId, DiemChuyenMon, DiemKyNang, DiemThaiDo, DiemTrachNhiem, DiemGiaoTiep, DiemHoanThanh, NhanXet)
VALUES 
(1, 9.50, 9.00, 10.00, 9.50, 9.00, 9.50, N'Sinh viên Nguyễn Phạm Quốc Việt có năng lực chuyên môn rất tốt, tư duy lập trình mạch lạc, tinh thần kỷ luật và thái độ làm việc xuất sắc. Dự án hoàn thành đúng tiến độ với chất lượng cao.');

-- 13. Nạp Thông báo mẫu
INSERT INTO dbo.ThongBao (TaiKhoanId, TieuDe, NoiDung, LoaiThongBao, DuongDanLienKet, DaDoc)
VALUES 
(5, N'Đơn ứng tuyển được tiếp nhận', N'Công ty FPT Software đã duyệt tiếp nhận bạn vào vị trí Thực tập sinh .NET.', N'UngTuyen', '/Student/Internships', 1),
(5, N'Có kết quả đánh giá thực tập', N'Doanh nghiệp đã hoàn thành đánh giá thực tập 6 tiêu chí cho hồ sơ của bạn.', N'DanhGia', '/Student/Internships/Evaluation', 0);

COMMIT TRANSACTION;
GO

-- ==========================================================================================
-- KIỂM TRA TOÀN BỘ CƠ SỞ DỮ LIỆU VỪA TẠO
-- ==========================================================================================
PRINT N'===> ĐÃ TẠO THÀNH CÔNG CSDL CỔNG THÔNG TIN THỰC TẬP (TIẾNG VIỆT, KHÔNG DÙNG MAX)!';
SELECT 
    t.name AS [TenBang],
    COUNT(c.column_id) AS [SoLuongCot]
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
GROUP BY t.name
ORDER BY t.name;
GO
