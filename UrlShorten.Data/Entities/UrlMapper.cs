

namespace UrlShorten.Data.Entities
{
    public class UrlMapper
    {
        public Guid Id { get; set; }
        public string InputUrl { get; set; } = string.Empty;
        public string ShortenUrl { get; set; } = string.Empty;
    }
}
