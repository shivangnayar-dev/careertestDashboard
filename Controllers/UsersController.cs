using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public IActionResult Grid()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
    }
}