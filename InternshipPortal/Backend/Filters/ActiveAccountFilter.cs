using InternshipPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternshipPortal.Filters;

public class ActiveAccountFilter(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;
        var user = await users.GetUserAsync(context.HttpContext.User);
        if (user is not null && user.IsActive && !await users.IsLockedOutAsync(user)) return;
        await signIn.SignOutAsync();
        context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
    }
}