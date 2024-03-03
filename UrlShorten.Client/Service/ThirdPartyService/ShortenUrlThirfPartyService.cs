
using System.Net.Http.Json;
using System.Text.Json;
using UrlShorten.Client.Pages;

namespace UrlShorten.Client.Service.ThirdPartyService
{
    public class ShortenUrlThirfPartyService : IShortenUrlThirdParty
    {
        private readonly string BitlyAccessToken = "13a09fc6981ef3fb31e9886aefd6181b58ddf95f";
        private readonly string BitlyApiUrl = "https://api-ssl.bitly.com/v4/shorten";
        private readonly HttpClient _httpClient;

        public ShortenUrlThirfPartyService(HttpClient _httpClient)
        {
            this._httpClient = _httpClient;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {BitlyAccessToken}");
        }

        public async Task<string> ShortenUrlAsyncUsingBitly(string longUrl)
        {
            var requestData = new { long_url = longUrl };
            var response = await _httpClient.PostAsJsonAsync(BitlyApiUrl, requestData);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                string shortLink = GetShortUrlFromResponse(responseContent);
                return shortLink == null ? longUrl : shortLink;
            }
            return longUrl;
        }

        private string GetShortUrlFromResponse(string responseContent)
        {
            // {"created_at":"2024-03-03T08:09:40+0000","id":"bit.ly/48FgHHx"
            // ,"link":"https://bit.ly/48FgHHx"
            // ,"custom_bitlinks":[]
            // ,"long_url":"https://www.google.com/search?q=u&oq=u&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIGCAEQRRg7MgYIAhBFGDsyBggDEEUYOzIGCAQQRRhBMgYIBRBFGDwyBggGEEUYPDIGCAcQLhhA0gEHNjUzajBqMagCALACAA&sourceid=chrome&ie=UTF-8"
            // ,"archived":false,"tags":[],"deeplinks":[],"references":{"group":"https://api-ssl.bitly.com/v4/groups/Bo3379YCRWn"}}

            string shortUrlLink = responseContent.Split(",")[2].Trim('"').Substring(7);
            return shortUrlLink;

        }
    }
}
