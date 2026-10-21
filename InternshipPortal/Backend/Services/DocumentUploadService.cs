using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace InternshipPortal.Services;

public record StoredDocument(string Path, string OriginalName, long Size);

public class DocumentUploadService(IWebHostEnvironment environment, PdfUploadService pdfFiles)
{
    public const long MaxSize = 10 * 1024 * 1024;

    public async Task<StoredDocument> SaveAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0) throw new InvalidOperationException("Vui lòng chọn tài liệu có nội dung.");
        if (file.Length > MaxSize) throw new InvalidOperationException("Tài liệu không được vượt quá 10 MB.");
        var name = Path.GetFileName(file.FileName.Replace('\\', '/')).Trim();
        if (string.IsNullOrWhiteSpace(name) || name.Any(char.IsControl) || name.Length > 200)
            throw new InvalidOperationException("Tên file cần hợp lệ và không quá 200 ký tự. Vui lòng đổi tên rồi nộp lại.");
        var extension = Path.GetExtension(name).ToLowerInvariant();
        if (extension == ".pdf")
            return new(await pdfFiles.SaveDocumentPdfAsync(file, cancellationToken), name, file.Length);
        if (extension != ".docx") throw new InvalidOperationException("Tài liệu chỉ nhận PDF hoặc DOCX.");
        ValidateDocx(file);
        var folder = Path.Combine(environment.WebRootPath, "uploads", "documents");
        Directory.CreateDirectory(folder);
        var storedName = $"{Guid.NewGuid():N}.docx";
        var fullPath = Path.Combine(folder, storedName);
        try
        {
            await using var output = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, useAsync: true);
            await file.CopyToAsync(output, cancellationToken);
            return new($"/uploads/documents/{storedName}", name, file.Length);
        }
        catch
        {
            if (File.Exists(fullPath)) File.Delete(fullPath);
            throw;
        }
    }

    private static void ValidateDocx(IFormFile file)
    {
        try
        {
            if (file.Length < 22) throw new InvalidDataException();
            using var input = file.OpenReadStream();
            var signature = new byte[4];
            input.ReadExactly(signature);
            if (signature[0] != 0x50 || signature[1] != 0x4b || signature[2] != 0x03 || signature[3] != 0x04)
                throw new InvalidDataException();
            input.Position = 0;
            using var archive = new ZipArchive(input, ZipArchiveMode.Read);
            var types = archive.GetEntry("[Content_Types].xml");
            var document = archive.GetEntry("word/document.xml");
            if (types is null || document is null || types.Length > 1024 * 1024
                || archive.Entries.Any(x => x.FullName.EndsWith("vbaProject.bin", StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException();
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 1024 * 1024 };
            using var typesStream = types.Open();
            using var typesReader = XmlReader.Create(typesStream, settings);
            var contentTypes = XDocument.Load(typesReader);
            XNamespace ns = "http://schemas.openxmlformats.org/package/2006/content-types";
            if (!contentTypes.Descendants(ns + "Override").Any(x => (string?)x.Attribute("PartName") == "/word/document.xml"
                && (string?)x.Attribute("ContentType") == "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"))
                throw new InvalidDataException();
            using var documentStream = document.Open();
            using var documentReader = XmlReader.Create(documentStream, settings);
            documentReader.MoveToContent();
            if (documentReader.LocalName != "document" || documentReader.NamespaceURI is not
                ("http://schemas.openxmlformats.org/wordprocessingml/2006/main" or "http://purl.oclc.org/ooxml/wordprocessingml/main"))
                throw new InvalidDataException();
        }
        catch (Exception exception) when (exception is InvalidDataException or XmlException or EndOfStreamException or ArgumentOutOfRangeException)
        {
            throw new InvalidOperationException("File không đúng định dạng Word DOCX hợp lệ.");
        }
    }
}
