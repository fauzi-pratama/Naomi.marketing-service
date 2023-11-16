namespace Naomi.marketing_service.Models.Request
{
    public class CreatePromotion
    {
        public Guid PromotionClassId { get; set; }
        public string? PromotionClassName { get; set; }

        public Guid? PromotionTypeId { get; set; } = Guid.Empty;
        public string? PromotionTypeName { get; set; }


        public string? PromotionName { get; set; }

        public Guid CompanyId { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }

        public string? Depts { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? PromotionChannel { get; set; }

        public string? Objective { get; set; }

        public string? PromotionMaterial { get; set; }

        public string? Zones { get; set; }

        public string? Sites { get; set; }

        public int TargetSales { get; set; }

        public string? MaxPromoUsedType { get; set; }

        public int MaxPromoUsedQty { get; set; }

        public bool MultiplePromo { get; set; }

        public int MultiplePromoMaxQty { get; set; }

        public string? RequirementExp { get; set; }

        public string? ResultExp { get; set; }
        public string? MopPromoSelectionId { get; set; }
        public string? MopPromoSelectionCode { get; set; }
        public string? MopPromoSelectionName { get; set; }

        public double MinTransaction { get; set; }

        public double MaxTransaction { get; set; }

        public string? Value { get; set; }

        public string? MaxDisc { get; set; }

        public bool MemberOnly { get; set; }
        public bool NewMember { get; set; } = false;
        public string? Members { get; set; }

        public string? Username { get; set; }
        public string? NipEntertain { get; set; }

        #region Additional for SuperApps
        public string? RedemptionCode { get; set; }
        public bool? DisplayOnApp { get; set; } = true;
        public string? PromoDisplayed { get; set; }
        public string? ShortDesc { get; set; }
        public string? PromoTermsCondition { get; set; }
        public IFormFile? PromoImage { get; set; }
        #endregion

        public string? RuleReqs { get; set; }
        public string? RuleRess { get; set; }
        public string? RuleMops { get; set; }
    }
}
