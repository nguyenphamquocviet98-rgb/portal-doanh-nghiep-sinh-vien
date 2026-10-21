using System.Data;
using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Models.Enums;
using InternshipPortal.ViewModels.Internships;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Services;

public record DocumentActionResult(bool Succeeded, string Message, int? Id = null, bool NotFound = false);

public class StudentDocumentService(ApplicationDbContext db, DocumentUploadService uploads, PdfUploadService files)
{
    public async Task<StudentDocumentsViewModel?> GetAsync(string userId, int internshipId, CancellationToken cancellationToken)
    {
        var model = await db.Internships.AsNoTracking().Where(x => x.Id == internshipId && x.Student.UserId == userId)
            .Select(x => new StudentDocumentsViewModel
            {
                InternshipId = x.Id, PositionTitle = x.Application.InternshipPosition.Title,
                CompanyName = x.Application.InternshipPosition.Company.Name, Status = x.Status
            }).SingleOrDefaultAsync(cancellationToken);
        if (model is null) return null;
        var documents = await db.InternshipDocuments.AsNoTracking().Where(x => x.InternshipId == internshipId)
            .OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id).ToListAsync(cancellationToken);
        model.Documents = documents.Select(x => new StudentDocumentViewModel
        {
            Id = x.Id, Name = x.OriginalFileName, DocumentType = x.DocumentType, FileSize = x.FileSize,
            CreatedAt = x.CreatedAt, HasFile = files.ResolvePrivatePath(x.FilePath) is not null
        }).ToList();
        return model;
    }

    public async Task<DocumentActionResult> UploadAsync(string userId, UploadDocumentViewModel model, CancellationToken cancellationToken)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        var internship = await db.Internships.AsNoTracking().SingleOrDefaultAsync(x => x.Id == model.InternshipId && x.Student.UserId == userId, cancellationToken);
        if (internship is null) return new(false, "Không tìm thấy hồ sơ thực tập của bạn.", NotFound: true);
        if (internship.Status is not (InternshipStatus.InProgress or InternshipStatus.Completed or InternshipStatus.Failed))
            return new(false, "Chỉ nộp tài liệu khi đang thực tập hoặc hồ sơ đã kết thúc.");
        if (model.File is null || !Enum.IsDefined(model.DocumentType)) return new(false, "Vui lòng chọn file và loại tài liệu hợp lệ.");
        var stored = await uploads.SaveAsync(model.File, cancellationToken);
        try
        {
            var document = new InternshipDocument
            {
                InternshipId = internship.Id, DocumentType = model.DocumentType, OriginalFileName = stored.OriginalName,
                FilePath = stored.Path, FileSize = stored.Size
            };
            db.InternshipDocuments.Add(document);
            await db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new(true, "Đã nộp tài liệu thực tập.", document.Id);
        }
        catch
        {
            files.DeleteNewUpload(stored.Path);
            throw;
        }
    }

    public async Task<(string Path, string Name)?> GetDownloadAsync(string userId, int id, CancellationToken cancellationToken)
    {
        var document = await db.InternshipDocuments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.Internship.Student.UserId == userId, cancellationToken);
        if (document is null) return null;
        var path = files.ResolvePrivatePath(document.FilePath);
        return path is null ? null : (path, document.OriginalFileName);
    }
}
