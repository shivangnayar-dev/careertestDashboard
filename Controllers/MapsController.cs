using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class MapsController : Controller
    {
        // GET: Maps
        public IActionResult Google()
        {
            return View();
        }
        public IActionResult Leaflet()
        {
            return View();
        }
    }
}