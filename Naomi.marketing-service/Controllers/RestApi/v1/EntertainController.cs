using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.ApprovalService;
using Naomi.marketing_service.Services.EntertainService;
using static Naomi.marketing_service.Models.Request.EntertainBudgetRequest;
using static Naomi.marketing_service.Models.Response.EntertainBudgetResponse;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class EntertainController : ControllerBase
    {
        public readonly IEntertainService _entertainService;
        public readonly ILogger<EntertainController> _logger;

        public EntertainController(IEntertainService entertainService, ILogger<EntertainController> logger)
        {
            _entertainService = entertainService;
            _logger = logger;
        }

        #region GetData
        [HttpGet("getNipEntertain")]
        public async Task<ActionResult<ServiceResponse<List<NipEntertainResponse>>>> GetNipEntertain(DateTime? monthYear, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            var nipEntertainList = await _entertainService.GetNipEntertain(monthYear, searchName, pageNo, pageSize);
            ServiceResponse<List<NipEntertainResponse>> response = new()
            {
                Data = nipEntertainList.Item1
            };

            if (nipEntertainList != null && nipEntertainList.Item1.Count > 0)
            {
                response.Pages = pageNo;
                response.TotalPages = nipEntertainList.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = nipEntertainList!.Item3 == "" ? "Data not found" : nipEntertainList.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }
        [HttpGet("getEntertainList")]
        public async Task<ActionResult<ServiceResponse<List<PromoEntertainListView>>>> GetEntertainList([FromQuery] PromotionListRequest promoRequest)
        {
            var promoEntertainList = await _entertainService.GetPromoEntertainListAsync(promoRequest.OrderColumn!, promoRequest.OrderMethod!, promoRequest.PageNo, promoRequest.PageSize, promoRequest.Search);
            ServiceResponse<List<PromoEntertainListView>> response = new();

            response.Data = promoEntertainList.Item1;
            if (promoEntertainList.Item1.Count > 0)
            {
                response.Pages = promoRequest.PageNo;
                response.TotalPages = promoEntertainList.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = promoEntertainList.Item3 == "" ? "Data not found" : promoEntertainList.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }
        [HttpGet("getEntertainByNip")]
        public async Task<ActionResult<ServiceResponse<PromotionEntertain>>> GetEntertainByNip(string? empNIP, DateTime? monthYear)
        {
            ServiceResponse<PromotionEntertain> response = new();

            var promoEntertain = await _entertainService.GetPromoEntertainByNIP(empNIP, monthYear);
            if (promoEntertain != null && promoEntertain.Item1 != null && promoEntertain.Item1.Id != Guid.Empty)
            {
                response.Data = promoEntertain.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = string.IsNullOrEmpty(promoEntertain!.Item2) ? string.Format("Data not found or budget has not been set for {0}", empNIP) : promoEntertain.Item2;
                response.Success = false;
                return NotFound(response);
            }
        }
        #endregion

        #region InsertData
        [HttpPost("addEntertain")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionEntertain, string>>>> CreateEntertain([FromBody] CreateEntertain createEntertain)
        {
            ServiceResponse<PromotionEntertain> response = new();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createEntertain == null)
            {
                response.Message = "Please check your data again";
                response.Success = false;
                return BadRequest(response);
            }

            var newEntertain = await _entertainService.CreateEntertain(createEntertain);
            if (newEntertain.Item1 != null && newEntertain.Item1.Id != Guid.Empty)
            {
                response.Data = newEntertain.Item1;
                return Ok(response);
            }

            response.Message = newEntertain!.Item2;
            response.Success = false;
            return BadRequest(response);
        }
        #endregion

        #region UpdateData
        [HttpPut("editEntertain")]
        public async Task<ActionResult<ServiceResponse<PromotionEntertain>>> UpdateEntertain([FromBody] UpdateEntertain updateEntertain)
        {
            var entertainUpdated = await _entertainService.UpdateEntertain(updateEntertain);
            ServiceResponse<PromotionEntertain> response = new();

            if (entertainUpdated != null && entertainUpdated.Item1.Id != Guid.Empty)
            {
                response.Data = entertainUpdated.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = entertainUpdated!.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
        #endregion
    }
}
