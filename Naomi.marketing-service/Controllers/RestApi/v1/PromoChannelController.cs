using AutoMapper;
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
        public async Task<ActionResult<ServiceResponse<List<PromotionChannel>>>> GetPromotionChannel(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            ServiceResponse<List<PromotionChannel>> response = new();
            var promoChannels = await _promoChannelService.GetPromotionChannel(searchName!, pageNo, pageSize);

            if (promoChannels != null && promoChannels.Item1.Count > 0)
            {
                response.Data = promoChannels.Item1;
                response.Pages = pageNo;
                response.TotalPages = promoChannels.Item2;
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
        [HttpPost("add_promotion_channel")]
        public async Task<ActionResult<ServiceResponse<PromotionChannel>>> AddPromotionChannel([FromBody] PromoChannelRequest promotionChannel)
        {
            var msg = JsonConvert.SerializeObject(promotionChannel);
            _logger.LogInformation("Calling add_promotion_channel with params {msg}", msg);

            ServiceResponse<PromotionChannel> response = new();
            var newPromoChannel = await _promoChannelService.InsertPromotionChannel(_mapper.Map<PromotionChannel>(promotionChannel));

            if (newPromoChannel.Item1 != null && newPromoChannel.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoChannel.Item1;

                _logger.LogInformation("Success add_promotion_channel with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = newPromoChannel.Item2;
                response.Success = false;

                _logger.LogError("Failed add_promotion_channel with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion
    }
}
