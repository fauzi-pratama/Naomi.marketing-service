
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Naomi.marketing_service.Controllers.MessageHandler
{
    public class SapMessageController : ControllerBase
    {
        private readonly ILogger<SapMessageController> _logger;

        public SapMessageController(ILogger<SapMessageController> logger)
        {
            _logger = logger;
        }

        [NonAction]
        [CapSubscribe("site")]
        public void SiteMessage(JsonElement jsonElement)
        {
            _logger.LogDebug("Receive message site : {jsonElement}", jsonElement);
        }
    }
}
