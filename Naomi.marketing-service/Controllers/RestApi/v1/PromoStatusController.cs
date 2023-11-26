using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoChannelService;
using Naomi.marketing_service.Services.PromoStatusService;
using static Naomi.marketing_service.Models.Request.PromotionStatusRequest;
using static Naomi.marketing_service.Models.Response.PromotionStatusResponse;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
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

        [HttpGet("get_promotion_status")]
        public async Task<ActionResult<ServiceResponse<List<PromotionStatus>>>> GetPromotionStatus(Guid id)
        {
            List<PromotionStatus> promoStatus = await _promoStatusService.GetPromotionStatus(id);
            ServiceResponse<List<PromotionStatus>> response = new();

            if (promoStatus != null && promoStatus.Count > 0)
            {
                response.Data = promoStatus;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_data_status")]
        public async Task<ActionResult<ServiceResponse<List<RespondPromotionStatusCount>>>> GetPromotionStatusCount()
        {
            List<RespondPromotionStatusCount> statusCount = await _promoStatusService.GetPromotionStatusCount();
            ServiceResponse<List<RespondPromotionStatusCount>> response = new();

            if (statusCount != null && statusCount.Count > 0)
            {
                response.Data = statusCount;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpPost("add_promo_status")]
        public async Task<ActionResult<ServiceResponse<PromotionStatus>>> AddPromoStatus([FromBody] CreatePromotionStatus promotionStatus)
        {
            var newPromoStatus = await _promoStatusService.InsertPromotionStatus(_mapper.Map<PromotionStatus>(promotionStatus));
            ServiceResponse<PromotionStatus> response = new();

            if (newPromoStatus.Item1 != null && newPromoStatus.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoStatus.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = newPromoStatus.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
