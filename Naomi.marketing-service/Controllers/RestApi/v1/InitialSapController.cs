using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Services.SapService;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class InitialSapController : ControllerBase
    {
        private readonly ISapService _sapService;
        public InitialSapController(ISapService sapService)
        {
            _sapService= sapService;
        }

        #region GetData
        [HttpPost("get_initial_data_company")]
        public async Task<IActionResult> GetInitialDataCompany()
        {
            await _sapService.GetCompany();

            return Ok();
        }

        [HttpPost("get_initial_data_sitezone")]
        public async Task<IActionResult> GetInitialDataSiteZone()
        {
            await _sapService.GetSiteZoneSAP();

            return Ok();
        }

        [HttpPost("get_initial_data_mop")]
        public async Task<IActionResult> GetInitialDataMop()
        {
            await _sapService.GetMop();

            return Ok();
        }
        #endregion
    }
}
