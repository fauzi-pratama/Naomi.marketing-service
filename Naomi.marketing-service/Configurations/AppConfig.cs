namespace Naomi.marketing_service.Configurations
{
    public class AppConfig
    {
        public string? PostgreSqlConnectionString { get; set; }
        public string? KafkaConnectionString { get; set; }
        public string? PublicKey { get; set; }

        public string UrlInitialCompanyWorkPlaceService { get; private set; } = "https://api-aurora-dev.klgsys.com/sapadapter/GetListCompany";
        public string UrlInitialMopPaymentService { get; private set; } = "https://api-aurora-dev.klgsys.com/payment/Mop/MOPSiteListing";
        public string UrlInitialSiteZoneSapAdapter { get; private set; } = "https://api-aurora-dev.klgsys.com/sapadapter/GetSiteByCompanyCode";
    }
}
