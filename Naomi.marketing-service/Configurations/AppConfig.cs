namespace Naomi.marketing_service.Configurations
{
    public class AppConfig
    {
        public string? PostgreSqlConnectionString { get; set; }
        public string? KafkaConnectionString { get; set; }

        /*public key token v2*/
        public string? PublicKey { get; set; } = @"-----BEGIN PUBLIC KEY----- MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAors1PyGiIqt4LhVIvWZJ qq0q147ql0ZATV3DWelRLMGoe/K/fitS7QdfC+JSDc2/r7i1mf2P8QXHVQNTDny8 bAU44OYz9P9W0b8BZgDwI0ccWW6CTEKLTI5UYmeaL3+mJK3FZ8tfsmfrTDfaDHof UkhqHJzmcncJE8DpySpIrYy3g+VAVRNcnRWF7mQJ5rt9YiyeE2qF6rvGEq08Bcar pjAdXhK06jIdyM6IbDPTFmNYaFN7A4TbPYwED+QntHn2bPZVplWiDstujhLubUuX p1KrjMbVHZ5S7KfRIQpyQvEKaVexxUwJ7rst0A9k8bktLnyV2HC2oE8msZoPnyky 2wIDAQAB -----END PUBLIC KEY-----";

        public string UrlInitialCompanyWorkPlaceService { get; private set; } = "https://api-aurora-dev.klgsys.com/sapadapter/GetListCompany";
        public string UrlInitialMopPaymentService { get; private set; } = "https://api-aurora-dev.klgsys.com/payment/Mop/MOPSiteListing";
        public string UrlInitialSiteZoneSapAdapter { get; private set; } = "https://api-aurora-dev.klgsys.com/sapadapter/GetSiteByCompanyCode";
    }
}
