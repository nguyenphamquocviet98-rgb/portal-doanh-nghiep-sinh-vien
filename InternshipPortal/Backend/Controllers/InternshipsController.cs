using InternshipPortal.Services;
using InternshipPortal.ViewModels.Internships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternshipPortal.Controllers;

[AllowAnonymous]
public class InternshipsController(PositionCatalogService catalog) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(InternshipSearchViewModel model, CancellationToken cancellationToken)
    {
        await catalog.PopulateFiltersAsync(model, cancellationToken);
        if (ModelState.IsValid) await catalog.SearchAsync(model, cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var position = await catalog.GetDetailsAsync(id, cancellationToken);
        if (position is not null) return View(position);
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("Unavailable");
    }
}