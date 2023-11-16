namespace Naomi.marketing_service.Models.Message.Consume
{
    public class CompanySapApiResponse
    {
        public string? id { get; set; }
        public string? companyCode { get; set; }
        public string? companyDescription { get; set; }
        public string? companyAddress { get; set; }
        public DateTime? createdDate { get; set; }
        public string? createdBy { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string? lastUpdatedBy { get; set; }
        public bool? activeFlag { get; set; }
    }
}
