using System.Net.Http.Json;
using UrlShorten.Data.DTO;

namespace UrlShorten.Client.Service.ShortenUrlService
{
    public class UrlService : IUrl
    {
        private readonly HttpClient _httpClient;

        public UrlService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateShortenUrlForUserInputUrl(UserUrlInputDto userUrlInputDto)
        {
            var response = await _httpClient.PostAsJsonAsync("CreateShortenUrl", userUrlInputDto);
            var message = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return message;
            }
            throw new Exception($"Http status code: {response.StatusCode}, Message: {message}");
        }

        public async Task<string> GetNewShortenUrlForExisitingLink(UserUrlInputDto userUrlInputDto)
        {
            var response = await _httpClient.PostAsJsonAsync("GetNewUrl", userUrlInputDto);
            var message = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return message;
            }
            throw new Exception($"Http status code: {response.StatusCode}, Message: {message}");
        }
    }
}
