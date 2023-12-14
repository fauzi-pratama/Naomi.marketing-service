using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoClassService;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
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
        public async Task<ActionResult<ServiceResponse<List<PromotionClass>>>> GetPromotionClassAsync(Guid id)
        {
            _logger.LogInformation("Calling get_promotion_class with Id: {id}", id);
            ServiceResponse<List<PromotionClass>> response = new();
            List<PromotionClass> promoClass = await _promoClassService.GetPromotionClass(id);

            if (promoClass != null && promoClass.Count > 0)
            {
                response.Data = promoClass;

                _logger.LogInformation("Success get_promotion_class with Id: {id}", id);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_promotion_class with Id: {id}", id);
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_promotion_class")]
        public async Task<ActionResult<ServiceResponse<PromotionClass>>> AddPromotionClassAsync([FromBody] CreatePromotionClass promotionClass)
        {
            var param = JsonConvert.SerializeObject(promotionClass);
            _logger.LogInformation("Calling add_promotion_class with params: {param}", param);

            ServiceResponse<PromotionClass> response = new();
            var newPromoClass = await _promoClassService.InsertPromotionClass(_mapper.Map<PromotionClass>(promotionClass));
            
            if (newPromoClass.Item1 != null && newPromoClass.Item1.Id != Guid.Empty)
            {
                response.Data = newPromoClass.Item1;

                _logger.LogInformation("Success add_promotion_class with params: {param}", param);
                return Ok(response);
            }

            var msg = newPromoClass.Item2 == "" ? "Data not found" : newPromoClass.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed add_promotion_class with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion

        #region UpdateData
        [HttpPut("update_promotion_class")]
        public async Task<ActionResult<ServiceResponse<PromotionClass>>> UpdatePromotionClassAsync([FromBody] UpdatePromotionClass promoClassUpdate)
        {
            var param = JsonConvert.SerializeObject(promoClassUpdate);
            _logger.LogInformation("Calling update_promotion_class with params: {param}", param);

            ServiceResponse<PromotionClass> response = new();
            var updatePromoClass = await _promoClassService.UpdatePromotionClass(_mapper.Map<PromotionClass>(promoClassUpdate));

            if (updatePromoClass.Item1 != null && updatePromoClass.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoClass.Item1;

                _logger.LogInformation("Sucess update_promotion_class with params: {param}", param);
                return Ok(response);
            }

            var msg = updatePromoClass!.Item2 == "" ? "Data not found" : updatePromoClass.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed update_promotion_class with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion
    }
}
