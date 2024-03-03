

using System.ComponentModel.DataAnnotations;

namespace UrlShorten.Data.DTO
{
    public class UserUrlInputDto
    {
        [Required]
        public string InputUrl { get; set; } = string.Empty;
    }
}
