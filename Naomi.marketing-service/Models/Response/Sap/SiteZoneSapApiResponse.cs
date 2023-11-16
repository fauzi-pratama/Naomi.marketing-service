namespace Naomi.marketing_service.Models.Response.Sap
{
    public class SiteZoneSapApiResponse
    {
        public string? siteCode { get; set; }

        public string? siteDescription { get; set; }

        public string? pricingZone { get; set; }

        public bool activeFlag { get; set; }
    }
}
