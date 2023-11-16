using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers.RestApi
{
    public class InitialDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
