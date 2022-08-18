using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SONA.Controllers
{
    public class MyProfileController : Controller
    {
        // GET
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}