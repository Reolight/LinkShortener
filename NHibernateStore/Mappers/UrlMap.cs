using LinkShortenerStore.Entities;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace LinkShortenerStore.Mappers;

internal class UrlMap : ClassMapping<Url>
{
    public UrlMap()
    {
        Id(url => url.ShortUrl);
        Property(url => url.FullUrl);
        Property(url => url.VisitedTimes);
        Property(url => url.CreationTime);
        Table("urls");
    }
}