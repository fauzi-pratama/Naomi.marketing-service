namespace Naomi.marketing_service.Models.Request.Sap
{
    public class IntegrationMessageRequest
    {
        public string? DocumentNumber { get; set; }

        public string? SyncName { get; set; }

        public object? SyncData { get; set; }
    }
}
