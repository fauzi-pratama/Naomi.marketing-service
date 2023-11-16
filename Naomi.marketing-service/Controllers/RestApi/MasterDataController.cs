using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers.RestApi
{
    public class MasterDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
