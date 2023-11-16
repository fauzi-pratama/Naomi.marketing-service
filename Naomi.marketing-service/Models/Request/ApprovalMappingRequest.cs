namespace Naomi.marketing_service.Models.Request
{
    public class ApprovalMappingRequest
    {

        public int ApprovalLevel { get; set; }
        public string? ApproverId { get; set; }
        public string? JobPosition { get; set; }
    }
    public class CreateApprovalMapping
    {
        public Guid CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public List<ApprovalMappingRequest>? ApprovalMappingList { get; set; }
    }

    public class UpdateApprovalMapping
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public List<ApprovalMappingRequest>? ApprovalMappingList { get; set; }
        public bool ActiveFlag { get; set; }
    }

    public class GeneratePromoApproval
    {
        public Guid PromotionHeaderId { get; set; } = Guid.Empty;
        public Guid CompanyId { get; set; } = Guid.Empty;
        public string? CompanyCode { get; set; }
    }

    public class ApproveRejectPromotion
    {
        public Guid PromotionHeaderId { get; set; }
        public string? ApproverId { get; set; }
        public bool Approve { get; set; } = false;
        public string? ApprovalNotes { get; set; }
    }
}
