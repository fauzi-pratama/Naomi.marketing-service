﻿using Microsoft.AspNetCore.Mvc;

namespace Naomi.marketing_service.Controllers
{
    public class EntertainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
