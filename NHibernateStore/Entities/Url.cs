namespace LinkShortenerStore.Entities;

internal class Url
{
    public Guid ShortUrl { get; set; }
    public Uri FullUrl { get; set; }
    public int VisitedTimes { get; set; }
    public DateTime CreationTime { get; set; }
}