
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Message.Consume;
using Naomi.marketing_service.Services.SapService;
using System.Text.Json;

namespace Naomi.marketing_service.Controllers.MessageHandler
{
    public class SapMessageController : ControllerBase
    {
        private readonly ILogger<SapMessageController> _logger;
        private readonly ISapService _sapService;

        public SapMessageController(ILogger<SapMessageController> logger, ISapService sapService)
        {
            _logger = logger;
            _sapService = sapService;
        }

        [NonAction]
        [CapSubscribe("site")]
        public async Task ConsumeSiteMessage(SapIntegrationMessage message)
        {
            _logger.LogDebug("Receive message site : {message}", message);

            try
            {
                SiteMessage? msg = JsonSerializer.Deserialize<SiteMessage>(message!.SyncData!.ToString() ?? "");
                if (msg != null)
                {
                    #region COMPANY
                    await _sapService.InsertCompany(msg);
                    _logger.LogInformation("Insert/update Company data succeeded");
                    #endregion

                    #region SITE
                    await _sapService.InsertSite(msg);
                    _logger.LogInformation("Insert/update Site data succeeded");
                    #endregion

                    #region ZONE
                    await _sapService.InsertZone(msg);
                    _logger.LogInformation("Insert/update Zone data succeeded");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                _logger.LogError("Error Message : {msg}", msg);
            }
            
        }

        [NonAction]
        [CapSubscribe("mop")]
        public async Task MopSubcriber(SapIntegrationMessage message)
        {
            _logger.LogInformation("Receive message mop : {message}", message);

            try
            {
                MopMessage? msg = JsonSerializer.Deserialize<MopMessage>(message!.SyncData!.ToString() ?? "");
                if (msg != null)
                {
                    await _sapService.InsertMop(msg);
                    _logger.LogInformation("Insert/update Mop data succeeded");
                }

            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                _logger.LogError("Error Message : {msg}", msg);
            }
        }
    }
}
