using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoAppService;
using Naomi.marketing_service.Services.PromoClassService;
using Newtonsoft.Json;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class PromoAppController : ControllerBase
    {
        public readonly IPromoAppService _promoAppService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoAppController> _logger;

        public PromoAppController(IPromoAppService promoAppService, IMapper mapper, ILogger<PromoAppController> logger) 
        {
            _promoAppService = promoAppService;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_promo_app_display")]
        public async Task<ActionResult<ServiceResponse<List<PromotionAppDisplay>>>> GetPromotionAppDisplay(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            ServiceResponse<List<PromotionAppDisplay>> response = new();
            var promoAppDisplay = await _promoAppService.GetPromotionAppDisplay(searchName, pageNo, pageSize);

            if (promoAppDisplay != null && promoAppDisplay.Item1.Count > 0)
            {
                response.Data = promoAppDisplay.Item1;
                response.Pages = pageNo;
                response.TotalPages = promoAppDisplay.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }
        #endregion

        #region InsertData
        [HttpPost("add_promotion_app_display")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionAppDisplay, string>>>> AddPromotionAppDisplay([FromBody] AppDisplayRequest promoAppDisplay)
        {
            var msg = JsonConvert.SerializeObject(promoAppDisplay);
            _logger.LogInformation("Calling add_promotion_app_display with params {msg}", msg);

            ServiceResponse<PromotionAppDisplay> response = new();
            var newAppDisplay = await _promoAppService.InsertPromotionAppDisplay(_mapper.Map<PromotionAppDisplay>(promoAppDisplay));

            if (newAppDisplay.Item1 != null && newAppDisplay.Item1.Id != Guid.Empty)
            {
                response.Data = newAppDisplay.Item1!;

                _logger.LogInformation("Success add_promotion_app_display with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = newAppDisplay.Item2;
                response.Success = false;

                _logger.LogError("Failed add_promotion_app_display with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion

        #region InsertData
        [HttpPut("edit_promotion_app_display")]
        public async Task<ActionResult<ServiceResponse<PromotionAppDisplay>>> EditPromotionAppDisplay([FromBody] AppDisplayEditRequest promoAppDisplay)
        {
            var msg = JsonConvert.SerializeObject(promoAppDisplay);
            _logger.LogInformation("Calling edit_promotion_app_display with params {msg}", msg);

            ServiceResponse<PromotionAppDisplay> response = new();
            var updatePromoAppDisplay = await _promoAppService.UpdatePromotionAppDisplay(_mapper.Map<PromotionAppDisplay>(promoAppDisplay));

            if (updatePromoAppDisplay.Item1 != null && updatePromoAppDisplay.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoAppDisplay.Item1;

                _logger.LogInformation("Success edit_promotion_app_display with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = updatePromoAppDisplay!.Item2;
                response.Success = false;

                _logger.LogError("Failed edit_promotion_app_display with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion
    }
}
