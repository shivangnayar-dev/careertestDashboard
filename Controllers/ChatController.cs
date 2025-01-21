using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public IActionResult Index()
        {
            return View();
        }
    }
}