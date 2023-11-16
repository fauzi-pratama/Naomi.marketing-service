namespace Naomi.marketing_service.Models.Message
{
    public class PromotionClassUpdated
    {
        public Guid Id { get; set; }
        public string? PromotionClassKey { get; set; }
        public string? PromotionClassName { get; set; }
        public int LineNum { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool ActiveFlag { get; set; }
    }
}
