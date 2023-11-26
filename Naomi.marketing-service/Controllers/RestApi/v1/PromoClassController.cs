using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoClassService;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class PromoClassController : ControllerBase
    {
        public readonly IPromoClassService _promoClassService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoClassController> _logger;

        public PromoClassController(IPromoClassService promoClassService, IMapper mapper, ILogger<PromoClassController> logger)
        {
            _promoClassService = promoClassService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get_promotion_class")]
        public async Task<ActionResult<ServiceResponse<List<PromotionClass>>>> GetPromotionClass(Guid id)
        {
            ServiceResponse<List<PromotionClass>> response = new();
            List<PromotionClass> promoClass = await _promoClassService.GetPromotionClass(id);

            if (promoClass != null && promoClass.Count > 0)
            {
                response.Data = promoClass;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpPost("add_promotion_class")]
        public async Task<ActionResult<ServiceResponse<PromotionClass>>> AddPromotionClass([FromBody] CreatePromotionClass promotionClass)
        {
            ServiceResponse<PromotionClass> response = new();
            var newPromoClass = await _promoClassService.InsertPromotionClass(_mapper.Map<PromotionClass>(promotionClass));
            
            if (newPromoClass.Item1 != null && newPromoClass.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoClass.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = newPromoClass.Item2 == "" ? "Data not found" : newPromoClass.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }

        [HttpPut("update_promotion_class")]
        public async Task<ActionResult<ServiceResponse<PromotionClass>>> UpdatePromotionClass([FromBody] UpdatePromotionClass promoClassUpdate)
        {
            ServiceResponse<PromotionClass> response = new();
            var updatePromoClass = await _promoClassService.UpdatePromotionClass(_mapper.Map<PromotionClass>(promoClassUpdate));

            if (updatePromoClass.Item1 != null && updatePromoClass.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoClass.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = updatePromoClass!.Item2 == "" ? "Data not found" : updatePromoClass.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
