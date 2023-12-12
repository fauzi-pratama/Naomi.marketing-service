using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.ApprovalService;
using Newtonsoft.Json;
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

        #region GetData
        [HttpGet("get_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<Tuple<List<ApprovalMappingView>, string>>>> GetApprovalMapping(Guid? companyId, string? companyCode)
        {
            ServiceResponse<List<ApprovalMappingView>> response = new();
            var apprvMapping = await _approvalService.GetApprovalMapping(companyId, companyCode);
            
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
        #endregion

        #region InsertData
        [HttpPost("add_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<Tuple<ApprovalMappingView, string>>>> AddApprovalMapping([FromBody] CreateApprovalMapping createApprovalMapping)
        {
            var msg = JsonConvert.SerializeObject(createApprovalMapping);
            _logger.LogInformation("Calling add_approval_mapping with params {msg}", msg);

            ServiceResponse<ApprovalMappingView> response = new();
            var newApprovalMappings = await _approvalService.InsertApprovalMapping(_mapper.Map<ApprovalMappingView>(createApprovalMapping));
            
            if (newApprovalMappings.Item1 != null && newApprovalMappings.Item1.Id != Guid.Empty)
            {
                response.Data = newApprovalMappings.Item1;
                response.Message = newApprovalMappings.Item2;

                _logger.LogInformation("Success add_approval_mapping with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = newApprovalMappings!.Item2;
                response.Success = false;

                _logger.LogError("Failed add_approval_mapping with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion

        #region EditData
        [HttpPut("edit_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<ApprovalMappingView>>> EditApprovalMapping([FromBody] UpdateApprovalMapping approvalUpdate)
        {
            var msg = JsonConvert.SerializeObject(approvalUpdate);
            _logger.LogInformation("Calling edit_approval_mapping with params {msg}", msg);

            ServiceResponse<ApprovalMappingView> response = new();
            var updateApproval = await _approvalService.UpdateApprovalMapping(_mapper.Map<ApprovalMappingView>(approvalUpdate));
            
            if (updateApproval.Item1 != null && updateApproval.Item1.Id != Guid.Empty)
            {
                response.Data = updateApproval.Item1;

                _logger.LogInformation("Success edit_approval_mapping with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = updateApproval.Item2;
                response.Success = false;

                _logger.LogError("Failed edit_approval_mapping with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion

        #region GeneratePromotionApproval
        [HttpPost("generate_approval_status")]
        public async Task<ActionResult<ServiceResponse<List<PromotionApproval>>>> GenerateApprovalStatus([FromBody] GeneratePromoApproval request)
        {
            var msg = JsonConvert.SerializeObject(request);
            _logger.LogInformation("Calling generate_approval_status with params {msg}", msg);

            ServiceResponse<List<PromotionApproval>> response = new();
            List<PromotionApproval> newPromoApprovalStatuses = await _approvalService.GeneratePromoApproval(request);
            
            if (newPromoApprovalStatuses != null)
            {
                response.Data = newPromoApprovalStatuses;

                _logger.LogInformation("Success generate_approval_status with params {msg}", msg);
                return Ok(response);
            }
            else
            {
                response.Message = "";
                response.Success = false;

                _logger.LogInformation("Failed generate_approval_status with params {msg}", msg);
                return BadRequest(response);
            }
        }
        #endregion
    }
}
