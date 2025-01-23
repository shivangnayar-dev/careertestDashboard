using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class AppController : Controller
    {
        // GET: App
        public IActionResult mailbox()
        {
            return View();
        }
    }
}