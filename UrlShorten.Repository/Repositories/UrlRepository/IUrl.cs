

using UrlShorten.Data.DTO;
using UrlShorten.Data.Entities;

namespace UrlShorten.Repository.Repositories.UrlRepository
{
    public interface IUrl
    {
        UrlMapper DeleteShortenUrl(UserUrlInputDto userUrlInput);
        UrlMapper DeleteShortenUrl(ShortenUrlDto shortenUrlDto);
        OriginalUrlDto GetOriginalUrl(ShortenUrlDto shortenUrlDto);
        OriginalUrlDto GetOriginalUrlByKey(string key);
        UrlMapper CreateShortenUrlForUserInputUrl(UserUrlInputDto userUrlInput);
        UrlMapper UpdateShortenUrl(UserUrlInputDto userUrlInput);
        IEnumerable<UrlMapper> GetAllMappingUrls();
    }
}
