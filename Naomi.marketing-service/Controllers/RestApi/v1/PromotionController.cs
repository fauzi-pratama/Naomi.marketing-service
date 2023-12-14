using Amazon.Runtime.Internal;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Pub;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromotionService;
using Newtonsoft.Json;
using static Naomi.marketing_service.Models.Response.PromotionHeaderResponse;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
    [Route("/v1/")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        public readonly IPromotionService _promoService;
        public readonly IMapper _mapper;
        public readonly ILogger<PromotionController> _logger;

        public PromotionController(IPromotionService promoService, IMapper mapper, ILogger<PromotionController> logger)
        {
            _promoService = promoService;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_data")]
        public async Task<ActionResult<ServiceResponse<List<PromotionListView>>>> GetPromotionListAsync([FromQuery] PromotionListRequest promoRequest)
        {
            _logger.LogInformation("Calling get_data with params: {promoRequest}", promoRequest);

            ServiceResponse<List<PromotionListView>> response = new();
            var promoList = await _promoService.GetPromotionListAsync(promoRequest.OrderColumn!, promoRequest.OrderMethod!, promoRequest.PageNo, promoRequest.PageSize, promoRequest.Search);
    
            if (promoList != null && promoList.Item1.Count > 0)
            {
                response.Data = promoList.Item1;
                response.Pages = promoRequest.PageNo;
                response.TotalPages = promoList.Item2;

                _logger.LogInformation("Success get_data with params: {promoRequest}", promoRequest);
                return Ok(response);
            }

            var msg = promoList!.Item3 == "" ? "Data not found" : promoList.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_data with message: {msg} and params: {promoRequest}", msg, promoRequest);
            return NotFound(response);
        }

        [HttpGet("get_promotion")]
        public async Task<ActionResult<ServiceResponse<PromotionViewModel>>> GetPromotionViewAsync(Guid promoHeaderId, string userId)
        {
            _logger.LogInformation("Calling get_promotion with Id: {promoHeaderId} and user Id: {userId}", promoHeaderId, userId);
            ServiceResponse<PromotionViewModel> response = new();
            var msg = "";

            if (promoHeaderId == Guid.Empty)
            {
                msg = "Promotion Id is required";
                response.Message = msg;
                response.Success = false;

                _logger.LogInformation("Failed get_promotion with message: {msg} and Id: {promoHeaderId} and user Id: {userId}", msg, userId, promoHeaderId);
                return BadRequest(response);
            }

            PromotionViewModel promoView = await _promoService.GetPromotionViewAsync(promoHeaderId, userId);
            if (promoView != null && promoView.Id != Guid.Empty)
            {
                response.Data = promoView;

                _logger.LogInformation("Calling get_promotion with Id: {promoHeaderId} and user Id: {userId}", promoHeaderId, userId);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_promotion with Id: {promoHeaderId} and user Id: {userId}", promoHeaderId, userId);
            return NotFound(response);
        }

        [HttpGet("get_promotion_approval_list")]
        public async Task<ActionResult<ServiceResponse<List<PromotionListView>>>> GetPromotionApprovalListAsync([FromQuery] PromotionListRequest promoRequest, string userId)
        {
            _logger.LogInformation("Calling get_promotion_approval_list with params: {promoRequest} and user Id: {userId}", promoRequest, userId);

            var promoApprovalList = await _promoService.GetPromotionApprovalListAsync(userId, promoRequest.OrderColumn!, promoRequest.OrderMethod!, promoRequest.PageNo, promoRequest.PageSize, promoRequest.Search);
            ServiceResponse<List<PromotionListView>> response = new();

            if (promoApprovalList != null && promoApprovalList.Item1.Count > 0)
            {
                response.Data = promoApprovalList.Item1;
                response.Pages = promoRequest.PageNo;
                response.TotalPages = promoApprovalList.Item2;

                _logger.LogInformation("Success get_promotion_approval_list with params: {promoRequest} and user Id: {userId}", promoRequest, userId);
                return Ok(response);
            }

            var msg = promoApprovalList!.Item3 == "" ? "Data not found" : promoApprovalList.Item3;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_promotion_approval_list with message: {msg} and params: {promoRequest} and user Id: {userId}", msg, promoRequest, userId);
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_promo")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionHeader, string>>>> CreatePromotionAsync([FromForm] CreatePromotion createPromotion)
        {
            var param = JsonConvert.SerializeObject(createPromotion);
            _logger.LogInformation("Calling add_promo with params {param}", param);

            ServiceResponse<PromotionHeader> response = new();
            var msg = "";

            if (createPromotion == null)
            {
                msg = "Please check your data again";
                response.Message = msg;
                response.Success = false;

                _logger.LogInformation("Failed add_promo with message: {msg} and params {param}", msg, param);
                return BadRequest(response);
            }

            var newPromo = await _promoService.CreatePromotion(createPromotion);
            if (newPromo.Item1 != null && newPromo.Item1.Id! != Guid.Empty)
            {
                response.Data = newPromo.Item1;

                _logger.LogInformation("Success add_promo with params {param}", param);
                return Ok(response);
            }

            msg = newPromo.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed add_promo with message: {msg} and params {param}", msg, param);
            return BadRequest(response);
        }
        #endregion

        #region UpdateData
        [HttpPut("edit_promo")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionHeader, string>>>> UpdatePromotionAsync([FromForm] UpdatePromotion promoUpdate)
        {
            var param = JsonConvert.SerializeObject(promoUpdate);
            _logger.LogInformation("Calling add_promo with params: {param}", param);
            var msg = "";

            ServiceResponse<PromotionHeader> response = new();
            if (promoUpdate == null)
            {
                msg = "Please check your data again";
                response.Message = msg;
                response.Success = false;

                _logger.LogInformation("Failed add_promo with message: {msg} and params: {param}", msg, param);
                return BadRequest(response);
            }

            var updatePromo = await _promoService.UpdatePromotion(promoUpdate);

            if (updatePromo.Item1 != null && updatePromo.Item1.Id != Guid.Empty)
            {
                response.Data = updatePromo.Item1;
                response.Message = updatePromo.Item2;

                _logger.LogInformation("Success add_promo with params: {param}", param);
                return Ok(response);
            }

            msg = updatePromo.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed add_promo with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion

        #region SetActiveInactive
        [HttpPut("set_active_promo")]
        public async Task<ActionResult<ServiceResponse<PromotionHeader>>> SetActivePromoAsync(Guid promoId, string? username, bool activeFlag = true)
        {
            _logger.LogInformation("Calling set_active_promo with promo Id: {promoId} and activeFlag: {activeFlag}", promoId, activeFlag);

            ServiceResponse<PromotionHeader> response = new();
            
            if (promoId == Guid.Empty)
            {
                var msg = "Promotion Id is required";
                response.Message = msg;
                response.Success = false;

                _logger.LogInformation("Failed set_active_promo with message: {msg} and promo Id: {promoId} and activeFlag: {activeFlag}", msg, promoId, activeFlag);
                return BadRequest(response);
            }

            PromotionHeader promoHeader = await _promoService.UpdateActivePromo(promoId, username, activeFlag);
            if (promoHeader != null && promoHeader.Id != Guid.Empty)
            {
                response.Data = promoHeader;

                _logger.LogInformation("Success set_active_promo with promo Id: {promoId} and activeFlag: {activeFlag}", promoId, activeFlag);
                return Ok(response);
            }

            response.Message = "";
            response.Success = false;

            _logger.LogInformation("Failed set_active_promo with promo Id: {promoId} and activeFlag: {activeFlag}", promoId, activeFlag);
            return BadRequest(response);

        }
        #endregion

        #region ApproveReject
        [HttpPut("approve_reject_promotion")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionApprovalDetail, string>>>> ApproveRejectPromotionAsync([FromQuery] ApproveRejectPromotion approvalStatus)
        {
            var param = JsonConvert.SerializeObject(approvalStatus);
            _logger.LogInformation("Calling approve_reject_promotion with params: {param}", param);

            ServiceResponse<PromotionApprovalDetail> response = new();
            var msg = "";

            if (approvalStatus == null)
            {
                msg = "Please check your data again";
                response.Message = msg;
                response.Success = false;

                _logger.LogError("Failed approve_reject_promotion with message: {msg} and params: {param}", msg, param);
                return BadRequest(response);
            }

            var approvalMapping = await _promoService.ApproveRejectPromotion(approvalStatus);
            if (approvalMapping.Item1 != null && approvalMapping.Item1.Id! != Guid.Empty)
            {
                response.Data = approvalMapping.Item1!;

                _logger.LogInformation("Success approve_reject_promotion with params: {param}", param);
                return Ok(response);
            }

            msg = approvalMapping.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed approve_reject_promotion with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion
    }
}
