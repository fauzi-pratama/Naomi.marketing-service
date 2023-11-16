namespace Naomi.marketing_service.Models.Request
{
    public class PromotionStatusRequest
    {
        public class CreatePromotionStatus
        {
            public string? PromotionStatusKey { get; set; }

            public string? PromotionStatusName { get; set; }

        }
    }
}
