namespace Naomi.marketing_service.Models.Request
{
    public class UpdatePromotion
    {
        public Guid Id { get; set; }


        public DateTime EndDate { get; set; }

        public string? Zones { get; set; }
        public string? Sites { get; set; }

        public bool MemberOnly { get; set; }
        public bool NewMember { get; set; }
        public string? Members { get; set; }

        public string? Username { get; set; }

        public string? RuleReqs { get; set; }
        public string? RuleRess { get; set; }

        #region Additional for SuperApps
        public string? PromoDisplayed { get; set; }
        public string? ShortDesc { get; set; }
        public string? PromoTermsCondition { get; set; }
        public IFormFile? PromoImage { get; set; }
        #endregion

    }
}
