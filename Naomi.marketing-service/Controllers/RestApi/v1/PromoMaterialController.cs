using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoChannelService;
using Naomi.marketing_service.Services.PromoMaterialService;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
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

        [HttpGet("get_promotion_material")]
        public async Task<ActionResult<ServiceResponse<List<PromotionMaterial>>>> GetPromotionMaterial(string searchName = "", int pageNo = 1, int pageSize = 10)
        {
            var promoMaterials = await _promoMaterialService.GetPromotionMaterial(searchName, pageNo, pageSize);
            ServiceResponse<List<PromotionMaterial>> response = new();

            if (promoMaterials != null && promoMaterials.Item1.Count > 0)
            {
                response.Data = promoMaterials.Item1;
                response.Pages = pageNo;
                response.TotalPages = promoMaterials.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpPost("add_promotion_material")]
        public async Task<ActionResult<ServiceResponse<PromotionMaterial>>> AddPromotionMaterial([FromBody] PromoMaterialRequest promotionMaterial)
        {
            var newPromoMaterial = await _promoMaterialService.InsertPromotionMaterial(_mapper.Map<PromotionMaterial>(promotionMaterial));
            ServiceResponse<PromotionMaterial> response = new();

            if (newPromoMaterial.Item1 != null && newPromoMaterial.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoMaterial.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = newPromoMaterial.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
