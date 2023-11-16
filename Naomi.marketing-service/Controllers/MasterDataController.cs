using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers
{
    public class MasterDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
