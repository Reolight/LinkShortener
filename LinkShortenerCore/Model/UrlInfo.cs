﻿namespace LinkShortenerCore.Model;

public class UrlDto
{
    public string ShortUrl { get; set; }
    public string FullUrl { get; set; }
    public int VisitedTimes { get; set; }
    public DateTime CreationTime { get; set; }
}