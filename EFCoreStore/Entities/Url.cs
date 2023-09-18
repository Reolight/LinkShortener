using System.ComponentModel.DataAnnotations;

namespace EFCoreStore.Entities;

public class Url
{
    [Key]
    public string ShortUrl { get; set; }
    public string FullUrl { get; set; }
    public int VisitedTimes { get; set; }
    public DateTime CreationTime { get; set; }
}