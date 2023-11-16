namespace Naomi.marketing_service.Models.Response
{
    public class PromotionHeaderResponse
    {
        public class PromotionListView
        {
            public Guid Id { get; set; }
            public Guid? PromotionTypeId { get; set; }
            public string? PromotionType { get; set; }
            public string? PromotionName { get; set; }
            public DateTime StartDate { set; get; }
            public DateTime EndDate { set; get; }
            public string? Depts { get; set; }
            public Guid PromotionStatusId { get; set; }
            public string? Status { get; set; }
            public DateTime CreatedDate { get; set; }
        }
        public class PromotionListResultView
        {
            public Guid? PromoId { get; set; }
            public Guid? PromoTypeId { get; set; }
            public string? promoTypeName { get; set; }
            public string? PromoName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string? Depts { get; set; }

            public Guid? PromoStatusId { get; set; }
            public string? PromoStatusName { get; set; }
            public DateTime? CreatedDate { get; set; }
            public Guid? PromoApprovalId { get; set; }
        }
    }
}
