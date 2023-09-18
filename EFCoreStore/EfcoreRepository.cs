using EFCoreStore.Entities;
using LinkShortenerCore.Model;
using LinkShortenerCore.Repository;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace EFCoreStore;

public class EfCoreRepository : IUrlRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapster;

    internal EfCoreRepository(AppDbContext context, IMapper mapster)
    {
        _context = context;
        _mapster = mapster;
    }

    public async Task<UrlDto?> GetUrlInfo(string shortUrl) => 
        (await _context.Urls.FindAsync(shortUrl))?.Adapt<UrlDto>();

    public async Task<IEnumerable<UrlDto>> GetAllUrlInfo() => 
        await _context.Urls
            .ProjectToType<UrlDto>()
            .AsNoTracking()
            .ToListAsync();

    public async ValueTask<bool> RemoveUrl(string shortUrl)
    {
        if (await _context.Urls.FindAsync() is not { } urlToRemove)
            return false;
        _context.Urls.Remove(urlToRemove);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UrlDto?> AddShortUrl(string shortUrl, string fullUrl)
    {
        if (await _context.Urls.FindAsync(shortUrl) != null)
            return null;
        
        var url = new Url
        {
            ShortUrl = shortUrl, 
            FullUrl = fullUrl, 
            VisitedTimes = 0,
            CreationTime = DateTime.UtcNow, 
        };

        var addedEntry =  _context.Urls.Add(url);
        await _context.SaveChangesAsync();
        return addedEntry.Entity.Adapt<UrlDto>();
    }

    public async Task<UrlDto?> UpdateUrl(string shortUrl, string fullUrl)
    {
        if (await _context.Urls.FindAsync(shortUrl) is not { } updatedInstance)
            return null;
        new UrlDto{ FullUrl = fullUrl, CreationTime = DateTime.Now, ShortUrl = shortUrl, VisitedTimes = 0 }
            .Adapt(updatedInstance);
        await _context.SaveChangesAsync();
        return updatedInstance.Adapt<UrlDto>();
    }

    public async Task<string?> VisitUrl(string shortUrl)
    {
        if (await _context.Urls.FindAsync(shortUrl) is not { } url)
            return null;
        url.VisitedTimes++;
        await _context.SaveChangesAsync();
        return url.FullUrl;
    }
}