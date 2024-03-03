using UrlShorten.Data.DTO;

namespace UrlShorten.Client.Service.ShortenUrlService
{
    public interface IUrl
    {
        Task<string> CreateShortenUrlForUserInputUrl(UserUrlInputDto userUrlInput);
        Task<string> GetNewShortenUrlForExisitingLink(UserUrlInputDto userUrlInputDto);
    }
}
