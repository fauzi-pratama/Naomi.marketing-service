using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Naomi.marketing_service.Models.Entities
{
    [Table("promotion_header")]
    public class PromotionHeader
    {
        [Key]
        [Column("id", Order = 0)]
        public Guid Id { get; set; }


        [Required]
        [ForeignKey("promotion_class")]
        [Column("promotion_class_id", Order = 1)]
        public Guid PromotionClassId { get; set; }

        [ForeignKey("promotion_type")]
        [Column("promotion_type_id", Order = 2)]
        public Guid? PromotionTypeId { get; set; }

        [ForeignKey("promotion_status")]
        [Column("promotion_status_id", Order = 3)]
        public Guid PromotionStatusId { get; set; }

        [ForeignKey("company_view_model")]
        [Column("company_id", Order = 4)]
        public Guid CompanyId { get; set; }
        
        [Required]
        [Column("company_code", Order = 5), MaxLength(50)]
        public string? CompanyCode { get; set; }

        [Column("company_name", Order = 6), MaxLength(200)]
        public string? CompanyName { get; set; }


        [Column("promotion_code", Order = 7), MaxLength(50)]
        public string? PromotionCode { get; set; }

        [Column("redemption_code", Order = 8), MaxLength(50)]
        public string? RedemptionCode { get; set; }

        [Required]
        [Column("promotion_name", Order = 9), MaxLength(200)]
        public string? PromotionName { get; set; }

        /*ini sebenernya udah ga dipake*/
        [ForeignKey("brand_view_model")]
        [Column("brand_id")]
        public Guid? BrandId { get; set; }
        [Column("brand"), MaxLength(50)]
        public string? Brand { get; set; }
        /*ini sebenernya udah ga dipake -- sampe sini*/

        [Column("depts", Order = 10), DataType("text")]
        public string? Depts { get; set; }

        [Column("start_date", Order = 11)]
        public DateTime StartDate { get; set; }

        [Column("end_date", Order = 12)]
        public DateTime EndDate { get; set; }

        [Column("promotion_channel", Order = 13), DataType("text")]
        public string? PromotionChannel { get; set; }

        [Column("objective", Order = 14)]
        public string? Objective { get; set; }

        [Column("promotion_material", Order = 15), DataType("text")]
        public string? PromotionMaterial { get; set; }

        [Column("zones", Order = 16), DataType("text")]
        public string? Zones { get; set; }

        [Column("sites", Order = 17), DataType("text")]
        public string? Sites { get; set; }

        [Column("target_sales", Order = 18)]
        public int TargetSales { get; set; }

        [Column("max_promo_used_type", Order = 19), MaxLength(50)]
        public string? MaxPromoUsedType { get; set; }

        [Column("max_promo_used_qty", Order = 20)]
        public int MaxPromoUsedQty { get; set; }

        [Column("multiple_promo", Order = 21)]
        public bool MultiplePromo { get; set; }

        [Column("multiple_promo_max_qty", Order = 22)]
        public int MultiplePromoMaxQty { get; set; }

        [Column("requirement_exp", Order = 23), MaxLength(10)]
        public string? RequirementExp { get; set; }

        [Column("result_exp", Order = 24), MaxLength(10)]
        public string? ResultExp { get; set; }

        [Column("min_transaction", Order = 25)]
        public double MinTransaction { get; set; }

        [Column("max_transaction", Order = 26)]
        public double MaxTransaction { get; set; }

        [Column("value", Order = 27)]
        public string? Value { get; set; }

        [Column("max_disc", Order = 28)]
        public string? MaxDisc { get; set; }

        [Column("member_only", Order = 29)]
        public bool MemberOnly { get; set; }

        [Column("new_member", Order = 30)]
        public bool NewMember { get; set; } = false;

        [Column("members", Order = 31)]
        public string? Members { get; set; }

        [Column("mop_promo_selection_id", Order = 32)]
        public string? MopPromoSelectionId { get; set; }

        [Column("mop_promo_selection_code", Order = 33)]
        public string? MopPromoSelectionCode { get; set; }

        [Column("mop_promo_selection_name", Order = 34)]
        public string? MopPromoSelectionName { get; set; }

        [Column("nip_entertain", Order = 35), MaxLength(10)]
        public string? NipEntertain { get; set; }

        [Column("entertain_budget", Order = 36)]
        public decimal? EntertainBudget { get; set; }

        [Column("promo_terms_condition", Order = 37), DataType("text")]
        public string? PromoTermsCondition { get; set; }

        [Column("short_desc", Order = 38), DataType("text")]
        public string? ShortDesc { get; set; }

        [Column("display_on_app", Order = 39)]
        public bool? DisplayOnApp { get; set; } = true;

        [Column("promo_displayed", Order = 40), DataType("text")]
        public string? PromoDisplayed { get; set; }

        [NotMapped]
        public IFormFile? PromoImage { get; set; }


        [NotMapped]
        public string? Username { get; set; }

        public List<PromotionRuleRequirement>? PromoRuleRequirements { get; set; }

        public List<PromotionRuleResult>? PromoRuleResults { get; set; }

        public List<PromotionRuleMopGroup>? PromoRuleMops { get; set; }
        public List<PromotionAppImage>? PromoAppImages { get; set; }


        [Column("created_date", Order = 41)]
        public DateTime CreatedDate { get; set; }

        [Column("created_by", Order = 42), MaxLength(50)]
        public string? CreatedBy { get; set; }

        [Column("updated_date", Order = 43)]
        public DateTime UpdatedDate { get; set; }

        [Column("updated_by", Order = 44), MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [Required]
        [Column("active_flag", Order = 45)]
        public bool ActiveFlag { get; set; }
    }
}
