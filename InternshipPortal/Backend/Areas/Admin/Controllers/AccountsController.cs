using System.Data;
using InternshipPortal.Data;
using InternshipPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
namespace InternshipPortal.Areas.Admin.Controllers;
[Area("Admin"), Authorize(Roles = "Admin"), ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AccountsController(ApplicationDbContext db) : Controller
{
    private IQueryable<string> RoleUsers(string role) => from ur in db.UserRoles join r in db.Roles on ur.RoleId equals r.Id where r.Name == role select ur.UserId;
    public async Task<IActionResult> Index(AdminAccountsViewModel model, CancellationToken ct)
    {
        if (model.Role is not (null or "" or "Student" or "Company" or "Admin")) { ModelState.AddModelError("Role", "Nhóm tài khoản không hợp lệ."); model.Role = null; }
        if (!ModelState.IsValid) model.Keyword = null;
        var query = db.Users.AsNoTracking();
        if (!string.IsNullOrEmpty(model.Role)) query = query.Where(x => RoleUsers(model.Role).Contains(x.Id));
        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            var term = model.Keyword.Trim();
            query = query.Where(x => x.FullName.Contains(term) || x.Email!.Contains(term) || db.Students.Any(s => s.UserId == x.Id && s.StudentCode.Contains(term)));
        }
        model.TotalCount = await query.CountAsync(ct);
        model.TotalPages = Math.Max(1, (int)Math.Ceiling(model.TotalCount / 10d)); model.Page = Math.Clamp(model.Page, 1, model.TotalPages);
        model.Accounts = await query.OrderBy(x => x.FullName).ThenBy(x => x.Id).Skip((model.Page - 1) * 10).Take(10)
            .Select(x => new AdminAccountDetailsViewModel { Id = x.Id, FullName = x.FullName, Email = x.Email, IsActive = x.IsActive }).ToListAsync(ct);
        return View(model);
    }
    private async Task<AdminAccountDetailsViewModel?> GetAsync(string id, CancellationToken ct)
    {
        var x = await db.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        if (x is null) return null;
        var model = new AdminAccountDetailsViewModel { Id = id, FullName = x.FullName, Email = x.Email, Phone = x.PhoneNumber, IsActive = x.IsActive, Version = x.ConcurrencyStamp };
        model.Roles = await (from ur in db.UserRoles join r in db.Roles on ur.RoleId equals r.Id where ur.UserId == id select r.Name!).ToListAsync(ct);
        var student = await db.Students.AsNoTracking().Include(x => x.Major).ThenInclude(x => x.Faculty).SingleOrDefaultAsync(x => x.UserId == id, ct);
        if (student is not null) { model.StudentCode = student.StudentCode; model.ClassName = student.ClassName; model.MajorName = student.Major.Name; model.FacultyName = student.Major.Faculty.Name; }
        model.CompanyName = await db.Companies.Where(x => x.UserId == id).Select(x => x.Name).SingleOrDefaultAsync(ct);
        return model;
    }
    public async Task<IActionResult> Details(string id, CancellationToken ct)
    {
        var model = await GetAsync(id, ct); return model is null ? NotFound() : View(model);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(string id, ToggleAccountViewModel input, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            try
            {
                var user = await db.Users.SingleOrDefaultAsync(x => x.Id == id, ct);
                if (user is null) return NotFound();
                if (await RoleUsers("Admin").ContainsAsync(id, ct)) ModelState.AddModelError("", "Không thay đổi hoạt động tài khoản Admin ở chức năng này.");
                else if (user.ConcurrencyStamp != input.Version) ModelState.AddModelError("", "Tài khoản vừa thay đổi. Hãy tải lại trang.");
                else
                {
                    user.IsActive = input.IsActive!.Value;
                    user.ConcurrencyStamp = Guid.NewGuid().ToString();
                    await db.SaveChangesAsync(ct); await tx.CommitAsync(ct);
                    TempData["Success"] = "Đã cập nhật hoạt động tài khoản.";
                    return RedirectToAction(nameof(Details), new { id });
                }
            }
            catch (DbUpdateConcurrencyException) { ModelState.AddModelError("", "Tài khoản vừa thay đổi. Hãy tải lại trang."); }
            catch (Exception ex) when (Deadlock(ex)) { ModelState.AddModelError("", "Có thao tác đồng thời. Hãy tải lại trang."); }
        }
        var model = await GetAsync(id, ct);
        if (model is null) return NotFound();
        model.Version = input.Version;
        return View("Details", model);
    }
    private static bool Deadlock(Exception ex)
    {
        for (Exception? e = ex; e is not null; e = e.InnerException) if (e is SqlException { Number: 1205 }) return true;
        return false;
    }
}
