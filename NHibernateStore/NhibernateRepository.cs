using LinkShortenerCore.Model;
using LinkShortenerCore.Repository;
using NhibernateStore.Entities;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;

namespace NhibernateStore;

public class NhibernateRepository : IUrlRepository
{
    private readonly ISession _session;
    private readonly ILogger<NhibernateRepository> _logger;

    public NhibernateRepository(ISession session, ILogger<NhibernateRepository> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<UrlDto?> GetUrlInfo(string shortUrl)
        => (await _session.Query<Url>()
                .FirstOrDefaultAsync(url => url.ShortUrl == shortUrl))
            .AdaptToDto();

    public async Task<IEnumerable<UrlDto>> GetAllUrlInfo() =>
        await _session.Query<Url>()
            .Select(url => url.AdaptToDto())
            .ToListAsync();

    public async ValueTask<bool> RemoveUrl(string shortUrl)
    {
        if (await _session.Query<Url>()
                .FirstOrDefaultAsync(url => url.ShortUrl == shortUrl) is not {} urlInstance)
            return false;
        using ITransaction transaction = _session.BeginTransaction();
        await _session.DeleteAsync(urlInstance);
        await transaction.CommitAsync();
        _logger.LogWarning("Short url [{ShortUrl}] for url [{FullUrl}] was removed",
            urlInstance.ShortUrl, urlInstance.FullUrl);
        return true;
    }

    public async Task<UrlDto?> AddShortUrl(string shortUrl, string fullUrl)
    {
        if (await _session.Query<Url>()
                .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl) != null)
            return null;
        
        var url = new Url
        {
            ShortUrl = shortUrl, 
            FullUrl = fullUrl, 
            VisitedTimes = 0,
            CreationTime = DateTime.UtcNow, 
        };

        using ITransaction transaction = _session.BeginTransaction();
        var addedInstance = await _session.SaveAsync(url) as Url;
        await transaction.CommitAsync();
        _logger.LogInformation("Created new short url [{ShortUrl}] for [{FullUrl}]",
            addedInstance?.ShortUrl, addedInstance?.FullUrl);
        return addedInstance?.AdaptToDto();
    }

    public async Task<UrlDto?> UpdateUrl(string shortUrlToUpdate, string newFullUrl)
    {
        if (await _session.GetAsync<Url>(shortUrlToUpdate) is not {} updatedInstance)
            return null;
        
        using ITransaction transaction = _session.BeginTransaction();
        new UrlDto
            {
                ShortUrl = shortUrlToUpdate, CreationTime = DateTime.UtcNow, FullUrl = newFullUrl, VisitedTimes = 0
            }
            .AdaptToEntity(updatedInstance);
        
        await _session.UpdateAsync(updatedInstance); 
        await transaction.CommitAsync();
        _logger.LogInformation("Url was updated: [{ShortUrl}] now references to [{FullUrl}]",
            shortUrlToUpdate, newFullUrl);
        return updatedInstance.AdaptToDto();
    }

    public async Task<string?> VisitUrl(string shortUrl)
    {
        if (await _session.GetAsync<Url>(shortUrl) is not { } visitedUrl)
            return null;
        using ITransaction transaction = _session.BeginTransaction();
        visitedUrl.VisitedTimes++;
        await _session.UpdateAsync(visitedUrl);
        await transaction.CommitAsync();
        return visitedUrl.FullUrl;
    }
}