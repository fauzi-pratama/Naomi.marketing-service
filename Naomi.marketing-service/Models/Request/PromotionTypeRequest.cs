namespace Naomi.marketing_service.Models.Request
{
    public class PromotionTypeRequest
    {
        public class CreatePromotionType
        {
            public Guid PromotionClassId { get; set; }

            public string? PromotionTypeKey { get; set; }

            public string? PromotionTypeName { get; set; }
        }
        public class UpdatePromotionType
        {
            public Guid Id { get; set; }

            public string? PromotionClassId { get; set; }

            public string? PromotionTypeKey { get; set; }

            public string? PromotionTypeName { get; set; }
        }
    }
}
