namespace UrlShorten.Client.Service.ThirdPartyService
{
    public interface IShortenUrlThirdParty
    {
        Task<string> ShortenUrlAsyncUsingBitly(string longUrl);
    }
}
