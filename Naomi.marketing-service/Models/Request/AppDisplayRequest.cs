namespace Naomi.marketing_service.Models.Request
{
    public class AppDisplayRequest
    {
        public string? AppName { get; set; }
        public string? BucketName { get; set; }
        public string? Region { get; set; }
        public string? SecretKey { get; set; }
        public string? AccessKey { get; set; }
        public string? BaseDirectory { get; set; }
        public string? Username { get; set; }
    }
    public class AppDisplayEditRequest
    {
        public string? AppCode { get; set; }
        public string? BucketName { get; set; }
        public string? Region { get; set; }
        public string? SecretKey { get; set; }
        public string? AccessKey { get; set; }
        public string? BaseDirectory { get; set; }
        public string? Username { get; set; }
    }
}
