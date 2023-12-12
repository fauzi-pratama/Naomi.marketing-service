using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoClassService;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;
using Newtonsoft.Json;

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

        #region GetData
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
        #endregion

        #region InsertData
        [HttpPost("add_promotion_class")]
        public async Task<ActionResult<ServiceResponse<PromotionClass>>> AddPromotionClass([FromBody] CreatePromotionClass promotionClass)
        {
            var msg = JsonConvert.SerializeObject(promotionClass);
            _logger.LogInformation("Calling add_promotion_class with params {msg}", msg);

            ServiceResponse<PromotionClass> response = new();
            var newPromoClass = await _promoClassService.InsertPromotionClass(_mapper.Map<PromotionClass>(promotionClass));
            
            if (newPromoClass.Item1 != null && newPromoClass.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoClass.Item1;

                _logger.LogInformation("Success add_promotion_class with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = newPromoClass.Item2 == "" ? "Data not found" : newPromoClass.Item2;
                response.Success = false;

                _logger.LogError("Failed add_promotion_class with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion

        #region UpdateData
        [HttpPut("update_promotion_class")]
        public async Task<ActionResult<ServiceResponse<PromotionClass>>> UpdatePromotionClass([FromBody] UpdatePromotionClass promoClassUpdate)
        {
            var msg = JsonConvert.SerializeObject(promoClassUpdate);
            _logger.LogInformation("Calling update_promotion_class with params {msg}", msg);

            ServiceResponse<PromotionClass> response = new();
            var updatePromoClass = await _promoClassService.UpdatePromotionClass(_mapper.Map<PromotionClass>(promoClassUpdate));

            if (updatePromoClass.Item1 != null && updatePromoClass.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoClass.Item1;

                _logger.LogInformation("Sucess update_promotion_class with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = updatePromoClass!.Item2 == "" ? "Data not found" : updatePromoClass.Item2;
                response.Success = false;

                _logger.LogError("Failed update_promotion_class with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion
    }
}
