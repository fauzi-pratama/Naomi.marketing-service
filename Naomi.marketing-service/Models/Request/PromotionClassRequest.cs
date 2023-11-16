namespace Naomi.marketing_service.Models.Request
{
    public class PromotionClassRequest
    {
        public class CreatePromotionClass
        {
            public string? PromotionClassKey { get; set; }

            public string? PromotionClassName { get; set; }

        }
        public class UpdatePromotionClass
        {
            public Guid Id { get; set; }

            public string? PromotionClassKey { get; set; }

            public string? PromotionClassName { get; set; }
        }
    }
}
