namespace Naomi.marketing_service.Models.Request
{
    public class ChannelMaterialRequest
    {
        public class PromoChannelRequest
        {
            public string? ChannelName { get; set; }
            public string? Username { get; set; }
        }

        public class PromoMaterialRequest
        {
            public string? MaterialName { get; set; }
            public string? Username { get; set; }
        }
    }
}
