namespace Naomi.marketing_service.Models.Response
{
    public class DataTokenAccessPayload
    {
        public string? Company { get; set; }
        public string? BuSap { get; set; }
        public List<string>? Sites { get; set; }
        public List<string>? Roles { get; set; }
        public List<string>? Permission { get; set; }
    }
}
