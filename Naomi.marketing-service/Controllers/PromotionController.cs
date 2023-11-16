using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers
{
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
