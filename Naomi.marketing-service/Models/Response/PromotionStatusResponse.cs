namespace Naomi.marketing_service.Models.Response
{
    public class PromotionStatusResponse
    {
        public class RespondPromotionStatusCount
        {
            public Guid StatusId { get; set; }
            public string? StatusKey { get; set; }
            public string? StatusName { get; set; }
            public int StatusCount { get; set; }
        }
    }
}
