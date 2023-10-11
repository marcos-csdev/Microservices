using Microsoft.AspNetCore.Mvc;

namespace Microservices.EmailAPI.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
