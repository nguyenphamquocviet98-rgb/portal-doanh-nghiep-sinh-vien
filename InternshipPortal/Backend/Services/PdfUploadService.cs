using System.Text;

namespace InternshipPortal.Services;

public class PdfUploadService(IWebHostEnvironment environment)
{
    public const long MaxCvSize = 10 * 1024 * 1024;

    public Task<string> SaveCvAsync(IFormFile file, CancellationToken cancellationToken) => SavePdfAsync(file, "cv", "CV", cancellationToken);

    public Task<string> SaveDocumentPdfAsync(IFormFile file, CancellationToken cancellationToken) => SavePdfAsync(file, "documents", "Tài liệu", cancellationToken);

    private async Task<string> SavePdfAsync(IFormFile file, string folderName, string label, CancellationToken cancellationToken)
    {
        if (file.Length == 0) throw new InvalidOperationException($"Vui lòng chọn file {label} có nội dung.");
        if (file.Length > MaxCvSize) throw new InvalidOperationException($"{label} không được vượt quá 10 MB.");
        if (!string.Equals(Path.GetExtension(file.FileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"{label} chỉ nhận file PDF.");
        if (file.Length < 8) throw new InvalidOperationException("File PDF chưa hợp lệ.");

        await using var input = file.OpenReadStream();
        var header = new byte[8];
        await input.ReadExactlyAsync(header, cancellationToken);
        var signature = Encoding.ASCII.GetString(header);
        if (!signature.StartsWith("%PDF-", StringComparison.Ordinal) || !char.IsDigit(signature[5]) || signature[6] != '.' || !char.IsDigit(signature[7]))
            throw new InvalidOperationException("Nội dung file không đúng định dạng PDF.");

        var folder = Path.Combine(environment.WebRootPath, "uploads", folderName);
        Directory.CreateDirectory(folder);
        var name = $"{Guid.NewGuid():N}.pdf";
        var fullPath = Path.Combine(folder, name);
        try
        {
            await using (var output = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                await output.WriteAsync(header, cancellationToken);
                await input.CopyToAsync(output, cancellationToken);
            }
            await using var stored = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            var tail = new byte[(int)Math.Min(1024, stored.Length)];
            stored.Seek(-tail.Length, SeekOrigin.End);
            await stored.ReadExactlyAsync(tail, cancellationToken);
            if (!Encoding.ASCII.GetString(tail).Contains("%%EOF", StringComparison.Ordinal))
                throw new InvalidOperationException("PDF chưa đầy đủ. Vui lòng xuất lại file thành PDF.");
            return $"/uploads/{folderName}/{name}";
        }
        catch
        {
            if (File.Exists(fullPath)) File.Delete(fullPath);
            throw;
        }
    }

    public string? ResolvePrivatePath(string? storedPath)
    {
        if (string.IsNullOrWhiteSpace(storedPath) || !storedPath.StartsWith("/uploads/", StringComparison.Ordinal)) return null;
        var root = Path.GetFullPath(Path.Combine(environment.WebRootPath, "uploads")) + Path.DirectorySeparatorChar;
        var fullPath = Path.GetFullPath(Path.Combine(environment.WebRootPath, storedPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)));
        return fullPath.StartsWith(root, StringComparison.OrdinalIgnoreCase) && File.Exists(fullPath) ? fullPath : null;
    }

    public void DeleteNewUpload(string storedPath)
    {
        var fullPath = ResolvePrivatePath(storedPath);
        if (fullPath is not null) File.Delete(fullPath);
    }
}
