using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers.RestApi
{
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
