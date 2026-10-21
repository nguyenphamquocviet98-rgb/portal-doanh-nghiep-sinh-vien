using InternshipPortal.Data;
using InternshipPortal.Filters;
using InternshipPortal.Models;
using InternshipPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "Frontend/wwwroot"
});

builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Frontend/Views/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Frontend/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Frontend/Areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Frontend/Areas/{2}/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Frontend/Views/Shared/{0}.cshtml");
});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ActiveAccountFilter>();
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((_, field) => $"Giá trị của {field} chưa hợp lệ.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(field => $"{field} phải là số.");
});
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<PdfUploadService>();
builder.Services.AddScoped<StudentProfileService>();
builder.Services.AddScoped<CompanyProfileService>();
builder.Services.AddScoped<CompanyPositionService>();
builder.Services.AddScoped<CompanyApplicationService>();
builder.Services.AddScoped<CompanyInternshipService>();
builder.Services.AddScoped<AdminCompanyService>();
builder.Services.AddScoped<AcademicUnitService>();
builder.Services.AddScoped<PositionCatalogService>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<StudentInternshipService>();
builder.Services.AddScoped<DocumentUploadService>();
builder.Services.AddScoped<StudentDocumentService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Chưa cấu hình kết nối database.")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddErrorDescriber<VietnameseIdentityErrorDescriber>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Seed chỉ chạy khi được gọi rõ ràng, không tự thay đổi database mỗi lần mở web.
if (args.Contains("--seed-demo"))
{
    using var scope = app.Services.CreateScope();
    await SeedData.InitializeAsync(scope.ServiceProvider, app.Environment, app.Configuration);
    return;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Profile HTTP phục vụ demo local; môi trường triển khai chuyển sang HTTPS.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
// CV và báo cáo không được phục vụ công khai từ wwwroot.
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/uploads", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }
    await next(context);
});
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
