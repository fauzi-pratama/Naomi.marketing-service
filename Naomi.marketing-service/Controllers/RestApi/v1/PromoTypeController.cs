using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoTypeService;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;
using Newtonsoft.Json;

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

        #region GetData
        [HttpGet("get_promotion_type")]
        public async Task<ActionResult<ServiceResponse<List<PromotionType>>>> GetPromotionType(Guid id)
        {
            ServiceResponse<List<PromotionType>> response = new();
            List<PromotionType> promoType = await _promoTypeService.GetPromotionType(id);

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
        #endregion

        #region InsertData
        [HttpPost("add_promotion_type")]
        public async Task<ActionResult<ServiceResponse<PromotionType>>> AddPromotionType([FromBody] CreatePromotionType promotionType)
        {
            var msg = JsonConvert.SerializeObject(promotionType);
            _logger.LogInformation("Calling add_promotion_type with params {msg}", msg);

            ServiceResponse<PromotionType> response = new();
            var newPromoType = await _promoTypeService.InsertPromotionType(_mapper.Map<PromotionType>(promotionType));

            if (newPromoType.Item1 != null && newPromoType.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoType.Item1;

                _logger.LogInformation("Success add_promotion_type with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = newPromoType.Item2;
                response.Success = false;

                _logger.LogError("Failed add_promotion_type with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion

        #region UpdateData
        [HttpPut("update_promotion_type")]
        public async Task<ActionResult<ServiceResponse<PromotionType>>> UpdatePromotionType([FromBody] UpdatePromotionType promoTypeUpdate)
        {
            var msg = JsonConvert.SerializeObject(promoTypeUpdate);
            _logger.LogInformation("Calling update_promotion_type with params {msg}", msg);

            ServiceResponse<PromotionType> response = new();
            var updatePromoType = await _promoTypeService.UpdatePromotionType(_mapper.Map<PromotionType>(promoTypeUpdate));

            if (updatePromoType.Item1 != null && updatePromoType.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoType.Item1;

                _logger.LogInformation("Success update_promotion_type with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = updatePromoType.Item2;
                response.Success = false;

                _logger.LogError("Failed update_promotion_type with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion
    }
}
