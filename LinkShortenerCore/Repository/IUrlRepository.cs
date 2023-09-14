using LinkShortenerCore.Model;

namespace LinkShortenerCore.Repository;

public interface IUrlRepository
{
    public UrlDto GetUrlInfo(string shortUrl);
    public IEnumerable<UrlDto> GetAllUrlInfo();
    public bool RemoveUrl(string shortUrl);
    public UrlDto AddShortUrl(Uri fullUrl);
    public UrlDto UpdateUrl(string shortUrl, Uri fullUrl);
}