using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.ApprovalService;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Linq;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;


namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
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
        public async Task<ActionResult<ServiceResponse<Tuple<List<ApprovalMappingView>, string>>>> GetApprovalMappingAsync(Guid? companyId, string? companyCode)
        {
            _logger.LogInformation("Calling get_approval_mapping with Company Id: {companyId} or Company Code: {companyCode}", companyId, companyCode);

            ServiceResponse<List<ApprovalMappingView>> response = new();

            //(bool validAccess, string accessMessage) = ValidateAccess(Request, "APPROVAL_INDEX");
            //if (!validAccess)
            //{
            //    response.Message = accessMessage;
            //    response.Success = false;

            //    _logger.LogInformation("Failed get_approval_mapping with message: {accessMessage} and params: Company Id = {companyId} or Company Code = {companyCode}", accessMessage, companyId, companyCode);
            //    return NotFound(response);
            //}

            var apprvMapping = await _approvalService.GetApprovalMapping(companyId, companyCode);
            if (apprvMapping != null && apprvMapping.Item1.Count > 0)
            {
                response.Data = apprvMapping.Item1;

                _logger.LogInformation("Success get_approval_mapping with Company Id: {companyId} or Company Code: {companyCode}", companyId, companyCode);
                return Ok(response);
            }

            var msg = string.IsNullOrEmpty(apprvMapping!.Item2) ? "Data not found" : apprvMapping!.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_approval_mapping with message: {msg} and params: Company Id = {companyId} or Company Code = {companyCode}", msg, companyId, companyCode);
            return NotFound(response);
        }

        [HttpGet("get_approval_mapping_by_id")]
        public async Task<ActionResult<ServiceResponse<ApprovalMappingView?>>> GetApprovalMappingByIdAsync(Guid approvalMappingId)
        {
            _logger.LogInformation("Calling get_approval_mapping_by_id with Approval Mapping Id: {approvalMappingId}", approvalMappingId);

            ServiceResponse<ApprovalMappingView> response = new();

            var apprvMapping = await _approvalService.GetApprovalMappingById(approvalMappingId);
            if (apprvMapping.Item1 != null && apprvMapping.Item1.Id != null && apprvMapping.Item1!.Id != Guid.Empty)
            {
                response.Data = apprvMapping.Item1;

                _logger.LogInformation("Success get_approval_mapping_by_id with Approval Mapping Id: {approvalMappingId}", approvalMappingId);
                return Ok(response);
            }

            var msg = string.IsNullOrEmpty(apprvMapping!.Item2) ? "Data not found" : apprvMapping!.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogInformation("Failed get_approval_mapping_by_id with message: {msg} and Approval Mapping Id: {approvalMappingId}", msg, approvalMappingId);
            return NotFound(response);
        }
        #endregion

        #region InsertData
        [HttpPost("add_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<Tuple<ApprovalMappingView, string>>>> AddApprovalMappingAsync([FromBody] CreateApprovalMapping createApprovalMapping)
        {
            var param = JsonConvert.SerializeObject(createApprovalMapping);
            _logger.LogInformation("Calling add_approval_mapping with params: {param}", param);

            ServiceResponse<ApprovalMappingView> response = new();
            var newApprovalMappings = await _approvalService.InsertApprovalMapping(_mapper.Map<ApprovalMappingView>(createApprovalMapping));
            
            if (newApprovalMappings.Item1 != null && newApprovalMappings.Item1.Id != Guid.Empty)
            {
                response.Data = newApprovalMappings.Item1;
                response.Message = newApprovalMappings.Item2;

                _logger.LogInformation("Success add_approval_mapping with params: {param}", param);
                return Ok(response);
            }

            var msg = newApprovalMappings!.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed add_approval_mapping with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion

        #region EditData
        [HttpPut("edit_approval_mapping")]
        public async Task<ActionResult<ServiceResponse<ApprovalMappingView>>> EditApprovalMappingAsync([FromBody] UpdateApprovalMapping approvalUpdate)
        {
            var param = JsonConvert.SerializeObject(approvalUpdate);
            _logger.LogInformation("Calling edit_approval_mapping with params: {param}", param);

            ServiceResponse<ApprovalMappingView> response = new();
            var updateApproval = await _approvalService.UpdateApprovalMapping(_mapper.Map<ApprovalMappingView>(approvalUpdate));
            
            if (updateApproval.Item1 != null && updateApproval.Item1.Id != Guid.Empty)
            {
                response.Data = updateApproval.Item1;

                _logger.LogInformation("Success edit_approval_mapping with params: {param}", param);
                return Ok(response);
            }

            var msg = updateApproval.Item2;
            response.Message = msg;
            response.Success = false;

            _logger.LogError("Failed edit_approval_mapping with message: {msg} and params: {param}", msg, param);
            return BadRequest(response);
        }
        #endregion

        #region GeneratePromotionApproval
        [HttpPost("generate_approval_status")]
        public async Task<ActionResult<ServiceResponse<List<PromotionApproval>>>> GenerateApprovalStatusAsync([FromBody] GeneratePromoApproval request)
        {
            var param = JsonConvert.SerializeObject(request);
            _logger.LogInformation("Calling generate_approval_status with params: {param}", param);

            ServiceResponse<List<PromotionApproval>> response = new();
            List<PromotionApproval> newPromoApprovalStatuses = await _approvalService.GeneratePromoApproval(request);
            
            if (newPromoApprovalStatuses != null)
            {
                response.Data = newPromoApprovalStatuses;

                _logger.LogInformation("Success generate_approval_status with params: {param}", param);
                return Ok(response);
            }

            response.Message = "";
            response.Success = false;

            _logger.LogInformation("Failed generate_approval_status with params: {param}", param);
            return BadRequest(response);
        }
        #endregion

        #region ValidateAccess
        private (bool, string) ValidateAccess(HttpRequest request, string accessName)
        {
            try
            {
                string companyName = ""; string siteCode = "";
                companyName = request.Headers["X-Company"]!;
                siteCode = request.Headers["X-Site"]!;
                
                if (User.Claims.FirstOrDefault(q => q.Type.Trim().ToUpper() == "ACCESS")?.Value != null)
                {
                    var listAccessPayload = User.Claims.Where(q => q.Type.Trim().ToUpper() == "ACCESS").ToList();

                    foreach (var loopAccessPayload in listAccessPayload)
                    {
                        DataTokenAccessPayload? dataPayload = JsonConvert.DeserializeObject<DataTokenAccessPayload>(Convert.ToString(loopAccessPayload.Value));
                        dataPayload!.Permission!.ConvertAll(x => x.Trim().ToUpper());
                        dataPayload!.Sites!.ConvertAll(x => x.Trim().ToUpper());

                        if (dataPayload.Company == companyName && dataPayload!.Sites!.Contains(siteCode.Trim().ToUpper()) && dataPayload!.Permission!.Contains(accessName.Trim().ToUpper()))
                            return (true, "Access granted");
                    }
                }
                return (false, "You don't have permission");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        #endregion
    }
}
