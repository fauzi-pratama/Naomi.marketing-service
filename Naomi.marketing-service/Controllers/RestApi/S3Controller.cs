using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers.RestApi
{
    public class S3Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
