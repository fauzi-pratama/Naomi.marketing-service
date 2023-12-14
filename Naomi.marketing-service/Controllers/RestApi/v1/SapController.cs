using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoTypeService;
using Naomi.marketing_service.Services.SapService;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
    [Route("/v1/")]
    [ApiController]
    public class SapController : ControllerBase
    {
        private readonly ISapService _sapService;
        private readonly ILogger<SapController> _logger;
        public SapController(ISapService sapService, ILogger<SapController> logger)
        {
            _sapService = sapService;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_company")]
        public async Task<ActionResult<ServiceResponse<List<CompanyViewModel>>>> GetCompanyViewModelAsync(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_company with Search name: {searchName}", searchName);

            ServiceResponse<List<CompanyViewModel>> response = new();
            var companyViewModel = await _sapService.GetCompanyViewModel(searchName, pageNo, pageSize);

            if (companyViewModel != null && companyViewModel.Item1.Count > 0)
            {
                response.Data = companyViewModel.Item1;
                response.Pages = pageNo;
                response.TotalPages = companyViewModel.Item2;

                _logger.LogInformation("Success get_company with Search name: {searchName}", searchName);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_company with Search name: {searchName}", searchName);
            return NotFound(response);
        }

        [HttpGet("get_site")]
        public async Task<ActionResult<ServiceResponse<List<SiteViewModel>>>> GetSiteViewModelAsync([FromQuery] string? companyCode, List<string>? zoneList, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_site with Company Code: {companyCode}, Zones: {zoneList} and Search name: {searchName}", companyCode, zoneList, searchName);

            var siteViewModel = await _sapService.GetSiteViewModel(companyCode, zoneList, searchName, pageNo, pageSize);
            ServiceResponse<List<SiteViewModel>> response = new();

            if (siteViewModel != null && siteViewModel.Item1.Count > 0)
            {
                response.Data = siteViewModel.Item1;
                response.Pages = pageNo;
                response.TotalPages = siteViewModel.Item2;

                _logger.LogInformation("Success get_site with Company Code: {companyCode}, Zones: {zoneList} and Search name: {searchName}", companyCode, zoneList, searchName);
                return Ok(response);
            }

            var msg = siteViewModel!.Item3 == "" ? "Data not found" : siteViewModel.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_site with message: {msg}, Company Code: {companyCode}, Zones: {zoneList} and Search name: {searchName}", msg, companyCode, zoneList, searchName);
            return NotFound(response);
        }

        [HttpGet("get_zone")]
        public async Task<ActionResult<ServiceResponse<List<ZoneViewModel>>>> GetZoneViewModelAsync(string? companyCode, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_zone with Company code: {companyCode} and Search name: {searchName}", companyCode, searchName);

            ServiceResponse<List<ZoneViewModel>> response = new();
            var zoneViewModel = await _sapService.GetZoneViewModel(companyCode, searchName, pageNo, pageSize);

            if (zoneViewModel != null && zoneViewModel.Item1.Count > 0)
            {
                response.Data = zoneViewModel.Item1;
                response.Pages = pageNo;
                response.TotalPages = zoneViewModel.Item2;

                _logger.LogInformation("Success get_zone with Company code: {companyCode} and Search name: {searchName}", companyCode, searchName);
                return Ok(response);
            }

            var msg = zoneViewModel!.Item3 == "" ? "Data not found" : zoneViewModel.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_zone with message: {msg}, Company code: {companyCode} and Search name: {searchName}", msg, companyCode, searchName);
            return NotFound(response);
        }

        [HttpGet("get_mop")]
        public async Task<ActionResult<ServiceResponse<List<MopViewModel>>>> GetMopViewModelAsync(string? companyCode, string? siteCode, string? searchName, bool isPromotion = false, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_mop with Company code: {companyCode}, Site code: {siteCode}, Search name: {searchName} and Promotion is {isPromotion}", companyCode, siteCode, searchName, isPromotion);

            var mopViewModel = await _sapService.GetMopViewModel(companyCode, siteCode, searchName, isPromotion, pageNo, pageSize);
            ServiceResponse<List<MopViewModel>> response = new();

            if (mopViewModel != null && mopViewModel.Item1.Count > 0)
            {
                response.Data = mopViewModel.Item1;
                response.Pages = pageNo;
                response.TotalPages = mopViewModel.Item2;

                _logger.LogInformation("Success get_mop with Company code: {companyCode}, Site code: {siteCode}, Search name: {searchName} and Promotion is {isPromotion}", companyCode, siteCode, searchName, isPromotion);
                return Ok(response);
            }

            var msg = mopViewModel!.Item3 == "" ? "Data not found" : mopViewModel.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_mop with Company code: {companyCode}, Site code: {siteCode}, Search name: {searchName} and Promotion is {isPromotion}", companyCode, siteCode, searchName, isPromotion);
            return NotFound(response);
        }
        #endregion

        #region SetIsPromotion
        [HttpPut("Set_mop_is_promotion")]
        public async Task<ActionResult<ServiceResponse<MopViewModel>>> SetMopIsPromotionAsync(Guid mopId, bool isPromotion = true)
        {
            _logger.LogInformation("Calling Set_mop_is_promotion with Mop Id: {mopId} and isPromotion: {isPromotion}", mopId, isPromotion);

            ServiceResponse<MopViewModel> response = new();
            var mopViewModel = await _sapService.SetMopIsPromotion(mopId, isPromotion);

            if (mopViewModel.Item1 != null && mopViewModel.Item1.Id != Guid.Empty)
            {
                response.Data = mopViewModel.Item1!;

                _logger.LogInformation("Success Set_mop_is_promotion with Mop Id: {mopId} and isPromotion: {isPromotion}", mopId, isPromotion);
                return Ok(response);
            }

            var msg = mopViewModel.Item2 == "" ? "Data Not Found" : mopViewModel.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed Set_mop_is_promotion with message: {msg} and Mop Id: {mopId} and isPromotion: {isPromotion}", msg, mopId, isPromotion);
            return BadRequest(response);
        }
        #endregion
    }
}
