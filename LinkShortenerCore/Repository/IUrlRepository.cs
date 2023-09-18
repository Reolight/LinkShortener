using LinkShortenerCore.Model;

namespace LinkShortenerCore.Repository;

public interface IUrlRepository
{
    public Task<UrlDto?> GetUrlInfo(string shortUrl);
    public Task<IEnumerable<UrlDto>> GetAllUrlInfo();
    public ValueTask<bool> RemoveUrl(string shortUrl);
    public Task<UrlDto?> AddShortUrl(string shortUrl, string fullUrl);
    public Task<UrlDto?> UpdateUrl(string shortUrl, string fullUrl);
    public Task<string?> VisitUrl(string shortUrl);
}