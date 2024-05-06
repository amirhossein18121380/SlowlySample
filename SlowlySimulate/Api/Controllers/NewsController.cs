using Microsoft.AspNetCore.Mvc;

namespace SlowlySimulate.Api.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}