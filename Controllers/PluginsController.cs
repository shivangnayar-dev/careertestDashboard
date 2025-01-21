using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class PluginsController : Controller
    {
        // GET: Plugins
        public IActionResult Lightbox()
        {
            return View();
        }
        public IActionResult Simplebar()
        {
            return View();
        }
        public IActionResult Sweetalert()
        {
            return View();
        }
        public IActionResult SwiperSlider()
        {
            return View();
        }
    }
}