namespace Naomi.marketing_service.Models.Message
{
    public class PromoUpdated
    {
        public string? PromoRuleId { get; set; }
        public string? PromoRuleCode { get; set; }
        public string? PromoRuleRedemptionCode { get; set; }
        public string? PromoRuleName { get; set; }
        public DateTime PromoRuleStartDate { get; set; }
        public DateTime PromoRuleEndDate { get; set; }
        public string? PromoWorkflowId { get; set; }
        public string? PromoWorkflowCode { get; set; }
        public string? PromoWorkflowName { get; set; }
        public string? BrandId { get; set; }
        public string? BrandName { get; set; }
        public List<Dept>? Depts { get; set; }
        public int PromoRuleClass { get; set; }
        public int PromoRuleLevel { get; set; }
        public string? PromoRuleActionType { get; set; }
        public string? PromoRuleItemType { get; set; }
        public string? PromoRuleActionValue { get; set; }
        public string? PromoRuleActionValueMax { get; set; }
        public string? Username { get; set; }
        public List<Zone>? Zones { get; set; }
        public List<Site>? Sites { get; set; }
        public bool MemberOnly { get; set; }
        public bool NewMember { get; set; }
        public List<Member>? Members { get; set; }
        public bool MultipleApp { get; set; }
        public int MultipleAppQty { get; set; }
        public int PromoQuota { get; set; }
        public int PromoBalance { get; set; }
        public bool IsActive { get; set; }
        public string? PromoExpMinTransAmount { get; set; }
        public string? PromoExpMinTransQty { get; set; }
        public string? MopPromoSelectionId { get; set; }
        public string? MopPromoSelectionCode { get; set; }
        public string? MopPromoSelectionName { get; set; }
        public string? NipEntertain { get; set; }
        public bool EntertainStatus { get; set; } = false;
        public string? PromoRuleTermsCondition { get; set; }
        public string? PromoRuleShortDesc { get; set; }
        public bool? PromoRuleDisplayOnApp { get; set; }
        public List<PromoDisplayed>? PromoDisplayed { get; set; }
        public string? PromoImageLink { get; set; }
        public Requirement? Requirement { get; set; }
        public Result? Result { get; set; }
        public List<MopGroup>? MopGroups { get; set; }
    }
}
