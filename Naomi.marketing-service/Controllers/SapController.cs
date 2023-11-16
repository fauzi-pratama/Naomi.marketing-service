using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers
{
    public class SapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
