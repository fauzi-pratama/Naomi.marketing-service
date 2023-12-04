using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Controllers.MessageHandler;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.ApprovalService;
using Naomi.marketing_service.Services.PromoAppService;
using Naomi.marketing_service.Services.PromoTypeService;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        public readonly IApprovalService _approvalService;
        public readonly IMapper _mapper;
        public readonly ILogger<ApprovalController> _logger;

        public ApprovalController(IApprovalService approvalService, IMapper mapper, ILogger<ApprovalController> logger)
        {
            _approvalService = approvalService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("get_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<Tuple<List<ApprovalMappingView>, string>>>> GetApprovalMapping(Guid? companyId, string? companyCode)
        {
            var apprvMapping = await _approvalService.GetApprovalMapping(companyId, companyCode);
            ServiceResponse<List<ApprovalMappingView>> response = new();

            if (apprvMapping != null && apprvMapping.Item1.Count > 0)
            {
                response.Data = apprvMapping.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = string.IsNullOrEmpty(apprvMapping!.Item2) ? "Data not found" : apprvMapping!.Item2;
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpGet("get_approval_mapping_by_id")]
        public async Task<ActionResult<ServiceResponse<ApprovalMappingView?>>> GetApprovalMappingById(Guid approvalMappingId)
        {
            ServiceResponse<ApprovalMappingView> response = new();
            var apprvMapping = await _approvalService.GetApprovalMappingById(approvalMappingId);
            if (apprvMapping.Item1 != null && apprvMapping.Item1.Id != null && apprvMapping.Item1!.Id != Guid.Empty)
            {
                response.Data = apprvMapping.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = string.IsNullOrEmpty(apprvMapping!.Item2) ? "Data not found" : apprvMapping!.Item2;
                response.Success = false;
                return NotFound(response);
            }
        }

        [HttpPost("add_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<Tuple<ApprovalMappingView, string>>>> AddApprovalMapping([FromBody] CreateApprovalMapping createApprovalMapping)
        {
            var newApprovalMappings = await _approvalService.InsertApprovalMapping(_mapper.Map<ApprovalMappingView>(createApprovalMapping));
            ServiceResponse<ApprovalMappingView> response = new();

            if (newApprovalMappings.Item1 != null && newApprovalMappings.Item1.Id != Guid.Empty)
            {
                response.Data = newApprovalMappings.Item1;
                response.Message = newApprovalMappings.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = newApprovalMappings!.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }

        [HttpPut("approve_reject_promotion")]
        public async Task<ActionResult<ServiceResponse<Tuple<PromotionApprovalDetail, string>>>> ApproveRejectPromotion([FromBody] ApproveRejectPromotion approvalStatus)
        {
            ServiceResponse<PromotionApprovalDetail> response = new();

            if (approvalStatus == null)
            {
                response.Message = "Please check your data again";
                response.Success = false;
                return BadRequest(response);
            }
            else
            {
                var approvalMapping = await _approvalService.ApproveRejectPromotion(approvalStatus);
                if (approvalMapping.Item1 != null && approvalMapping.Item1.Id! != Guid.Empty)
                {
                    response.Data = approvalMapping.Item1!;
                    return Ok(response);
                }
                else
                {
                    response.Message = approvalMapping.Item2;
                    response.Success = false;
                    return BadRequest(response);
                }
            }
        }

        [HttpPut("edit_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<ApprovalMappingView>>> EditApprovalMapping([FromBody] UpdateApprovalMapping approvalUpdate)
        {
            var updateApproval = await _approvalService.UpdateApprovalMapping(_mapper.Map<ApprovalMappingView>(approvalUpdate));
            ServiceResponse<ApprovalMappingView> response = new();

            if (updateApproval.Item1 != null && updateApproval.Item1.Id != Guid.Empty)
            {
                response.Data = updateApproval.Item1;
                return Ok(response);
            }
            else
            {
                response.Message = updateApproval.Item2;
                response.Success = false;
                return BadRequest(response);
            }
        }
    }
}
