using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CalendarMonthGrid()
        {
            return View();
        }
    }
}