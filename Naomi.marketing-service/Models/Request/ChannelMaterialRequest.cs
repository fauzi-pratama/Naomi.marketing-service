namespace Naomi.marketing_service.Models.Request
{
    public class ChannelMaterialRequest
    {
        public class PromoChannel
        {
            public string? ChannelName { get; set; }
        }

        public class PromoMaterial
        {
            public string? MaterialName { get; set; }
        }
    }
}
