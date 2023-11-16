using Naomi.marketing_service.Models.Request.Sap;

namespace Naomi.marketing_service.Models.Message.Consume
{
    public class SapIntegrationMessage : IntegrationMessageRequest
    {
        public SapIntegrationMessage()
        { }
        public SapIntegrationMessage(string? DocumentNumber, object SyncData, string SyncName, string Sender)
        {
            this.DocumentNumber = DocumentNumber;
            this.SyncData = SyncData;
            this.SyncName = SyncName;
        }
    }
}
