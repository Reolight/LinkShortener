using LinkShortenerCore.Model;

namespace LinkShortenerCore.Services;

public interface IUrlShortenerService
{
    public Task<UrlDto?> CreateShortLink(string fullUrl);
}