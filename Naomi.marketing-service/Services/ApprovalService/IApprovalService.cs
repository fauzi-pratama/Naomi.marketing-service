using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;

namespace Naomi.marketing_service.Services.ApprovalService
{
    public interface IApprovalService
    {
        Task<Tuple<List<ApprovalMappingView>, string>> GetApprovalMapping(Guid? companyId, string? companyCode);
        Task<Tuple<ApprovalMappingView?, string>> GetApprovalMappingById(Guid approvalMappingId);
        Task<Tuple<ApprovalMappingView, string>> InsertApprovalMapping(ApprovalMappingView approvalMapping);
        Task<Tuple<ApprovalMappingView, string>> UpdateApprovalMapping(ApprovalMappingView approvalMapping);
        Task<List<PromotionApproval>> GeneratePromoApproval(GeneratePromoApproval request);
    }
}
