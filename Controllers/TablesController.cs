using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class TablesController : Controller
    {
        // GET: Tables
        public IActionResult Basic()
        {
            return View();
        }
        public IActionResult Datatable()
        {
            return View();
        }
        public IActionResult Gridjs()
        {
            return View();
        }
        public IActionResult Listjs()
        {
            return View();
        }
    }
}