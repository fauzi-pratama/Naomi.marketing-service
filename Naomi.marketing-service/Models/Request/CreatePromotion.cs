using Newtonsoft.Json;

namespace Naomi.marketing_service.Models.Request
{
    public class CreatePromotion
    {
        [JsonProperty("promotion_class_id")]
        public Guid? PromotionClassId { get; set; }
        [JsonProperty("promotion_class_name")]
        public string? PromotionClassName { get; set; }

        [JsonProperty("promotion_type_id")]
        public Guid? PromotionTypeId { get; set; } = Guid.Empty;
        [JsonProperty("promotion_type_name")]
        public string? PromotionTypeName { get; set; }


        [JsonProperty("promotion_name")]
        public string? PromotionName { get; set; }

        [JsonProperty("company_id")]
        public Guid? CompanyId { get; set; }
        [JsonProperty("company_code")]
        public string? CompanyCode { get; set; }
        [JsonProperty("company_name")]
        public string? CompanyName { get; set; }

        [JsonProperty("depts")]
        public string? Depts { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("promotion_channel")]
        public string? PromotionChannel { get; set; }

        [JsonProperty("objective")]
        public string? Objective { get; set; }

        [JsonProperty("promotion_material")]
        public string? PromotionMaterial { get; set; }

        [JsonProperty("zones")]
        public string? Zones { get; set; }

        [JsonProperty("sites")]
        public string? Sites { get; set; }

        [JsonProperty("target_sales")]
        public int? TargetSales { get; set; }

        [JsonProperty("max_promo_used_type")]
        public string? MaxPromoUsedType { get; set; }

        [JsonProperty("max_promo_used_qty")]
        public int? MaxPromoUsedQty { get; set; }

        [JsonProperty("multiple_promo")]
        public bool? MultiplePromo { get; set; }

        public int? MultiplePromoMaxQty { get; set; }
        public int? MaxQtyPromo { get; set; }

        public string? RequirementExp { get; set; }

        public string? ResultExp { get; set; }
        public string? MopPromoSelectionId { get; set; }
        public string? MopPromoSelectionCode { get; set; }
        public string? MopPromoSelectionName { get; set; }

        public double? MinTransaction { get; set; }

        public double? MaxTransaction { get; set; }

        public string? Value { get; set; }

        public string? MaxDisc { get; set; }

        public bool? MemberOnly { get; set; }
        public bool? NewMember { get; set; } = false;
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
