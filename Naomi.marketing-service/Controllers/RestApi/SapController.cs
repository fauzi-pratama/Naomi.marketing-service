using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers.RestApi
{
    public class SapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
