namespace Naomi.marketing_service.Models.Response
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public int Pages { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public int Pages { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }
}
