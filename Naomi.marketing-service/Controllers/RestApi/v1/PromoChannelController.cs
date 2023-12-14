using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoChannelService;
using Naomi.marketing_service.Services.PromoClassService;
using Newtonsoft.Json;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
    [Route("/v1/")]
    [ApiController]
    public class PromoChannelController : ControllerBase
    {
        public readonly IPromoChannelService _promoChannelService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoChannelController> _logger;
        public PromoChannelController(IPromoChannelService promoChannelService, IMapper mapper, ILogger<PromoChannelController> logger) 
        {
            _promoChannelService = promoChannelService;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_promotion_channel")]
        public async Task<ActionResult<ServiceResponse<List<PromotionChannel>>>> GetPromotionChannelAsync(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_promotion_channel with search name: {searchName}", searchName);

            ServiceResponse<List<PromotionChannel>> response = new();
            var promoChannels = await _promoChannelService.GetPromotionChannel(searchName!, pageNo, pageSize);

            if (promoChannels != null && promoChannels.Item1.Count > 0)
            {
                response.Data = promoChannels.Item1;
                response.Pages = pageNo;
                response.TotalPages = promoChannels.Item2;

                _logger.LogInformation("Success get_promotion_channel with search name: {searchName}", searchName);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_promotion_channel with search name: {searchName}", searchName);
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_promotion_channel")]
        public async Task<ActionResult<ServiceResponse<PromotionChannel>>> AddPromotionChannelAsync([FromBody] PromoChannelRequest promotionChannel)
        {
            var param = JsonConvert.SerializeObject(promotionChannel);
            _logger.LogInformation("Calling add_promotion_channel with params: {param}", param);

            ServiceResponse<PromotionChannel> response = new();
            var newPromoChannel = await _promoChannelService.InsertPromotionChannel(_mapper.Map<PromotionChannel>(promotionChannel));

            if (newPromoChannel.Item1 != null && newPromoChannel.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoChannel.Item1;

                _logger.LogInformation("Success add_promotion_channel with params: {param}", param);
                return Ok(response);
            }

            var msg = newPromoChannel.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed add_promotion_channel with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion
    }
}
