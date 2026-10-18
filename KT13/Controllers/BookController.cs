using Microsoft.AspNetCore.Mvc;

namespace KT13.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
