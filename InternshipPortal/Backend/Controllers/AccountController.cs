using InternshipPortal.Data;
using InternshipPortal.Models;
using InternshipPortal.Services;
using InternshipPortal.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternshipPortal.Controllers;

public class AccountController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn,
    AccountService accounts, ApplicationDbContext db) : Controller
{
    [AllowAnonymous, HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction(nameof(Portal));
        return View(new LoginViewModel { ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : null });
    }

    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await users.FindByEmailAsync(model.Email.Trim());
        if (user is null || !user.IsActive)
        {
            ModelState.AddModelError("", "Email hoặc mật khẩu chưa đúng, hoặc tài khoản không được phép đăng nhập.");
            return View(model);
        }
        var result = await signIn.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.IsLockedOut
                ? "Tài khoản tạm khóa do đăng nhập sai nhiều lần. Vui lòng thử lại sau 15 phút."
                : "Email hoặc mật khẩu chưa đúng.");
            return View(model);
        }
        if (Url.IsLocalUrl(model.ReturnUrl)) return LocalRedirect(model.ReturnUrl!);
        return RedirectToAction(nameof(Portal));
    }

    [AllowAnonymous, HttpGet, HttpPost]
    public async Task<IActionResult> QuickLogin(string? role, string? returnUrl = null)
    {
        var targetRole = role?.Trim().ToLowerInvariant();
        var email = targetRole switch
        {
            "admin" => "admin@demo.edu.vn",
            "company" => "company@demo.vn",
            "student" => "student@demo.edu.vn",
            _ => null
        };

        if (email is null)
        {
            TempData["Error"] = "Vai trò demo không hợp lệ. Vui lòng chọn Admin, Company hoặc Student.";
            return RedirectToAction(nameof(Login));
        }

        var user = await users.FindByEmailAsync(email);
        if (user is null)
        {
            TempData["Error"] = $"Tài khoản demo {email} chưa được khởi tạo trong cơ sở dữ liệu.";
            return RedirectToAction(nameof(Login));
        }

        await signIn.SignOutAsync();
        await signIn.SignInAsync(user, isPersistent: true);

        TempData["Success"] = $"Đã đăng nhập thành công với vai trò {(targetRole == "admin" ? "Quản trị viên" : targetRole == "company" ? "Doanh nghiệp" : "Sinh viên")} ({email}).";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return targetRole switch
        {
            "admin" => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
            "company" => RedirectToAction("Index", "Dashboard", new { area = "Company" }),
            "student" => RedirectToAction("Index", "Dashboard", new { area = "Student" }),
            _ => RedirectToAction(nameof(Portal))
        };
    }

    [AllowAnonymous, HttpGet]
    public IActionResult Register() => User.Identity?.IsAuthenticated == true ? RedirectToAction(nameof(Portal)) : View();

    [AllowAnonymous, HttpGet]
    public async Task<IActionResult> RegisterStudent(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction(nameof(Portal));
        var model = new RegisterStudentViewModel();
        await PopulateMajorsAsync(model, cancellationToken);
        return View(model);
    }

    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterStudent(RegisterStudentViewModel model, CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction(nameof(Portal));
        if (ModelState.IsValid)
        {
            var result = await accounts.RegisterStudentAsync(model, cancellationToken);
            if (result.Succeeded)
            {
                TempData["Success"] = "Đăng ký sinh viên thành công. Bạn có thể đăng nhập ngay.";
                return RedirectToAction(nameof(Login));
            }
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }
        await PopulateMajorsAsync(model, cancellationToken);
        return View(model);
    }

    [AllowAnonymous, HttpGet]
    public IActionResult RegisterCompany() => User.Identity?.IsAuthenticated == true ? RedirectToAction(nameof(Portal)) : View(new RegisterCompanyViewModel());

    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterCompany(RegisterCompanyViewModel model, CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction(nameof(Portal));
        if (ModelState.IsValid)
        {
            var result = await accounts.RegisterCompanyAsync(model, cancellationToken);
            if (result.Succeeded)
            {
                TempData["Success"] = "Đăng ký doanh nghiệp thành công. Bạn có thể đăng nhập để xem hồ sơ; cần nhà trường duyệt trước khi đăng vị trí.";
                return RedirectToAction(nameof(Login));
            }
            foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        }
        return View(model);
    }

    [Authorize, HttpGet]
    public async Task<IActionResult> Portal()
    {
        var user = await users.GetUserAsync(User);
        if (user is null) return Challenge();
        var userRoles = await users.GetRolesAsync(user);
        var area = userRoles.Contains("Admin") ? "Admin" : userRoles.Contains("Company") ? "Company" : userRoles.Contains("Student") ? "Student" : null;
        return area is null ? Forbid() : RedirectToAction("Index", "Dashboard", new { area });
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous, HttpGet]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View();
    }

    private async Task PopulateMajorsAsync(RegisterStudentViewModel model, CancellationToken cancellationToken)
    {
        model.Faculties = await db.Faculties.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToListAsync(cancellationToken);
        model.Majors = await db.Majors.AsNoTracking().Where(x => x.IsActive && x.Faculty.IsActive).OrderBy(x => x.Name)
            .Select(x => new MajorOptionViewModel { Id = x.Id, FacultyId = x.FacultyId, Name = x.Name }).ToListAsync(cancellationToken);
    }
}