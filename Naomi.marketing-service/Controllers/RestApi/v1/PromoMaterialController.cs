using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoChannelService;
using Naomi.marketing_service.Services.PromoMaterialService;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
    [Route("/v1/")]
    [ApiController]
    public class PromoMaterialController : ControllerBase
    {
        public readonly IPromoMaterialService _promoMaterialService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoMaterialController> _logger;
        public PromoMaterialController(IPromoMaterialService promoMaterialService, IMapper mapper, ILogger<PromoMaterialController> logger)
        {
            _promoMaterialService = promoMaterialService;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_promotion_material")]
        public async Task<ActionResult<ServiceResponse<List<PromotionMaterial>>>> GetPromotionMaterialAsync(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_promotion_material with Search name: {searchName}", searchName);

            ServiceResponse<List<PromotionMaterial>> response = new();
            var promoMaterials = await _promoMaterialService.GetPromotionMaterial(searchName, pageNo, pageSize);

            if (promoMaterials != null && promoMaterials.Item1.Count > 0)
            {
                response.Data = promoMaterials.Item1;
                response.Pages = pageNo;
                response.TotalPages = promoMaterials.Item2;

                _logger.LogInformation("Success get_promotion_material with Search name: {searchName}", searchName);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_promotion_material with Search name: {searchName}", searchName);
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_promotion_material")]
        public async Task<ActionResult<ServiceResponse<PromotionMaterial>>> AddPromotionMaterialAsync([FromBody] PromoMaterialRequest promotionMaterial)
        {
            var param = JsonConvert.SerializeObject(promotionMaterial);
            _logger.LogInformation("Calling add_promotion_material with params: {param}", param);

            ServiceResponse<PromotionMaterial> response = new();
            var newPromoMaterial = await _promoMaterialService.InsertPromotionMaterial(_mapper.Map<PromotionMaterial>(promotionMaterial));

            if (newPromoMaterial.Item1 != null && newPromoMaterial.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoMaterial.Item1;

                _logger.LogInformation("Success add_promotion_material with params: {param}", param);
                return Ok(response);
            }

            response.Message = newPromoMaterial.Item2;
            response.Success = false;

            _logger.LogError("Failed add_promotion_material with params: {param}", param);
            return BadRequest(response);
        }
        #endregion
    }
}
