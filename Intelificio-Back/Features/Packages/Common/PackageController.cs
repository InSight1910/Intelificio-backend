using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Packages.Common;

public class PackageController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}