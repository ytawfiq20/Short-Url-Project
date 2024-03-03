namespace UrlShorten.Api.Models
{
    public class ApiResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string LongUrl { get; set; } = string.Empty;
        public T? Value { get; set; }
    }
}
