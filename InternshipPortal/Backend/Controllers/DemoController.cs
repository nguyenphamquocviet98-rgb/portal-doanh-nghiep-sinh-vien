using InternshipPortal.ViewModels.Demo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace InternshipPortal.Controllers;

[Route("demo")]
public class DemoController(IConfiguration configuration) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var connectionString = configuration.GetConnectionString("QuanLyThucTapConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            return View(new QuanLyThucTapDemoViewModel { LoiKetNoi = "Chưa cấu hình chuỗi kết nối QuanLyThucTapConnection." });

        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync(ct);

            var model = new QuanLyThucTapDemoViewModel();
            await using (var command = new SqlCommand("""
                SELECT
                    (SELECT COUNT(*) FROM dbo.SinhVien),
                    (SELECT COUNT(*) FROM dbo.DoanhNghiep),
                    (SELECT COUNT(*) FROM dbo.ViTriThucTap),
                    (SELECT COUNT(*) FROM dbo.DonUngTuyen);
                """, connection))
            await using (var reader = await command.ExecuteReaderAsync(ct))
            {
                await reader.ReadAsync(ct);
                model = new QuanLyThucTapDemoViewModel
                {
                    SoSinhVien = reader.GetInt32(0),
                    SoDoanhNghiep = reader.GetInt32(1),
                    SoViTri = reader.GetInt32(2),
                    SoDonUngTuyen = reader.GetInt32(3)
                };
            }

            await using (var command = new SqlCommand("""
                SELECT sv.MaSinhVien, tk.HoTen, n.TenNganh, sv.Lop, sv.GPA
                FROM dbo.SinhVien AS sv
                INNER JOIN dbo.TaiKhoan AS tk ON tk.Id = sv.TaiKhoanId
                INNER JOIN dbo.Nganh AS n ON n.Id = sv.NganhId
                ORDER BY sv.MaSinhVien;
                """, connection))
            await using (var reader = await command.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    model.SinhViens.Add(new SinhVienDemoItem(
                        reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                        reader.IsDBNull(4) ? null : reader.GetDecimal(4)));
                }
            }

            await using (var command = new SqlCommand("""
                SELECT vt.TieuDe, dn.TenDoanhNghiep, n.TenNganh, vt.HanNop
                FROM dbo.ViTriThucTap AS vt
                INNER JOIN dbo.DoanhNghiep AS dn ON dn.Id = vt.DoanhNghiepId
                INNER JOIN dbo.Nganh AS n ON n.Id = vt.NganhId
                ORDER BY vt.HanNop;
                """, connection))
            await using (var reader = await command.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    model.ViTris.Add(new ViTriDemoItem(
                        reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                }
            }

            await using (var command = new SqlCommand("""
                SELECT tk.HoTen, vt.TieuDe, d.TrangThai, d.NgayNop
                FROM dbo.DonUngTuyen AS d
                INNER JOIN dbo.SinhVien AS sv ON sv.Id = d.SinhVienId
                INNER JOIN dbo.TaiKhoan AS tk ON tk.Id = sv.TaiKhoanId
                INNER JOIN dbo.ViTriThucTap AS vt ON vt.Id = d.ViTriId
                ORDER BY d.NgayNop DESC;
                """, connection))
            await using (var reader = await command.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
                {
                    model.DonUngTuyens.Add(new DonUngTuyenDemoItem(
                        reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3)));
                }
            }

            return View(model);
        }
        catch (SqlException)
        {
            return View(new QuanLyThucTapDemoViewModel
            {
                LoiKetNoi = "Không thể đọc dữ liệu từ database QuanLyThucTap. Hãy kiểm tra SQL Server và chạy lại script tạo dữ liệu."
            });
        }
    }
}
