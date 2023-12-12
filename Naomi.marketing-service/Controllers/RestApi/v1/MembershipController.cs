using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Message.Pub;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.MembershipService;
using Naomi.marketing_service.Services.PromoClassService;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        public readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService, ILogger<MembershipController> logger)
        {
            _membershipService = membershipService;
        }

        #region GetData
        [HttpGet("get_member")]
        public async Task<ActionResult<ServiceResponse<List<Member>>>> GetMember(string? searchName, int pageNo = 1, int pageSize = 10)
        {
            ServiceResponse<List<Member>> response = new();
            var viewMember = await _membershipService.GetMember(searchName!, pageNo, pageSize);

            if (viewMember != null && viewMember.Item1.Count > 0)
            {
                response.Data = viewMember.Item1;
                response.Pages = pageNo;
                response.TotalPages = viewMember.Item2;
                return Ok(response);
            }
            else
            {
                response.Message = "Data not found";
                response.Success = false;
                return NotFound(response);
            }
        }
        #endregion
    }
}
