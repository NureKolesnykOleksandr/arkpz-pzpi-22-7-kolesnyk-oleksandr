using Microsoft.AspNetCore.Mvc;

namespace ServerMM.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
