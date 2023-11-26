﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoAppService;
using Naomi.marketing_service.Services.PromoClassService;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class PromoAppController : ControllerBase
    {
        public readonly IPromoAppService _promoAppService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromoAppController> _logger;

        public PromoAppController(IPromoAppService promoAppService, IMapper mapper, ILogger<PromoAppController> logger) 
        {
            _promoAppService = promoAppService;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet("get_promo_app_display")]
        public async Task<ActionResult<ServiceResponse<List<PromotionAppDisplay>>>> GetPromotionAppDisplay(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            var promoAppDisplay = await _promoAppService.GetPromotionAppDisplay(searchName, pageNo, pageSize);
            ServiceResponse<List<PromotionAppDisplay>> response = new();

            response.Data = promoAppDisplay.Item1;
            if (promoAppDisplay != null && promoAppDisplay.Item1.Count > 0)
            {
                response.Pages = pageNo;
                response.TotalPages = promoAppDisplay.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpPost("add_promotion_app_display")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionAppDisplay, string>>>> AddPromotionAppDisplay([FromBody] AppDisplayRequest promoAppDisplay)
        {
            var newAppDisplay = await _promoAppService.InsertPromotionAppDisplay(_mapper.Map<PromotionAppDisplay>(promoAppDisplay));
            ServiceResponse<PromotionAppDisplay> response = new();

            if (newAppDisplay.Item1 != null && newAppDisplay.Item1.Id != Guid.Empty)
            {
                response.Data = newAppDisplay.Item1!;
                return Ok(response);
            }
            else
            {
                response.Message = newAppDisplay.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }

        [HttpPut("edit_promotion_app_display")]
        public async Task<ActionResult<ServiceResponse<PromotionAppDisplay>>> EditPromotionAppDisplay([FromBody] AppDisplayEditRequest promoAppDisplay)
        {
            var updatePromoAppDisplay = await _promoAppService.UpdatePromotionAppDisplay(_mapper.Map<PromotionAppDisplay>(promoAppDisplay));
            ServiceResponse<PromotionAppDisplay> response = new();

            if (updatePromoAppDisplay.Item1 != null && updatePromoAppDisplay.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromoAppDisplay.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = updatePromoAppDisplay!.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
