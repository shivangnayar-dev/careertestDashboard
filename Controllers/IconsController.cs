using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class IconsController : Controller
    {
        // GET: Icons
        public IActionResult Lucide()
        {
            return View();
        }
        public IActionResult Remix()
        {
            return View();
        }
    }
}