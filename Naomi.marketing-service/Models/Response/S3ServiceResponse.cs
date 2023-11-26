namespace Naomi.marketing_service.Models.Response
{
    public class S3ServiceResponse<T>
    {
        public string? response_code { get; set; }
        public string? response_message { get; set; }
        public string? file_path { get; set; } = "";
        public int http_response { get; set; }
        public int current_page { get; set; } = 1;
        public int total_page { get; set; } = 1;
        public T? data { get; set; }
    }
}
