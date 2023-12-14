using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoChannelService;
using Naomi.marketing_service.Services.PromoStatusService;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;
using static Naomi.marketing_service.Models.Response.PromotionStatusResponse;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
    [Route("/v1/")]
    [ApiController]
    public class PromoStatusController : ControllerBase
    {
        public readonly IPromoStatusService _promoStatusService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoStatusController> _logger;
        public PromoStatusController(IPromoStatusService promoStatusService, IMapper mapper, ILogger<PromoStatusController> logger)
        {
            _promoStatusService = promoStatusService;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_promotion_status")]
        public async Task<ActionResult<ServiceResponse<List<PromotionStatus>>>> GetPromotionStatusAsync(Guid id)
        {
            _logger.LogInformation("Calling get_promotion_status with Id: {id}", id);

            ServiceResponse<List<PromotionStatus>> response = new();
            List<PromotionStatus> promoStatus = await _promoStatusService.GetPromotionStatus(id);

            if (promoStatus != null && promoStatus.Count > 0)
            {
                response.Data = promoStatus;

                _logger.LogInformation("Success get_promotion_status with Id: {id}", id);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_promotion_status with Id: {id}", id);
            return NotFound(response);
        }

        [HttpGet("get_data_status")]
        public async Task<ActionResult<ServiceResponse<List<RespondPromotionStatusCount>>>> GetPromotionStatusCountAsync()
        {
            _logger.LogInformation("Calling get_data_status");

            ServiceResponse<List<RespondPromotionStatusCount>> response = new();
            List<RespondPromotionStatusCount> statusCount = await _promoStatusService.GetPromotionStatusCount();

            if (statusCount != null && statusCount.Count > 0)
            {
                response.Data = statusCount;

                _logger.LogInformation("Success get_data_status");
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_data_status");
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_promo_status")]
        public async Task<ActionResult<ServiceResponse<PromotionStatus>>> AddPromoStatusAsync([FromBody] CreatePromotionStatus promotionStatus)
        {
            var param = JsonConvert.SerializeObject(promotionStatus);
            _logger.LogInformation("Calling add_promo_status with params: {param}", param);

            ServiceResponse<PromotionStatus> response = new();
            var newPromoStatus = await _promoStatusService.InsertPromotionStatus(_mapper.Map<PromotionStatus>(promotionStatus));

            if (newPromoStatus.Item1 != null && newPromoStatus.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoStatus.Item1;
                
                _logger.LogInformation("Success add_promo_status with params: {param}", param);
                return Ok(response);
            }

            var msg = newPromoStatus.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed add_promo_status with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion 
    }
}
