using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Message.Pub;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.MembershipService;
using Naomi.marketing_service.Services.PromoClassService;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Authorize]
    [Route("/v1/")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        public readonly IMembershipService _membershipService;
        public readonly ILogger<MembershipController> _logger;

        public MembershipController(IMembershipService membershipService, ILogger<MembershipController> logger)
        {
            _membershipService = membershipService;
            _logger = logger;
        }

        #region GetData
        [HttpGet("get_member")]
        public async Task<ActionResult<ServiceResponse<List<Member>>>> GetMemberAsync(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            _logger.LogInformation("Calling get_member with search name: {searchName}", searchName);

            ServiceResponse<List<Member>> response = new();
            var viewMember = await _membershipService.GetMember(searchName!, pageNo, pageSize);

            if (viewMember != null && viewMember.Item1.Count > 0)
            {
                response.Data = viewMember.Item1;
                response.Pages = pageNo;
                response.TotalPages = viewMember.Item2;

                _logger.LogInformation("Success get_member with search name: {searchName}", searchName);
                return Ok(response);
            }

            response.Message = "Data not found";
            response.Success = false;

            _logger.LogInformation("Failed get_member with search name: {searchName}", searchName);
            return NotFound(response);
        }
        #endregion
    }
}
