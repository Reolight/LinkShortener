using EFCoreStore.Entities;
using LinkShortenerCore.Model;
using LinkShortenerCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreStore;

public class EfCoreRepository : IUrlRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<EfCoreRepository> _logger;

    public EfCoreRepository(AppDbContext context, ILogger<EfCoreRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Запрашивает информацию из БД о конкретной сокращённой ссылке без инкремента счётчика переходов.
    /// </summary>
    /// <param name="shortUrl">Требуемая сокращённая ссылка</param>
    /// <returns>Объект с информацией о требуемой сокращённой ссылке. Если ссылка не найдена, возвращает null</returns>
    public async Task<UrlDto?> GetUrlInfo(string shortUrl) => 
        (await _context.Urls.FindAsync(shortUrl))?.AdaptToDto();

    /// <summary>
    /// Запрашивает из БД все существующие сокращённые ссылки (без pagination)
    /// </summary>
    /// <returns>Список с объектами, содержащими информацию о сокращённых ссылках</returns>
    public async Task<IEnumerable<UrlDto>> GetAllUrlInfo() =>
        await _context.Urls
            .AsNoTracking()
            .Select(url => url.AdaptToDto())
            .ToListAsync();

    /// <summary>
    /// Удаляет из базы данных сокращённую ссылку
    /// </summary>
    /// <param name="shortUrl">Удаляемая сокращённая ссылка</param>
    /// <returns>True, если успешно удалено, иначе false (в т.ч. если ссылка уже удалёна)</returns>
    public async ValueTask<bool> RemoveUrl(string shortUrl)
    {
        if (await _context.Urls.FindAsync(shortUrl) is not { } urlToRemove)
            return false;
        var removedEntry = _context.Urls.Remove(urlToRemove);
        await _context.SaveChangesAsync();
        _logger.LogWarning("Short url [{ShortUrl}] for url [{FullUrl}] was removed",
            removedEntry.Entity.ShortUrl, removedEntry.Entity.FullUrl);
        return true;
    }

    /// <summary>
    /// Добавляет сокращённую ссылку для перехода на сокращаемую 
    /// </summary>
    /// <param name="shortUrl">Сокращённая ссылка</param>
    /// <param name="fullUrl">Сокращаемая ссылка</param>
    /// <returns>Объект с информацией о новой созданной ссылке</returns>
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
        _logger.LogInformation("Created new short url [{ShortUrl}] for [{FullUrl}]",
            addedEntry.Entity.ShortUrl, addedEntry.Entity.FullUrl);
        return addedEntry.Entity.AdaptToDto();
    }

    
    /// <summary>
    /// Обновляет полную ссылку, но не изменяет короткую. Обновляет дату создания и обнуляет счётчик переходов.
    /// (Проверки на то, что было и что стало нет, потому что приложение не "комбайн"
    /// и не учитывает вариант подмены ссылки на сайт с казино)
    /// </summary>
    /// <param name="shortUrlToUpdate">Короткая ссылка, у которой подменяется полная</param>
    /// <param name="newFullUrl">Новая ссылка, на которую будет редиректить данная короткая</param>
    /// <returns>Обновлённый объект с информацией о ссылке</returns>
    public async Task<UrlDto?> UpdateUrl(string shortUrlToUpdate, string newFullUrl)
    {
        if (await _context.Urls.FindAsync(shortUrlToUpdate) is not { } updatedInstance)
            return null;
        new UrlDto{ FullUrl = newFullUrl, CreationTime = DateTime.Now, ShortUrl = shortUrlToUpdate, VisitedTimes = 0 }
            .AdaptToEntity(updatedInstance);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Url was updated: []");
        return updatedInstance.AdaptToDto();
    }

    /// <summary>
    /// Запрашивает из БД полную ссылку по сокращённой и инкрементирует счётчик переходов.
    /// </summary>
    /// <param name="shortUrl">Посещаемая сокращённая ссылка</param>
    /// <returns>Полную ссылку соответствующую данной сокращённой</returns>
    public async Task<string?> VisitUrl(string shortUrl)
    {
        if (await _context.Urls.FindAsync(shortUrl) is not { } url)
            return null;
        url.VisitedTimes++;
        await _context.SaveChangesAsync();
        return url.FullUrl;
    }
}