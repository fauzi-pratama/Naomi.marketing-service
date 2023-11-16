namespace Naomi.marketing_service.Models.Response.Sap
{
    class MopPaymentApiResponse
    {
        public bool Succeeded { get; set; }
        public List<DataMopResponse>? Data { get; set; }
    }

    public class DataMopResponse
    {
        public string? Id { get; set; }
        public string? salesOrganizationCode { get; set; }
        public string? siteCode { get; set; }
        public string? mopCode { get; set; }
        public string? mopName { get; set; }
        public bool status { get; set; }
    }
}
