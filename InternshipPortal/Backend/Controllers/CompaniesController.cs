using InternshipPortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Controllers;

[AllowAnonymous]
public class CompaniesController(PositionCatalogService catalog) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var company = await catalog.GetCompanyAsync(id, cancellationToken);
        if (company is not null) return View(company);
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("/Frontend/Views/Internships/Unavailable.cshtml");
    }
}