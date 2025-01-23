using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public IActionResult Breadcrumb()
        {
            return View();
        }
        public IActionResult Navbars()
        {
            return View();
        }
        public IActionResult Pagination()
        {
            return View();
        }
        public IActionResult Tabs()
        {
            return View();
        }
    }
}