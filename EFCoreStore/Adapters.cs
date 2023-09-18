using EFCoreStore.Entities;
using LinkShortenerCore.Model;

namespace EFCoreStore;

public static class Adapters
{
    public static UrlDto AdaptToDto(this Url urlEntity)
        => new()
        {
            ShortUrl = urlEntity.ShortUrl,
            FullUrl = urlEntity.FullUrl,
            VisitedTimes = urlEntity.VisitedTimes,
            CreationTime = urlEntity.CreationTime
        };

    public static Url AdaptToEntity(this UrlDto urlDto)
        => new()
        {
            ShortUrl = urlDto.ShortUrl,
            FullUrl = urlDto.FullUrl,
            VisitedTimes = urlDto.VisitedTimes,
            CreationTime = urlDto.CreationTime
        };

    public static Url AdaptToEntity(this UrlDto urlDto, Url updateUrl)
    {
        (updateUrl.CreationTime, updateUrl.FullUrl, updateUrl.VisitedTimes, updateUrl.ShortUrl)
            = (urlDto.CreationTime, urlDto.FullUrl, urlDto.VisitedTimes, urlDto.ShortUrl);
        return updateUrl;
    }
}