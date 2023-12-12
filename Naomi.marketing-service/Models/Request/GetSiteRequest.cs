namespace Naomi.marketing_service.Models.Request
{
    public class GetSiteRequest
    {
        public string? CompanyCode { get; set; }
        public List<string>? ZoneList { get; set; } 
        public string? SearchName { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
