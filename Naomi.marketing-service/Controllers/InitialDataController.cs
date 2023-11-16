using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers
{
    public class InitialDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
