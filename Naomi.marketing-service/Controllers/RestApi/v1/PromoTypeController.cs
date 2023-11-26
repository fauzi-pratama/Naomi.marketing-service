using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoTypeService;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class PromoTypeController : ControllerBase
    {
        public readonly IPromoTypeService _promoTypeService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoTypeController> _logger;

        public PromoTypeController(IPromoTypeService promoTypeService, IMapper mapper, ILogger<PromoTypeController> logger)
        {
            _promoTypeService = promoTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get_promotion_type")]
        public async Task<ActionResult<ServiceResponse<List<PromotionType>>>> GetPromotionType(Guid id)
        {
            List<PromotionType> promoType = await _promoTypeService.GetPromotionType(id);
            ServiceResponse<List<PromotionType>> response = new();

            if (promoType != null && promoType.Count > 0)
            {
                response.Data = promoType;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_promotion_type_by_class")]
        public async Task<ActionResult<ServiceResponse<List<PromotionType>>>> GetPromotionTypeByClass(Guid classId)
        {
            ServiceResponse<List<PromotionType>> response = new();
            var promoType = await _promoTypeService.GetPromotionTypeByClassIdAsync(classId);

            if (promoType.Item1 != null && promoType.Item1.Count > 0)
            {
                response.Data = promoType.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = promoType.Item2 == "" ? "Data not found" : promoType.Item2;
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpPost("add_promotion_type")]
        public async Task<ActionResult<ServiceResponse<PromotionType>>> AddPromotionType([FromBody] CreatePromotionType promotionType)
        {
            var newPromoType = await _promoTypeService.InsertPromotionType(_mapper.Map<PromotionType>(promotionType));
            ServiceResponse<PromotionType> response = new();

            if (newPromoType.Item1 != null && newPromoType.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoType.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = newPromoType.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }

        [HttpPut("update_promotion_type")]
        public async Task<ActionResult<ServiceResponse<PromotionType>>> UpdatePromotionType([FromBody] UpdatePromotionType promoTypeUpdate)
        {
            var updatePromoType = await _promoTypeService.UpdatePromotionType(_mapper.Map<PromotionType>(promoTypeUpdate));
            ServiceResponse<PromotionType> response = new();

            if (updatePromoType.Item1 != null && updatePromoType.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoType.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = updatePromoType.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
