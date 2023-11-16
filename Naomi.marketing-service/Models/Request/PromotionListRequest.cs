namespace Naomi.marketing_service.Models.Request
{
    public class PromotionListRequest
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PromotionsViewSearch? Search { get; set; }
        public string? OrderColumn { get; set; }
        public string? OrderMethod { get; set; }
    }
    public class PromotionsViewSearch
    {
        public string? PromotionSearch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? Status { get; set; }
    }
}
