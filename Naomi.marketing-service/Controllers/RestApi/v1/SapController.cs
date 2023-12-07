using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoTypeService;
using Naomi.marketing_service.Services.SapService;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
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
        public async Task<ActionResult<ServiceResponse<List<CompanyViewModel>>>> GetCompanyViewModel(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            var companyViewModel = await _sapService.GetCompanyViewModel(searchName, pageNo, pageSize);
            ServiceResponse<List<CompanyViewModel>> response = new()
            {
                Data = companyViewModel.Item1
            };

            if (companyViewModel != null && companyViewModel.Item1.Count > 0)
            {
                response.Pages = pageNo;
                response.TotalPages = companyViewModel.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_site")]
        public async Task<ActionResult<ServiceResponse<List<SiteViewModel>>>> GetSiteViewModel([FromQuery] string? companyCode, List<string>? zoneList, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            var siteViewModel = await _sapService.GetSiteViewModel(companyCode, zoneList, searchName, pageNo, pageSize);
            ServiceResponse<List<SiteViewModel>> response = new()
            {
                Data = siteViewModel.Item1
            };

            if (siteViewModel != null && siteViewModel.Item1.Count > 0)
            {
                response.Pages = pageNo;
                response.TotalPages = siteViewModel.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = siteViewModel!.Item3 == "" ? "Data not found" : siteViewModel.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_zone")]
        public async Task<ActionResult<ServiceResponse<List<ZoneViewModel>>>> GetZoneViewModel(string? companyCode, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            var zoneViewModel = await _sapService.GetZoneViewModel(companyCode, searchName, pageNo, pageSize);
            ServiceResponse<List<ZoneViewModel>> response = new()
            {
                Data = zoneViewModel.Item1
            };

            if (zoneViewModel != null && zoneViewModel.Item1.Count > 0)
            {
                response.Pages = pageNo;
                response.TotalPages = zoneViewModel.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = zoneViewModel!.Item3 == "" ? "Data not found" : zoneViewModel.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_mop")]
        public async Task<ActionResult<ServiceResponse<List<MopViewModel>>>> GetMopViewModel(string? companyCode, string? siteCode, string? searchName, bool isPromotion = false, int pageNo = 1, int pageSize = 10)
        {
            var mopViewModel = await _sapService.GetMopViewModel(companyCode, siteCode, searchName, isPromotion, pageNo, pageSize);
            ServiceResponse<List<MopViewModel>> response = new()
            {
                Data = mopViewModel.Item1
            };

            if (mopViewModel != null && mopViewModel.Item1.Count > 0)
            {
                response.Pages = pageNo;
                response.TotalPages = mopViewModel.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = mopViewModel!.Item3 == "" ? "Data not found" : mopViewModel.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }
        #endregion

        #region SetIsPromotion
        [HttpPut("Set_mop_is_promotion")]
        public async Task<ActionResult<ServiceResponse<MopViewModel>>> SetMopIsPromotion(Guid mopId, bool isPromotion = true)
        {
            _logger.LogInformation(string.Format("Calling Set_mop_is_promotion with Mop Id: {0} and isPromotion: {1}", mopId, isPromotion));

            ServiceResponse<MopViewModel> response = new();
            var mopViewModel = await _sapService.SetMopIsPromotion(mopId, isPromotion);
            if (mopViewModel.Item1 != null && mopViewModel.Item1.Id != Guid.Empty)
            {
                response.Data = mopViewModel.Item1!;

                _logger.LogInformation(string.Format("Success Set_mop_is_promotion with Mop Id: {0} and isPromotion: {1}", mopId, isPromotion));
                return Ok(response);
            }
            else
            {
                response.Message = mopViewModel.Item2 == "" ? "Data Not Found" : mopViewModel.Item2;
                response.Success = false;

                _logger.LogInformation(string.Format("Failed Set_mop_is_promotion with Mop Id: {0} and isPromotion: {1}", mopId, isPromotion));
                return BadRequest(response);
            }
        }
        #endregion
    }
}
