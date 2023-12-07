using Amazon.Runtime.Internal;
using AutoMapper;
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
        public async Task<ActionResult<ServiceResponse<List<PromotionListView>>>> GetPromotionList([FromQuery] PromotionListRequest promoRequest)
        {
            var promoList = await _promoService.GetPromotionListAsync(promoRequest.OrderColumn!, promoRequest.OrderMethod!, promoRequest.PageNo, promoRequest.PageSize, promoRequest.Search);
            ServiceResponse<List<PromotionListView>> response = new()
            {
                Data = promoList.Item1
            };
    
            if (promoList != null && promoList.Item1.Count > 0)
            {
                response.Pages = promoRequest.PageNo;
                response.TotalPages = promoList.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = promoList!.Item3 == "" ? "Data not found" : promoList.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_promotion")]
        public async Task<ActionResult<ServiceResponse<PromotionViewModel>>> GetPromotionView(Guid promoHeaderId, string userId)
        {
            ServiceResponse<PromotionViewModel> response = new();

            if (promoHeaderId == Guid.Empty)
            {
                response.Message = "Promotion Id is required";
                response.Success = false;
                return BadRequest(response);
            }
            else
            {
                PromotionViewModel promoView = await _promoService.GetPromotionViewAsync(promoHeaderId, userId);
                if (promoView != null && promoView.Id != Guid.Empty)
                {
                    response.Data = promoView;
                    return Ok(response);
                }
                else
                {
                    response.Message = "Data not found";
                    response.Success = false;
                    return NotFound(response);
                }
            }
        }

        [HttpGet("get_promotion_approval_list")]
        public async Task<ActionResult<ServiceResponse<List<PromotionListView>>>> GetPromotionApprovalList([FromQuery] PromotionListRequest promoRequest, string userId)
        {
            var promoApprovalList = await _promoService.GetPromotionApprovalListAsync(userId, promoRequest.OrderColumn!, promoRequest.OrderMethod!, promoRequest.PageNo, promoRequest.PageSize, promoRequest.Search);
            ServiceResponse<List<PromotionListView>> response = new()
            {
                Data = promoApprovalList.Item1
            };

            if (promoApprovalList != null && promoApprovalList.Item1.Count > 0)
            {
                response.Pages = promoRequest.PageNo;
                response.TotalPages = promoApprovalList.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = promoApprovalList!.Item3 == "" ? "Data not found" : promoApprovalList.Item3;
                response.Success = false;
                return NotFound(response);
            }
        }
        #endregion

        #region InsertData
        [HttpPost("add_promo")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionHeader, string>>>> CreatePromotion([FromForm] CreatePromotion createPromotion)
        {
            _logger.LogInformation(string.Format("Calling add_promo with params {0}", JsonConvert.SerializeObject(createPromotion)));

            ServiceResponse<PromotionHeader> response = new();

            if (createPromotion == null)
            {
                response.Message = "Please check your data again";
                response.Success = false;

                _logger.LogInformation(string.Format("Failed calling add_promo with params {0}", JsonConvert.SerializeObject(createPromotion)));
                return BadRequest(response);
            }
            else
            {
                var newPromo = await _promoService.CreatePromotion(createPromotion);
                if (newPromo.Item1 != null && newPromo.Item1.Id! != Guid.Empty)
                {
                    response.Data = newPromo.Item1;

                    _logger.LogInformation(string.Format("Success add_promo with params {0}", JsonConvert.SerializeObject(createPromotion)));
                    return Ok(response);
                }
                else
                {
                    response.Message = newPromo.Item2;
                    response.Success = false;

                    _logger.LogError(string.Format("Failed add_promo with params {0}", JsonConvert.SerializeObject(createPromotion)));
                    return BadRequest(response);
                }
            }
        }
        #endregion

        #region UpdateData
        [HttpPut("edit_promo")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionHeader, string>>>> UpdatePromotion([FromForm] UpdatePromotion promoUpdate)
        {
            _logger.LogInformation(string.Format("Calling add_promo with params {0}", JsonConvert.SerializeObject(promoUpdate)));

            ServiceResponse<PromotionHeader> response = new();
            if (promoUpdate == null)
            {
                response.Message = "Please check your data again";
                response.Success = false;

                _logger.LogInformation(string.Format("Failed calling add_promo with params {0}", JsonConvert.SerializeObject(promoUpdate)));
                return BadRequest(response);
            }
            else
            {
                var updatePromo = await _promoService.UpdatePromotion(promoUpdate);

                if (updatePromo.Item1 != null && updatePromo.Item1.Id != Guid.Empty)
                {
                    response.Data = updatePromo.Item1;
                    response.Message = updatePromo.Item2;

                    _logger.LogInformation(string.Format("Success add_promo with params {0}", JsonConvert.SerializeObject(promoUpdate)));
                    return Ok(response);
                }
                else
                {
                    response.Message = updatePromo.Item2;
                    response.Success = false;

                    _logger.LogError(string.Format("Failed add_promo with params {0}", JsonConvert.SerializeObject(promoUpdate)));
                    return BadRequest(response);
                }
            }
        }
        #endregion

        #region SetActiveInactive
        [HttpPut("set_active_promo")]
        public async Task<ActionResult<ServiceResponse<PromotionHeader>>> SetActivePromo(Guid promoId, bool activeFlag = true)
        {
            _logger.LogInformation(string.Format("Calling set_active_promo with promo Id: {0} and activeFlag: {1}", promoId, activeFlag));

            ServiceResponse<PromotionHeader> response = new();
            if (promoId == Guid.Empty)
            {
                response.Message = "Promotion Id is required";
                response.Success = false;

                _logger.LogInformation(string.Format("Failed calling set_active_promo with promo Id: {0} and activeFlag: {1}", promoId, activeFlag));
                return BadRequest(response);
            }
            else
            {
                PromotionHeader promoHeader = await _promoService.UpdateActivePromo(promoId, activeFlag);
                if (promoHeader != null && promoHeader.Id != Guid.Empty)
                {
                    response.Data = promoHeader;

                    _logger.LogInformation(string.Format("Success set_active_promo with promo Id: {0} and activeFlag: {1}", promoId, activeFlag));
                    return Ok(response);
                }
                else
                {
                    response.Message = "";
                    response.Success = false;

                    _logger.LogInformation(string.Format("Failed set_active_promo with promo Id: {0} and activeFlag: {1}", promoId, activeFlag));
                    return BadRequest(response);
                }
            }

        }
        #endregion

        #region ApproveReject
        [HttpPut("approve_reject_promotion")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionApprovalDetail, string>>>> ApproveRejectPromotion([FromQuery] ApproveRejectPromotion approvalStatus)
        {
            _logger.LogInformation(string.Format("Calling approve_reject_promotion with params {0}", JsonConvert.SerializeObject(approvalStatus)));

            ServiceResponse<PromotionApprovalDetail> response = new();
            if (approvalStatus == null)
            {
                response.Message = "Please check your data again";
                response.Success = false;

                _logger.LogError(string.Format("Failed calling approve_reject_promotion with params {0}", JsonConvert.SerializeObject(approvalStatus)));
                return BadRequest(response);
            }
            else
            {
                var approvalMapping = await _promoService.ApproveRejectPromotion(approvalStatus);
                if (approvalMapping.Item1 != null && approvalMapping.Item1.Id! != Guid.Empty)
                {
                    response.Data = approvalMapping.Item1!;

                    _logger.LogInformation(string.Format("Success approve_reject_promotion with params {0}", JsonConvert.SerializeObject(approvalStatus)));
                    return Ok(response);
                }
                else
                {
                    response.Message = approvalMapping.Item2;
                    response.Success = false;

                    _logger.LogError(string.Format("Failed approve_reject_promotion with params {0}", JsonConvert.SerializeObject(approvalStatus)));
                    return BadRequest(response);
                }
            }
        }
        #endregion
    }
}
