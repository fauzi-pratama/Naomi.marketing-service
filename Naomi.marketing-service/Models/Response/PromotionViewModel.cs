using Naomi.marketing_service.Models.Message;
using static Naomi.marketing_service.Models.Request.PromotionDetailRequest;

namespace Naomi.marketing_service.Models.Response
{
    public class PromotionViewModel
    {
        public Guid Id { get; set; }
        public string? PromotionCode { get; set; }
        public string? PromotionName { get; set; }
        public string? PromotionStatus { get; set; }
        public Guid CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Depts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? PromotionChannels { get; set; }
        public string? Objective { get; set; }
        public string? PromoMaterials { get; set; }
        public string? Zones { get; set; }
        public string? Sites { get; set; }
        public int TargetSales { get; set; }
        public string? MaxPromoUsedType { get; set; }
        public int MaxPromoUsedQty { get; set; }
        public bool MultiplePromo { get; set; }
        public int MultiplePromoMaxQty { get; set; }
        public Guid PromotionClassId { get; set; }
        public string? PromotionClassName { get; set; }
        public Guid? PromotionTypeId { get; set; }
        public string? PromotionTypeName { get; set; }
        public string? RequirementExp { get; set; }
        public List<CreatePromoRuleReq>? PromoRuleRequirements { get; set; }
        public string? ResultExp { get; set; }
        public List<CreatePromoRuleResult>? PromoRuleResults { get; set; }
        public string? MopPromoSelectionId { get; set; }
        public string? MopPromoSelectionCode { get; set; }
        public string? MopPromoSelectionName { get; set; }
        public List<MopGroup>? PromoRuleMops { get; set; }
        public double MinTransaction { get; set; }

        public double MaxTransaction { get; set; }

        public string? Value { get; set; }

        public string? MaxDisc { get; set; }
        public string? NipEntertain { get; set; }
        public decimal? EntertainBudget { get; set; }

        public bool MemberOnly { get; set; }
        public bool NewMember { get; set; }
        public string? Members { get; set; }
        public bool ActiveFlag { get; set; }
        public bool IsApprove { get; set; } = false;
        public List<ApprovalStatus>? ApprovalStatuses { get; set; }

        #region additional for SuperApps
        public string? RedemptionCode { get; set; }
        public bool? DisplayOnApp { get; set; }
        public string? PromoDisplayed { get; set; }
        public string? ShortDesc { get; set; }
        public string? PromoTermsCondition { get; set; }
        public string? ImageLink { get; set; }
        #endregion
    }
    public class ApprovalStatus
    {
        public int ApprovalLevel { get; set; }
        public string? JobPosition { get; set; }
        public string? ApproverId { get; set; }
        public string? ApprvStatus { get; set; }
        public string? ApprvNotes { get; set; }
    }
}
