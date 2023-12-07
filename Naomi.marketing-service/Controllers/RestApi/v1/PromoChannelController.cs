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
            var promoChannels = await _promoChannelService.GetPromotionChannel(searchName!, pageNo, pageSize);
            ServiceResponse<List<PromotionChannel>> response = new();

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
            _logger.LogInformation(string.Format("Calling add_promotion_channel with params {0}", JsonConvert.SerializeObject(promotionChannel)));

            var newPromoChannel = await _promoChannelService.InsertPromotionChannel(_mapper.Map<PromotionChannel>(promotionChannel));
            ServiceResponse<PromotionChannel> response = new();

            if (newPromoChannel.Item1 != null && newPromoChannel.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoChannel.Item1;

                _logger.LogInformation(string.Format("Success add_promotion_channel with params {0}", JsonConvert.SerializeObject(promotionChannel)));
                return Ok(response);
            }
            else
            {
                response.Message = newPromoChannel.Item2;
                response.Success = false;

                _logger.LogError(string.Format("Failed add_promotion_channel with params {0}", JsonConvert.SerializeObject(promotionChannel)));
                return BadRequest(response);
            }
        }
        #endregion
    }
}
