using Microsoft.AspNetCore.Mvc;

namespace Auxx.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public IActionResult AddNew ()
        {
            return View();
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Overview()
        {
            return View();
        }
    }
}