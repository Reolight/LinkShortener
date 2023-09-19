namespace LinkShortenerStore.Entities;

internal class Url
{
    public virtual string ShortUrl { get; set; }
    public virtual string FullUrl { get; set; }
    public virtual int VisitedTimes { get; set; }
    public virtual DateTime CreationTime { get; set; }
}