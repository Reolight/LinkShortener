using LinkShortenerCore.Model;
using LinkShortenerStore.Entities;

namespace LinkShortenerStore;

public static class Adapters
{
    internal static UrlDto AdaptToDto(this Url urlEntity)
        => new()
        {
            ShortUrl = urlEntity.ShortUrl,
            FullUrl = urlEntity.FullUrl,
            VisitedTimes = urlEntity.VisitedTimes,
            CreationTime = urlEntity.CreationTime
        };

    internal static Url AdaptToEntity(this UrlDto urlDto)
        => new()
        {
            ShortUrl = urlDto.ShortUrl,
            FullUrl = urlDto.FullUrl,
            VisitedTimes = urlDto.VisitedTimes,
            CreationTime = urlDto.CreationTime
        };

    internal static Url AdaptToEntity(this UrlDto urlDto, Url updateUrl)
    {
        (updateUrl.CreationTime, updateUrl.FullUrl, updateUrl.VisitedTimes, updateUrl.ShortUrl)
            = (urlDto.CreationTime, urlDto.FullUrl, urlDto.VisitedTimes, urlDto.ShortUrl);
        return updateUrl;
    }
}