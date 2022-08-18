using Microsoft.AspNetCore.Mvc;

namespace SONA.Controllers
{
    public class AboutController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}