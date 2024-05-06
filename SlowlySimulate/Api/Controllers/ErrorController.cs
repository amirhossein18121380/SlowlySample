using Microsoft.AspNetCore.Mvc;
using SlowlySimulate.Api.ViewModels;
using System.Diagnostics;

namespace SlowlySimulate.Api.Controllers;

public class ErrorController : Controller
{
    //public IActionResult Index()
    //{
    //    return View();
    //}
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
