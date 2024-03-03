

using UrlShorten.Data;
using UrlShorten.Data.DTO;
using UrlShorten.Data.Entities;

namespace UrlShorten.Repository.Repositories.UrlRepository
{
    public class ShortenUrlRepository : IUrl
    {
        private readonly ApplicationDbContext _dbContext;

        public ShortenUrlRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public OriginalUrlDto GetOriginalUrl(ShortenUrlDto shortenUrlDto)
        {
            var urlMapper = _dbContext.UrlMappers.FirstOrDefault(u => u.ShortenUrl == shortenUrlDto.ShortenUrl);
            if(urlMapper == null)
            {
                return null;
            }
            return new OriginalUrlDto() { OriginalUrl = urlMapper.InputUrl };
        }

        public OriginalUrlDto GetOriginalUrlByKey(string key)
        {
            var urlMapper = _dbContext.UrlMappers.FirstOrDefault(u => u.ShortenUrl == key);
            if (urlMapper == null)
            {
                return null;
            }
            return new OriginalUrlDto() { OriginalUrl = urlMapper.InputUrl };
        }

        public UrlMapper CreateShortenUrlForUserInputUrl(UserUrlInputDto userUrlInput)
        {
            var urlMapperTestFind = _dbContext.UrlMappers.FirstOrDefault(p => p.InputUrl == userUrlInput.InputUrl);
            if (urlMapperTestFind != null)
            {
                return urlMapperTestFind;
            }
            string shortenUrl = Guid.NewGuid().ToString().Substring(0, 8);

            UrlMapper urlMapper = new UrlMapper()
            {
                InputUrl = userUrlInput.InputUrl,
                ShortenUrl = shortenUrl
            };
            _dbContext.UrlMappers.Add(urlMapper);
            _dbContext.SaveChanges();
            return urlMapper;
        }
        public UrlMapper UpdateShortenUrl(UserUrlInputDto userUrlInput)
        {
            var UrlRow = _dbContext.UrlMappers.FirstOrDefault(p => p.InputUrl == userUrlInput.InputUrl);
            if (UrlRow == null)
            {
                return CreateShortenUrlForUserInputUrl(userUrlInput);
            }
            _dbContext.UrlMappers.Remove(UrlRow);
            _dbContext.SaveChanges();
            return CreateShortenUrlForUserInputUrl(userUrlInput);
        }
        public UrlMapper DeleteShortenUrl(UserUrlInputDto userUrlInput)
        {
            var UrlRow = _dbContext.UrlMappers.FirstOrDefault(p => p.InputUrl == userUrlInput.InputUrl);
            if (UrlRow == null)
            {
                return null;
            }
            _dbContext.UrlMappers.Remove(UrlRow);
            _dbContext.SaveChanges();
            return UrlRow;
        }
        public UrlMapper DeleteShortenUrl(ShortenUrlDto shortenUrlDto)
        {
            var UrlRow = _dbContext.UrlMappers.FirstOrDefault(p => p.ShortenUrl == shortenUrlDto.ShortenUrl);
            if (UrlRow == null)
            {
                return null;
            }
            _dbContext.UrlMappers.Remove(UrlRow);
            _dbContext.SaveChanges();
            return UrlRow;
        }

        public IEnumerable<UrlMapper> GetAllMappingUrls()
        {
            return _dbContext.UrlMappers.ToList();
        }
    }

}
