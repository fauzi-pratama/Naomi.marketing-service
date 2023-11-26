using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Controllers.MessageHandler;
using Naomi.marketing_service.Models.Response;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        //private readonly ILogger<SapMessageController> _logger;

        //public ApprovalController(ILogger<SapMessageController> logger)
        //{
        //    _logger = logger;
        //}

        //#region GET
        //[HttpGet("getapprovalmapping")]
        ////public async Task<ActionResult<ServiceResponse<Tuple<List<ApprovalMappingView>, string>>>> GetApprovalMapping(Guid? companyId, string? companyCode)
        ////{
        ////    ServiceResponse<List<ApprovalMappingView>> response = new();

        ////    return Ok(response);
        ////}
        //#endregion

        //#region POST
        //#endregion

        //#region PUT
        //#endregion
    }
}
