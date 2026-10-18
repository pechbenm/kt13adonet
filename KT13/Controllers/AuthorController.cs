using Microsoft.AspNetCore.Mvc;

namespace KT13.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
