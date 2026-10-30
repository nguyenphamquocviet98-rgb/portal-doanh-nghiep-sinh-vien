namespace InternshipPortal.ViewModels.Demo;

public sealed class QuanLyThucTapDemoViewModel
{
    public int SoSinhVien { get; init; }
    public int SoDoanhNghiep { get; init; }
    public int SoViTri { get; init; }
    public int SoDonUngTuyen { get; init; }
    public string? LoiKetNoi { get; init; }
    public List<SinhVienDemoItem> SinhViens { get; } = [];
    public List<ViTriDemoItem> ViTris { get; } = [];
    public List<DonUngTuyenDemoItem> DonUngTuyens { get; } = [];
}

public sealed record SinhVienDemoItem(string MaSinhVien, string HoTen, string Nganh, string Lop, decimal? Gpa);

public sealed record ViTriDemoItem(string TieuDe, string DoanhNghiep, string Nganh, DateTime HanNop);

public sealed record DonUngTuyenDemoItem(string SinhVien, string ViTri, string TrangThai, DateTime NgayNop);
