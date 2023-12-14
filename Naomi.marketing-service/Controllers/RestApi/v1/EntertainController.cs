using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
        [HttpGet("get_nip_entertain")]
        public async Task<ActionResult<ServiceResponse<List<NipEntertainResponse>>>> GetNipEntertainAsync(DateTime? monthYear, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_nip_entertain with Period: {monthYear} and search name: {searchName}", monthYear, searchName);

            ServiceResponse<List<NipEntertainResponse>> response = new();
            var nipEntertainList = await _entertainService.GetNipEntertain(monthYear, searchName, pageNo, pageSize);
            
            if (nipEntertainList != null && nipEntertainList.Item1.Count > 0)
            {
                response.Data = nipEntertainList.Item1;
                response.Pages = pageNo;
                response.TotalPages = nipEntertainList.Item2;

                _logger.LogInformation("Success get_nip_entertain with Period: {monthYear} and search name: {searchName}", monthYear, searchName);
                return Ok(response);
            }

            var msg = nipEntertainList!.Item3 == "" ? "Data not found" : nipEntertainList.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_nip_entertain with message: {msg} and Period: {monthYear} and search name: {searchName}", msg, monthYear, searchName);
            return NotFound(response);
        }
        [HttpGet("get_entertain_list")]
        public async Task<ActionResult<ServiceResponse<List<PromoEntertainListView>>>> GetEntertainListAsync([FromQuery] PromotionListRequest promoRequest)
        {
            _logger.LogInformation("Calling get_entertain_list with params: {promoRequest}", promoRequest);

            ServiceResponse<List<PromoEntertainListView>> response = new();
            var promoEntertainList = await _entertainService.GetPromoEntertainListAsync(promoRequest.OrderColumn!, promoRequest.OrderMethod!, promoRequest.PageNo, promoRequest.PageSize, promoRequest.Search);

            if (promoEntertainList.Item1.Count > 0)
            {
                response.Data = promoEntertainList.Item1;
                response.Pages = promoRequest.PageNo;
                response.TotalPages = promoEntertainList.Item2;

                _logger.LogInformation("Success get_entertain_list with params: {promoRequest}", promoRequest);
                return Ok(response);
            }

            var msg = promoEntertainList.Item3 == "" ? "Data not found" : promoEntertainList.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_entertain_list with message: {msg} and params: {promoRequest}", msg, promoRequest);
            return NotFound(response);
        }
        [HttpGet("get_entertain_by_nip")]
        public async Task<ActionResult<ServiceResponse<PromotionEntertain>>> GetEntertainByNipAsync(string? empNIP, DateTime? monthYear)
        {
            _logger.LogInformation("Calling get_entertain_by_nip with NIP: {empNIP} and Period: {monthYear}", empNIP, monthYear);

            ServiceResponse<PromotionEntertain> response = new();
            var promoEntertain = await _entertainService.GetPromoEntertainByNIP(empNIP, monthYear);

            if (promoEntertain != null && promoEntertain.Item1 != null && promoEntertain.Item1.Id != Guid.Empty)
            {
                response.Data = promoEntertain.Item1;

                _logger.LogInformation("Success get_entertain_by_nip with NIP: {empNIP} and Period: {monthYear}", empNIP, monthYear);
                return Ok(response);
            }

            var msg = string.IsNullOrEmpty(promoEntertain!.Item2) ? string.Format("Data not found or budget has not been set for {0}", empNIP) : promoEntertain.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_entertain_by_nip with message: {msg} and NIP: {empNIP} and Period: {monthYear}", msg, empNIP, monthYear);
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_entertain")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionEntertain, string>>>> CreateEntertainAsync([FromBody] CreateEntertain createEntertain)
        {
            _logger.LogInformation("Calling add_entertain with params: {createEntertain}", createEntertain);

            ServiceResponse<PromotionEntertain> response = new();
            var newEntertain = await _entertainService.CreateEntertain(createEntertain);

            if (newEntertain.Item1 != null && newEntertain.Item1.Id != Guid.Empty)
            {
                response.Data = newEntertain.Item1;

                _logger.LogInformation("Success add_entertain with params: {createEntertain}", createEntertain);
                return Ok(response);
            }

            var msg = newEntertain!.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed add_entertain with message: {msg} and params: {createEntertain}", msg, createEntertain);
            return BadRequest(response);
        }
        #endregion

        #region UpdateData
        [HttpPut("edit_entertain")]
        public async Task<ActionResult<ServiceResponse<PromotionEntertain>>> UpdateEntertainAsync([FromBody] UpdateEntertain updateEntertain)
        {
            _logger.LogInformation("Calling edit_entertain with params: {updateEntertain}", updateEntertain);

            ServiceResponse<PromotionEntertain> response = new();
            var entertainUpdated = await _entertainService.UpdateEntertain(updateEntertain);
            
            if (entertainUpdated != null && entertainUpdated.Item1.Id != Guid.Empty)
            {
                response.Data = entertainUpdated.Item1;

                _logger.LogInformation("Success edit_entertain with params: {updateEntertain}", updateEntertain);
                return Ok(response);
            }

            var msg = entertainUpdated!.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed edit_entertain with message: {msg} and params: {updateEntertain}", msg, updateEntertain);
            return BadRequest(response);
        }
        #endregion
    }
}
