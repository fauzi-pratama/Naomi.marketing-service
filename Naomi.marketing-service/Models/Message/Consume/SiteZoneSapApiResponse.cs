namespace Naomi.marketing_service.Models.Message.Consume
{
    public class SiteZoneSapApiResponse
    {
        public string? siteCode { get; set; }

        public string? siteDescription { get; set; }

        public string? pricingZone { get; set; }

        public bool activeFlag { get; set; }
    }
}
